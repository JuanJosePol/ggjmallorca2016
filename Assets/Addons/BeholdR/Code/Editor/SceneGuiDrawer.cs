using UnityEngine;
using UnityEditor;

namespace Beholder.Editor
{
    /// <summary>
    /// Responsible for drawing the Scene GUI for BeholdR
    /// </summary>
    [InitializeOnLoad]
    public static class SceneGuiDrawer
    {
        #region Constructors
        static SceneGuiDrawer()
        {
            _linkIcon = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "Link.png");
            _unlinkIcon = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "Unlink.png");
            _includeIcon = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "Include.png");
            _excludeIcon = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "Exclude.png");
            SceneView.onSceneGUIDelegate += Draw;
        }
        #endregion

        #region Data Members
        private static Rect _controlsGuiRect;
        private static Texture2D _linkIcon;
        private static Texture2D _unlinkIcon;
        private static Texture2D _includeIcon;
        private static Texture2D _excludeIcon;

        #region GUI definitions
        private const float GUI_AREA_MARGIN = 10f;
        private const float HEIGHT = 36f;
        private const float WIDTH = 68f;
        private const float ICON_SIZE = 28;
        private const float UNITY_GIZMO_WIDTH = 80f;
        private const float CAMERA_PREVIEW_NORMALIZED = 0.2f;
        private const float CAMERA_PREVIEW_OFFSET = 10f;
        #endregion
        #endregion

        #region Public Methods
        /// <summary>
        /// Draws the in-scene GUI controls for the currently active BeholdR component
        /// </summary>
        /// <param name="sceneView">The currently drawing scene view</param>
        public static void Draw(SceneView sceneView)
        {
            if (BeholdR.ActiveInstance == null || !BeholdR.ActiveInstance.ShowGuiControls || Utilities.TestSuppressionNeeded(sceneView))
                return;

            _controlsGuiRect = CalculateGuiRect(BeholdR.ActiveInstance.GuiAnchor, sceneView.camera);

            Handles.BeginGUI();
            {
                GUILayout.BeginArea(_controlsGuiRect, GUI.skin.box);
                {
                    GUILayout.BeginHorizontal();
                    {
                        DrawViewLinkControl(sceneView);
                        DrawFilterControl(sceneView);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// calculate the area in which we draw the GUI, considering the current anchor option
        /// </summary>
        /// <param name="anchor">In what area of the scene view we should draw</param>
        /// <param name="sceneCamera">The camera of the scene view in which we render the GUI</param>
        /// <returns>the GUI area in the appropriate anchor position</returns>
        private static Rect CalculateGuiRect(TextAnchor anchor, Camera sceneCamera)
        {
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    return new Rect(GUI_AREA_MARGIN, GUI_AREA_MARGIN, WIDTH, HEIGHT);
                case TextAnchor.UpperCenter:
                    return new Rect((sceneCamera.pixelWidth / 2f) - (WIDTH / 2f), GUI_AREA_MARGIN, WIDTH, HEIGHT);
                case TextAnchor.UpperRight:
                    return new Rect((sceneCamera.pixelWidth - WIDTH - GUI_AREA_MARGIN) - UNITY_GIZMO_WIDTH, GUI_AREA_MARGIN, WIDTH, HEIGHT);
                case TextAnchor.MiddleLeft:
                    return new Rect(GUI_AREA_MARGIN, (sceneCamera.pixelHeight / 2f) - (HEIGHT / 2f), WIDTH, HEIGHT);
                case TextAnchor.MiddleCenter:
                    return new Rect((sceneCamera.pixelWidth / 2f) - (WIDTH / 2f), (sceneCamera.pixelHeight / 2f) - (HEIGHT / 2f), WIDTH, HEIGHT);
                case TextAnchor.MiddleRight:
                    return new Rect((sceneCamera.pixelWidth - WIDTH - GUI_AREA_MARGIN), (sceneCamera.pixelHeight / 2f) - (HEIGHT / 2f), WIDTH, HEIGHT);
                case TextAnchor.LowerLeft:
                    return new Rect(GUI_AREA_MARGIN, (sceneCamera.pixelHeight - HEIGHT - GUI_AREA_MARGIN), WIDTH, HEIGHT);
                case TextAnchor.LowerCenter:
                    return new Rect((sceneCamera.pixelWidth / 2f) - (WIDTH / 2f), (sceneCamera.pixelHeight - HEIGHT - GUI_AREA_MARGIN), WIDTH, HEIGHT);
                case TextAnchor.LowerRight:
                    return new Rect((sceneCamera.pixelWidth - WIDTH - GUI_AREA_MARGIN), (sceneCamera.pixelHeight - HEIGHT - GUI_AREA_MARGIN), WIDTH, HEIGHT);
                default:
                    goto case TextAnchor.UpperLeft;
            }
        }

        /// <summary>
        /// Draw a button to link/unlink the game camera with the given scene view
        /// </summary>
        /// <param name="sceneView">The currently drawing scene view</param>
        private static void DrawViewLinkControl(SceneView sceneView)
        {
            Color bkp = GUI.color;
            bool linked = BeholdR.ActiveInstance.LinkedSceneView == sceneView;

            if (linked)
                GUI.color = Color.red;

            if(GUILayout.Button(linked ? _unlinkIcon : _linkIcon, GUILayout.Width(ICON_SIZE), GUILayout.Height(ICON_SIZE))) {
                BeholdR.ActiveInstance.ToggleLinkedView(sceneView);
                SceneView.RepaintAll();
            }

            GUI.color = bkp;
        }

        /// <summary>
        /// Draw a button to add/remove the drawing scene view from the filtered view set of BeholdR
        /// </summary>
        /// <param name="sceneView">The currently drawing scene view</param>
        private static void DrawFilterControl(SceneView sceneView)
        {
            Color bkp = GUI.color;
            bool excluded = BeholdR.ActiveInstance.IsViewExcluded(sceneView);

            if (excluded)
                GUI.color = Color.red;

            if (GUILayout.Button(excluded ? _includeIcon : _excludeIcon, GUILayout.Width(ICON_SIZE), GUILayout.Height(ICON_SIZE)))
                BeholdR.ActiveInstance.ToggleViewExclusion(sceneView);

            GUI.color = bkp;
        }
        #endregion
    }
}
