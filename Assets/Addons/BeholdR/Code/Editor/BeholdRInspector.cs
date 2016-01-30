using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Beholder.Editor
{
	[CustomEditor(typeof(BeholdR))]
	public class BeholdrInspector : UnityEditor.Editor
	{
		private BeholdR _beholdr;
		private ReorderableList _reordableFxList;
		private bool _isListVisible = true;

		private Texture2D _icon;
		private Texture2D Icon { get { return _icon ?? (_icon = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "BeholdR.png")); } }

		private GUIStyle _foldStyle;
		private string _presetName = "PostEffectsSet";
		private VersionChecker _versionChecker;

		#region Sections Visibility
		private static bool _isControlSectionVisible = true;
		private static bool _isLinkSectionVisible = true;
		private static bool _isPresetSectionVisible = true;
		private static bool _isSyncSectionVisible = true;
		#endregion

		#region Unity Overrides
		void OnEnable()
		{
			_beholdr = target as BeholdR;
            _versionChecker = new VersionChecker();
			InitializeReordableList();
		}

		public override void OnInspectorGUI()
		{
			DrawAboutSection();
			DrawVersionSection();
			DrawControlSection();
			DrawLinkSection();
			DrawPresetSection();
			DrawSyncSection();

			serializedObject.ApplyModifiedProperties();
		}
		#endregion
		
		#region Sections Draw Methods
		private void DrawAboutSection()
		{
			EditorGUILayout.BeginHorizontal("box");
			{
				GUILayout.Label(Icon, GUILayout.Width(Icon.width), GUILayout.Height(Icon.height));
				GUILayout.Label("BeholdR v" + VersionChecker.LOCAL_VERSION + "\n(c) Virtual Mirror Game Studios ltd. 2011-" + DateTime.UtcNow.Year, EditorStyles.miniLabel);
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawVersionSection()
		{
			if (_versionChecker.RemoteVersion > _versionChecker.LocalVersion)
				EditorGUILayout.HelpBox("Your version of BeholdR may be outdated! The newest available version is " + _versionChecker.RemoteVersionString, MessageType.Warning);
		}

		private void DrawControlSection()
		{
			EditorGUILayout.BeginVertical("box");
			{
				if (!DrawToggledHeader("Control Panel", ref _isControlSectionVisible)) {
					EditorGUILayout.EndVertical();
					return;
				}

				bool _oldShow = _beholdr.ShowGuiControls;
				_beholdr.ShowGuiControls = EditorGUILayout.Toggle("Show Controls", _beholdr.ShowGuiControls);
				if (_oldShow != _beholdr.ShowGuiControls)
					SceneView.RepaintAll();

				_beholdr.GuiAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Controls Anchor", _beholdr.GuiAnchor);
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawLinkSection()
		{
			EditorGUILayout.BeginVertical("box");
			{
				if (!DrawToggledHeader("Scene Link", ref _isLinkSectionVisible)) {
					EditorGUILayout.EndVertical();
					return;
				}

				GUI.enabled = false;
				EditorGUILayout.Toggle("Linked to a Scene?", _beholdr.LinkedSceneView != null);
				GUI.enabled = true;

				bool lastMatch = _beholdr.MatchCameraColor;
				_beholdr.MatchCameraColor = EditorGUILayout.Toggle("Match Background Color", _beholdr.MatchCameraColor);

				if (_beholdr.MatchCameraColor && !lastMatch)
					BeholdR.CacheSceneColor();
				else if (!_beholdr.MatchCameraColor && lastMatch)
					BeholdR.ResetSceneColor();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawPresetSection()
		{
			EditorGUILayout.BeginVertical("box");
			{
				if (!DrawToggledHeader("Presets", ref _isPresetSectionVisible)) {
					EditorGUILayout.EndVertical();
					return;
				}

				_presetName = EditorGUILayout.TextField("Preset Name", _presetName);

				EditorGUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Save Preset"))
						PresetManager.Create(_beholdr, _presetName);

					if (GUILayout.Button("Load Preset"))
						PresetManager.Load(_beholdr, Utilities.PRESETS_SUBDIR + EditorUtility.OpenFilePanel("Select preset...", Utilities.PresetsPathAbsolute, "prefab").Split('/').Last());
					}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawSyncSection()
		{
			EditorGUILayout.BeginVertical("box");
			{
				if (!DrawToggledHeader("Post Effects", ref _isSyncSectionVisible)) {
					EditorGUILayout.EndVertical();
					return;
				}

				if(_beholdr.IsSupressed()) {
					EditorGUILayout.HelpBox(Utilities.SUPPRESSION_MESSAGE, MessageType.Error);
					EditorGUILayout.EndVertical();
					return;
				}

				BeholdR.AutoDiscover = EditorGUILayout.Toggle("Auto Discover", BeholdR.AutoDiscover);
				BeholdR.AutoDisableInPlayMode = EditorGUILayout.Toggle("Auto Disable in Play", BeholdR.AutoDisableInPlayMode);

				if (DrawToggledHeader("Effects List", ref _isListVisible, false))
					_reordableFxList.DoList(GUILayoutUtility.GetRect(GUILayoutUtility.GetLastRect().width, _reordableFxList.GetHeight()));

				EditorGUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Discover FX"))
						_beholdr.DiscoverEffects();

					if (GUILayout.Button("Clean Missing"))
						_beholdr.PostEffects.RemoveAll(fx => fx == null);

					if (GUILayout.Button("Reorder List")) {
						_beholdr.PostEffects.Clear();
						_beholdr.DiscoverEffects();
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Draws a folding header for the GUI section that the user can click on to fold/unfold the section
		/// </summary>
		/// <param name="header">Section name</param>
		/// <param name="isFolded">The folded state of the section right now</param>
		/// <param name="isBold">Should the header text be bold?</param>
		/// <returns>The folded state of the section after user interaction</returns>
		private bool DrawToggledHeader(string header, ref bool isFolded, bool isBold = true)
		{
			// set styling for bold label
			_foldStyle = new GUIStyle(GUI.skin.FindStyle("Foldout")) {
				richText = true,
			};

			if (isBold)
				header = "<b>" + header + "</b>";

			isFolded = GUILayout.Toggle(isFolded, header, _foldStyle);
			return isFolded;
		}
		#endregion

		#region Reorderable List
		private void InitializeReordableList()
		{
			_reordableFxList = new ReorderableList(serializedObject, serializedObject.FindProperty("PostEffects"), true, true, true,
				true)
			{
				drawHeaderCallback = DrawListHeader,
				drawElementCallback = DrawListElement,
				onReorderCallback = ReorderActualEffects,
			};
		}

		/// <summary>
		/// Mark that we need to reorder the components on the object to reflect a change in the list
		/// </summary>
		/// <param name="list"></param>
		private void ReorderActualEffects(ReorderableList list)
		{
			_beholdr.IsOrderRequired = true;
		}

		/// <summary>
		/// Draw each of the list's elements as an assignable property field
		/// </summary>
		/// <param name="rect">The drawing area</param>
		/// <param name="index">The index of the element in the list</param>
		/// <param name="isactive">Is this the active element</param>
		/// <param name="isfocused">Is this the focused element</param>
		private void DrawListElement(Rect rect, int index, bool isactive, bool isfocused)
		{
			// fixing slight height issue
			rect.y += 2;
			rect.height = EditorGUIUtility.singleLineHeight;

			// draw property in list
			SerializedProperty element = serializedObject.FindProperty("PostEffects").GetArrayElementAtIndex(index);
			string label = _beholdr.PostEffects[index] != null ? _beholdr.PostEffects[index].GetType().Name : string.Empty;
			EditorGUI.PropertyField(rect, element, new GUIContent(label));
		}

		/// <summary>
		/// Draw the header line of the reorder-able post effects list
		/// </summary>
		/// <param name="rect">The rectangle of the GUI content of the list's header</param>
		private void DrawListHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Post Effects", EditorStyles.boldLabel);
		}
		#endregion
	}
}
