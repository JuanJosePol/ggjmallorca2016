using UnityEditor;
using UnityEngine;

namespace Beholder.Editor
{
	/// <summary>
	/// Responsible for drawing the BeholdR icon in the Hierarchy window
	/// The icon will appear on every object that has a BeholdR component on it
	/// The icon will also serve as a quick button to enable/disable the BeholdR component
	/// 
	/// This class will be instantiated by Unity every time the Editor starts
	/// </summary>
	[InitializeOnLoad]
	public class BeholdRHierarchyIcon
	{
		/// <summary>
		/// Static constructor called by Unity when the Editor starts
		/// </summary>
		static BeholdRHierarchyIcon()
		{
			FindHierarchyIcons();
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyGuiCallback;
		}

		private static Texture2D _iconActive;
		private static Texture2D _iconInactive;
		private const int PADDING = 2;

		/// <summary>
		/// Looks for the BeholdR icons in the Project files
		/// </summary>
		static void FindHierarchyIcons()
		{
			_iconActive = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "BeholdR.png");
			_iconInactive = Utilities.GetAsset<Texture2D>(Utilities.ICONS_SUBDIR + "BeholdR_bw.png");
		}

		/// <summary>
		/// Callback from the Unity Editor for each visible object in the Hierarchy window
		/// </summary>
		/// <param name="instanceId">Equivalent to calling <see cref="Object.GetInstanceID"/>, 
		/// this can be used to get a reference for the object via <see cref="EditorUtility.InstanceIDToObject"/></param>
		/// <param name="itemRect">Passed from Unity, this is the area in which the Hierarchy item is being drawn</param>
		static void HierarchyGuiCallback(int instanceId, Rect itemRect)
		{
			if(_iconActive == null || _iconInactive == null)
			{
				FindHierarchyIcons();
				return;
			}

			GameObject hierarchyObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

			if(hierarchyObject == null) return;
			BeholdR beholdRComponent = hierarchyObject.GetComponent<BeholdR>();
			if(beholdRComponent != null)
			{
				Rect iconRect = itemRect;
				iconRect.x = iconRect.width - PADDING;
				iconRect.width = iconRect.height + PADDING * 2;

				if(GUI.Button(iconRect, beholdRComponent.enabled ? _iconActive : _iconInactive, "label"))
				{
					beholdRComponent.enabled = !beholdRComponent.enabled;
				}
			}
		}
	}
}
