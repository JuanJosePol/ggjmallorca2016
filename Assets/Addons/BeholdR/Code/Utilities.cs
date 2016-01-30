using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Beholder
{
    /// <summary>
    /// Contains various utility methods and constants for BeholdR
    /// </summary>
    public static class Utilities
    {
#if UNITY_EDITOR
        #region Constants
        public const BindingFlags DEFAULT_BINDING_FLAG = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        public const string ASSETS_SUBDIR = "Assets/";
        public const string ICONS_SUBDIR = "Icons/";
        public const string PRESETS_SUBDIR = "Presets/";

        public static string PresetsPathAbsolute = BeholdrAbsolutePath + ASSETS_SUBDIR + PRESETS_SUBDIR;
        public static string PresetsPathRelative = BeholdrRelativePath + ASSETS_SUBDIR + PRESETS_SUBDIR;

        public const string SUPPRESSION_MESSAGE =
            "BeholdR is currently suppressed because of conflicts with Scene View filtering or Render Mode.\n" +
            "BeholdR will re-synchronize as soon as the filter is removed and Render Mode is set to Shaded.";
        #endregion

        #region Data Members
        private static string _relativePath;
        public static string BeholdrRelativePath {
            get {
                if(string.IsNullOrEmpty(_relativePath))
                    GetBeholdrRelativePath(Application.dataPath, ref _relativePath);

                return _relativePath;
            }
        }

        private static string _absolutePath;
        public static string BeholdrAbsolutePath {
            get {
                if (string.IsNullOrEmpty(_absolutePath))
                    GetBeholdrAbsolutePath(ref _absolutePath);

                return _absolutePath;
            }
        }

        private readonly static Dictionary<string, Type> TypesCache = new Dictionary<string, Type>();
        #endregion

        #region Extension Methods
        public static void MatchTo(this Transform self, Transform target)
        {
            self.position   = target.position;
            self.rotation   = target.rotation;
            self.localScale = target.localScale;
        }

        /// <summary>
        /// Return the first instance of the component of type <see cref="type"/> found on the game object
        /// ---- NOTE: THIS METHOD IS OBSOLETE IN UNITY 5 ----
        /// </summary>
        /// <param name="self">the game object we're looking at</param>
        /// <param name="type">the type of the component we're looking for</param>
        /// <returns>the first instance of the component whose type equals the desired type, or null if none were found</returns>
        public static Component GetComponent(this GameObject self, Type type)
        {
            foreach(Component comp in self.GetComponents<Component>())
                if(comp.GetType() == type)
                    return comp;

            return null;
        }

        /// <summary>
        /// Check to see if the <see cref="source"/> contains any element that is not null
        /// </summary>
        /// <param name="source">The collection we want to check</param>
        /// <returns>True if there is any element in the collection that is not null</returns>
        public static bool AnyConcrete(this IList source)
        {
            if(source.Count == 0)
                return false;

            foreach(object element in source)
                if(element != null)
                    return true;

            return false;
        }
        #endregion

        #region Public API
        /// <returns>True if component on scene camera is of a forbidden type</returns>
        public static bool IsForbiddenComponent(Camera sceneCamera = null, Component component = null)
        {
            return 
               (component == null ||
                component is Transform ||
                component is Camera ||
                component is BeholdR ||
                component is BeholdR ||
                component is FlareLayer ||
                component.GetType().Name.Equals("Tonemapping") ||
                sceneCamera != null && component == sceneCamera.GetComponent("HaloLayer"));
        }

        /// <summary>
        /// Looks for a type with the specified type name in the currently loaded assemblies
        /// </summary>
        /// <param name="typeName">The full name of the type we are looking for</param>
        /// <returns>The Type, if found</returns>
        public static Type FindTypeByName(string typeName)
        {
            Type t;
            if(!TypesCache.TryGetValue(typeName, out t)) {
                t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => type.FullName == typeName);

                TypesCache[typeName] = t;
            }

            return t;
        }

        /// <summary>
        /// Checks all open inspectors to see if there is an opened inspector of the given type
        /// </summary>
        /// <param name="typeName">The type of inspector we are looking for by name</param>
        /// <returns>true if there is any inspector window with an open inspector of the given type</returns>
        public static bool IsEditorVisible(string typeName)
        {
            bool visible = false;

            Type inspectorT = FindTypeByName("UnityEditor.InspectorWindow");

            if(inspectorT == null) return false;

            // iterate on all inspector windows
            Object[] inspectors = Resources.FindObjectsOfTypeAll(inspectorT);
            foreach(Object inspector in inspectors) {
                // get the inspector tracker
                FieldInfo fieldInfo = inspectorT.GetField("m_Tracker", DEFAULT_BINDING_FLAG);
                if(fieldInfo == null) return false;

                object tracker = fieldInfo.GetValue(inspector);
                if(tracker == null) continue;

                // look at the open (active) inspectors for the provided type
                object shared = tracker.GetType().GetProperty("activeEditors").GetValue(tracker, null);
                foreach(object thing in (IEnumerable)shared)
                {
                    if(thing.GetType().FullName != typeName) continue;

                    object target = thing.GetType().GetProperty("target").GetValue(thing, null);
                    visible = InternalEditorUtility.GetIsInspectorExpanded(target as Object);
                }
            }

            return visible;
        }

        /// <summary>
        /// Loads and returns the first asset with the given name found somewhere in the InvokR assets folder
        /// </summary>
        /// <typeparam name="T">The type of the object that will be loaded from the asset</typeparam>
        /// <param name="relativePath">The relative path to the asset under the BeholdR/Assets folder</param>
        /// <returns>An object of type T loaded from the asset file</returns>
        public static T GetAsset<T>(string relativePath) where T : Object
        {
            try {
                return (T)AssetDatabase.LoadAssetAtPath(string.Concat(BeholdrRelativePath, ASSETS_SUBDIR, relativePath), typeof(T));
            }
            catch (Exception) { }

            return null;
        }

        /// <summary>
        /// Tests to see if we need to suppress BeholdR in order to prevent crash
        /// </summary>
        public static bool TestSuppressionNeeded(SceneView view)
        {
            if (view.renderMode != DrawCameraMode.Textured)
                return true;

            MethodInfo useFilteringMethod = view.GetType().GetMethod("UseSceneFiltering", DEFAULT_BINDING_FLAG);
            bool useFilteringValue = useFilteringMethod != null && (bool)useFilteringMethod.Invoke(view, null);
            if (useFilteringValue)
                return true;

            return false;
        }

        /// <summary>
        /// Sets the visibility of object gizmos in the scene.
        /// Based on Immanuel Scholz's answer @ http://answers.unity3d.com/answers/837370/view.html
        /// </summary>
        public static void ToggleSceneGizmos(bool areGizmosVisible)
        {
            Type Annotation = FindTypeByName("UnityEditor.Annotation");
            FieldInfo ClassId = Annotation.GetField("classID");
            FieldInfo ScriptClass = Annotation.GetField("scriptClass");

            Type AnnotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
            MethodInfo GetAnnotations = AnnotationUtility.GetMethod("GetAnnotations", DEFAULT_BINDING_FLAG);
            MethodInfo SetGizmoEnabled = AnnotationUtility.GetMethod("SetGizmoEnabled", DEFAULT_BINDING_FLAG);
            MethodInfo SetIconEnabled = AnnotationUtility.GetMethod("SetIconEnabled", DEFAULT_BINDING_FLAG);

            Array annotations = (Array)GetAnnotations.Invoke(null, null);
            foreach (var annotatedObject in annotations)
            {
                int classId = (int)ClassId.GetValue(annotatedObject);
                string scriptClass = (string)ScriptClass.GetValue(annotatedObject);
                int visibility = areGizmosVisible ? 1 : 0;

                SetGizmoEnabled.Invoke(null, new object[] { classId, scriptClass, visibility });
                SetIconEnabled .Invoke(null, new object[] { classId, scriptClass, visibility });
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Perform a recursive searching the project folder in a depth-first manner 
        /// until we find the relative root BeholdR folder path
        /// </summary>
        /// <param name="rootPath">The current root folder in which we're searching</param>
        /// <param name="beholdrPath">This is a REF parameter that will be set to the path of the local BeholdR root folder</param>
        private static void GetBeholdrRelativePath(string rootPath, ref string beholdrPath)
        {
            if (!string.IsNullOrEmpty(beholdrPath))
                return;

            foreach (string subDirPath in Directory.GetDirectories(rootPath)){
                if (!string.IsNullOrEmpty(beholdrPath))
                    break;

                string localPath = subDirPath.Replace('\\', '/').Split(new[] { "Assets/" }, StringSplitOptions.RemoveEmptyEntries).Last();
                if (localPath.Contains("BeholdR")) {
                    beholdrPath = "Assets/" + localPath + "/";
                    return;
                }

                GetBeholdrRelativePath(subDirPath, ref beholdrPath);
            }
        }

        private static void GetBeholdrAbsolutePath(ref string absolutePath)
        {
            if (!string.IsNullOrEmpty(absolutePath))
                return;

            absolutePath = Application.dataPath.Replace("Assets", "") + BeholdrRelativePath;
        }
        #endregion
#endif
    }
}