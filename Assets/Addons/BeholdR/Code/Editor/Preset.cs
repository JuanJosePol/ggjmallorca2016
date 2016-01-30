using System.IO;
using UnityEditor;
using UnityEngine;

namespace Beholder.Editor
{
    public static class PresetManager
    {
        #region Public Methods
        public static void Create(BeholdR source, string presetName)
        {
            if (!Directory.Exists(Utilities.PresetsPathAbsolute))
                Directory.CreateDirectory(Utilities.PresetsPathAbsolute);

            GameObject preset = new GameObject(presetName);
            BeholdR presetBeholdR = preset.AddComponent<BeholdR>();
            presetBeholdR.enabled = false;

            foreach (Component sourceEffect in source.PostEffects) {
                if (sourceEffect == null)
                    continue;

                Component presetEffect = preset.AddComponent(sourceEffect.GetType());
                EditorUtility.CopySerialized(sourceEffect, presetEffect);
                presetBeholdR.PostEffects.Add(presetEffect);
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(Utilities.PresetsPathRelative + presetName + ".prefab");
            PrefabUtility.CreatePrefab(path, preset);
            Object.DestroyImmediate(preset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            source.enabled = true;
        }

        public static void Load(BeholdR target, string relativePresetPath)
        {
            GameObject presetObject = Utilities.GetAsset<GameObject>(relativePresetPath);
            BeholdR presetBeholdr = presetObject.GetComponent<BeholdR>();
            if(presetBeholdr == null) {
                Debug.LogError("Selected asset is not a valid BeholdR preset!");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(target, "load preset");
            bool enabledState = target.enabled;
            target.enabled = false;            

            for (int i = 0; i < target.PostEffects.Count; i++) {
                Object.DestroyImmediate(target.PostEffects[i]);
            }

            target.PostEffects.Clear();

            for (int i = 0; i < presetBeholdr.PostEffects.Count; i++) {
                Component targetEffect = target.gameObject.AddComponent(presetBeholdr.PostEffects[i].GetType());
                EditorUtility.CopySerialized(presetBeholdr.PostEffects[i], targetEffect);
                target.PostEffects.Add(targetEffect);
            }

            target.enabled = enabledState;
        }
        #endregion
    }
}
