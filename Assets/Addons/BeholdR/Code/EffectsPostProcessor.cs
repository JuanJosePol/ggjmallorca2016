using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Beholder
{
	/// <summary>
	/// Sometimes the default copied values don't work properly, so additional work is required to synchronize correctly.
	/// This class contains the methods to do this extra work.
	/// </summary>
	public static class EffectsPostProcessor
	{
#if UNITY_EDITOR
		private delegate void PostProcessEffectMethod(Component gameComponent, Component sceneComponent);
		
		#region Class Data Members
		private static Dictionary<string, PostProcessEffectMethod> _postProcessMethods = new Dictionary<string, PostProcessEffectMethod> {
			{ "DepthOfField", ProcessDepthOfField }
		};
		#endregion

		#region Public Methods
		public static void Process(string componentTypeName, Component gameComponent, Component sceneComponent)
		{
			PostProcessEffectMethod processMethod;
			if (_postProcessMethods.TryGetValue(componentTypeName, out processMethod))
				processMethod.Invoke(gameComponent, sceneComponent);
		}
		#endregion

		#region PostProcessMethods
		private static void ProcessDepthOfField(Component gameComponent, Component sceneComponent)
		{
			Type dofType = sceneComponent.GetType();
			FieldInfo fPlaneField = dofType.GetField("focusPlane", Utilities.DEFAULT_BINDING_FLAG);
			if (fPlaneField == null) return;

			float currentValue = (float)fPlaneField.GetValue(sceneComponent);
			float factor = Mathf.Pow(Mathf.Pow(currentValue, 4) / (sceneComponent.GetComponent<Camera>().farClipPlane / gameComponent.GetComponent<Camera>().farClipPlane), 1.0f / 4);
			fPlaneField.SetValue(sceneComponent, factor);
		}
		#endregion
#endif
	}
}