using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beholder
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class BeholdR : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Fields & Properties
        public static BeholdR ActiveInstance;
        public List<Component> PostEffects = new List<Component>();
        private Camera _camera;

        public bool IsOrderRequired;
        public bool ShowGuiControls = true;
        public TextAnchor GuiAnchor = TextAnchor.UpperLeft;
        public SceneView LinkedSceneView;
        private static readonly HashSet<SceneView> ExcludedSceneViews = new HashSet<SceneView>();

        #region  Scene Color Matching
        public bool MatchCameraColor;

        private static object _sceneColorField;
        private static PropertyInfo _sceneColorProperty;
        private static PropertyInfo SceneColorProperty
        {
            get
            {
                if (_sceneColorProperty == null)
                {
                    Type svType = Utilities.FindTypeByName("UnityEditor.SceneView");
                    FieldInfo cField = svType.GetField("kSceneViewBackground", Utilities.DEFAULT_BINDING_FLAG);
                    if (cField == null)
                        return null;

                    _sceneColorField = cField.GetValue(null);
                    if (_sceneColorField == null)
                        return null;

                    Type bgColorType = _sceneColorField.GetType();
                    _sceneColorProperty = bgColorType.GetProperty("Color");
                }

                return _sceneColorProperty;
            }
        }

        private static Color _cachedSceneColor;
        #endregion        

        #region Automatic Disable
        private const string AUTO_DISABLE_PKEY = "BeholdR_IsAutoDisableActive";
        public static bool AutoDisableInPlayMode {
            get { return EditorPrefs.GetBool(AUTO_DISABLE_PKEY, true); }
            set { EditorPrefs.SetBool(AUTO_DISABLE_PKEY, value); }
        }
        #endregion

        #region Automatic Discovery
        private int _lastComponentCount;

        private const string AUTO_DISCOVER_PKEY = "BeholdR_IsAutoDiscoverActive";
        public static bool AutoDiscover {
            get { return EditorPrefs.GetBool(AUTO_DISCOVER_PKEY, true); }
            set { EditorPrefs.SetBool(AUTO_DISCOVER_PKEY, value); }
        }

        private const string POST_FX_ATTRIBUTE_KEY = "Image Effects";
        private static readonly List<Type> PostFxBaseTypes = new List<Type> {
            Utilities.FindTypeByName("ImageEffectBase"),
            Utilities.FindTypeByName("PostEffectsBase")
        };
        #endregion

        #region Gizmo Visibility
        private const string GIZMO_HIDDEN_PKEY = "BeholdR_AreGizmosHidden";
        public static bool AreGizmosHidden {
            get { return EditorPrefs.GetBool(GIZMO_HIDDEN_PKEY, false); }
            set {
                EditorPrefs.SetBool(GIZMO_HIDDEN_PKEY, value);
                Utilities.ToggleSceneGizmos(!value);
            }
        }
        #endregion
        #endregion

        #region PostProcessing
        private HashSet<string> _ppRequired = new HashSet<string> {
            "DepthOfField",
        };
        #endregion

        #region Unity Overrides
        void OnEnable()
        {
            BeholdR[] all = FindObjectsOfType<BeholdR>();
            for (int i = 0; i < all.Length; i++)
                all[i].enabled = all[i] == this;

            ActiveInstance = this;
            _camera = GetComponent<Camera>();
            CacheSceneColor();
            AreGizmosHidden = AreGizmosHidden;
            EditorApplication.update += BUpdate;

#if UNITY_5_3
            Selection.selectionChanged += OnSelectionChanged;
#endif

            OnSelectionChanged();
        }

        void OnDisable()
        {
            if (ActiveInstance == this)
                ActiveInstance = null;

            EditorApplication.update -= BUpdate;

#if UNITY_5_3
            Selection.selectionChanged -= OnSelectionChanged;
#endif

            Tools.hidden = false;
            CleanAllCameras();
            ResetSceneColor();
        }
        #endregion

        #region Callbacks
        void BUpdate()
        {
            if (AutoDiscover)
                DoAutoDiscover();

            SynchronizeToScene();
            SynchronizeFromScene();

            if (IsOrderRequired)
                ReorderActualEffects();
        }

        void OnSelectionChanged()
        {
            Tools.hidden =  Selection.activeGameObject == gameObject && 
                            LinkedSceneView != SceneView.currentDrawingSceneView;
        }
        #endregion

        #region Synchronization to Scene
        private void ReorderActualEffects()
        {
            for (int i = 0; i < PostEffects.Count; i++) {
                Component nEffect = gameObject.AddComponent(PostEffects[i].GetType());
                EditorUtility.CopySerialized(PostEffects[i], nEffect);
                DestroyImmediate(PostEffects[i]);
            }

            PostEffects.RemoveAll(fx => fx == null);
            DiscoverEffects();
            IsOrderRequired = false;
        }

        private void SynchronizeToScene()
        {
            if (AutoDiscover)
                DiscoverEffects();

            if (MatchCameraColor)
                SetSceneColor(_camera.backgroundColor);

            var allViews = SceneView.sceneViews;
            for (int i = 0; i < allViews.Count; i++)
                SyncView((SceneView)allViews[i]);
        }

        private void SyncView(SceneView sceneView)
        {
            if (ShouldSkipView(sceneView))
            {
                CleanCamera(sceneView.camera);
                return;
            }

            CleanCamera(sceneView.camera);
            SyncCamera(sceneView.camera);
        }

        private bool ShouldSkipView(SceneView sceneView)
        {
            return (AutoDisableInPlayMode && EditorApplication.isPlayingOrWillChangePlaymode) ||
                    IsViewExcluded(sceneView) ||
                    Utilities.TestSuppressionNeeded(sceneView);
        }

        private void CleanAllCameras()
        {
            var views = SceneView.sceneViews;
            for (int i = 0; i < views.Count; i++)
                CleanCamera(((SceneView)views[i]).camera);
        }

        private void CleanCamera(Camera camera)
        {
            Component[] cameraComponents = camera.GetComponents<Component>();
            for (int i = 0; i < cameraComponents.Length; i++)
            {
                if (Utilities.IsForbiddenComponent(camera, cameraComponents[i]))
                    continue;

                DestroyImmediate(cameraComponents[i]);
            }
        }

        private void SyncCamera(Camera camera)
        {
            for (int i = 0; i < PostEffects.Count; i++)
            {
                if (PostEffects[i] == null || Utilities.IsForbiddenComponent(camera, PostEffects[i]))
                    continue;

                Component effectComponent = camera.gameObject.AddComponent(PostEffects[i].GetType());
                EditorUtility.CopySerialized(PostEffects[i], effectComponent);

                string componentTypeName = effectComponent.GetType().Name;
                if (_ppRequired.Contains(componentTypeName))
                    EffectsPostProcessor.Process(componentTypeName, PostEffects[i], effectComponent);
            }
        }
        #endregion

        #region Synchronize from Scene
        private void SynchronizeFromScene()
        {
            if (LinkedSceneView != null)
                transform.MatchTo(LinkedSceneView.camera.transform);
        }

        public void ToggleLinkedView(SceneView view)
        {
            if (view == LinkedSceneView)
                LinkedSceneView = null;
            else
                LinkedSceneView = view;
        }
        #endregion

        #region Exclusion
        public void ExcludeView(SceneView view)
        {
            ExcludedSceneViews.Add(view);
        }

        public void IncludeView(SceneView view)
        {
            ExcludedSceneViews.Remove(view);
        }

        public bool IsViewExcluded(SceneView view)
        {
            return ExcludedSceneViews.Contains(view);
        }

        public void ToggleViewExclusion(SceneView view)
        {
            if (IsViewExcluded(view))
                IncludeView(view);
            else
                ExcludeView(view);
        }

        public bool IsSupressed()
        {
            foreach (SceneView sceneView in SceneView.sceneViews)
            {
                if (sceneView.renderMode != DrawCameraMode.Textured)
                    return true;

                MethodInfo useFilteringMethod = sceneView.GetType().GetMethod("UseSceneFiltering", Utilities.DEFAULT_BINDING_FLAG);
                bool useFilteringValue = useFilteringMethod != null && (bool)useFilteringMethod.Invoke(sceneView, null);
                if (useFilteringValue)
                    return true;
            }

            return false;
        }
        #endregion

        #region Discovery
        public void DoAutoDiscover()
        {
            Component[] allComponents = GetComponents<Component>();
            if (allComponents.Length != _lastComponentCount)
                DiscoverEffects();

            _lastComponentCount = allComponents.Length;
        }

        public void DiscoverEffects()
        {
            Component[] allComponents = GetComponents<Component>();
            for (int i = 0; i < allComponents.Length; i++)
                if (IsValidPostEffect(allComponents[i]) && !PostEffects.Contains(allComponents[i]))
                    PostEffects.Add(allComponents[i]);            
        }

        private bool IsValidPostEffect(Component component)
        {
            return !Utilities.IsForbiddenComponent(null, component) &&
                    (TryWithAttribute(component) || TryWithType(component) || TryWithMethod(component));
        }

        /// <summary>
        /// Look at the component's AddComponentMenu attribute for "Image Effects", which is standard for Unity's built-in postFX and many others
        /// </summary>
        /// <param name="postFxCandidate">the component we're currently checking in the <see cref="TryAutoCollectPostFx"/> loop</param>
        /// <returns>True if the component was found to be an Image Effect and was added to the list, false otherwise</returns>
        private bool TryWithAttribute(Component postFxCandidate)
        {
            object[] attributes = postFxCandidate.GetType().GetCustomAttributes(typeof(AddComponentMenu), false);
            if (attributes.Length == 0)
                return false;

            for (int i = 0; i < attributes.Length; i++)
                if (((AddComponentMenu)attributes[i]).componentMenu.Split('/')[0].Contains(POST_FX_ATTRIBUTE_KEY) && !PostEffects.Contains(postFxCandidate))
                    return true;

            return false;
        }

        /// <summary>
        /// Look at the component's type to see if it inherits from <see cref="PostEffectsBase"/> of <see cref="ImageEffectBase"/>
        /// </summary>
        /// <param name="postFxCandidate">the component we're currently checking in the <see cref="TryAutoCollectPostFx"/> loop</param>
        /// <returns></returns>
        private static bool TryWithType(Component postFxCandidate)
        {
            for (int i = 0; i < PostFxBaseTypes.Count; i++)
                if (PostFxBaseTypes[i] != null && postFxCandidate.GetType().IsSubclassOf(PostFxBaseTypes[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Look at the component for the "OnRenderImage" method that is used by image effects
        /// </summary>
        /// <param name="postFxCandidate"></param>
        /// <returns></returns>
        private static bool TryWithMethod(Component postFxCandidate)
        {
            return postFxCandidate.GetType().GetMethod("OnRenderImage", Utilities.DEFAULT_BINDING_FLAG) != null;
        }
        #endregion

        #region Scene Background Color
        /// <summary>
        /// Sets the scene view background color to the cached initial value
        /// </summary>
        public static void ResetSceneColor()
        {
            SceneColorProperty.SetValue(_sceneColorField, _cachedSceneColor, null);
            SceneView.RepaintAll();
        }

        /// <summary>
        /// Save the initial color of the scene view so that we can later reset to it
        /// </summary>
        public static void CacheSceneColor()
        {
            Type svType = Utilities.FindTypeByName("UnityEditor.SceneView");
            FieldInfo cField = svType.GetField("kSceneViewBackground", Utilities.DEFAULT_BINDING_FLAG);
            if (cField == null)
                return;

            object bgColor = cField.GetValue(null);
            if (bgColor == null)
                return;

            Type bgColorType = bgColor.GetType();
            PropertyInfo col = bgColorType.GetProperty("Color");
            if (col == null)
                return;

            _cachedSceneColor = (Color)col.GetValue(bgColor, null);
        }

        private static void SetSceneColor(Color color)
        {
            SceneColorProperty.SetValue(_sceneColorField, color, null);
            SceneView.RepaintAll();
        }
        #endregion
#endif
    }
}
