using UnityEngine;
using UnityEditor;

public class VoxelAssetsImporter : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
    {
        foreach (var asset in imported)
        {
            if (asset.Contains("VoxelModels/"))
            {
                if (asset.Contains(".mtl") || asset.Contains(".png"))
                {
                    AssetDatabase.DeleteAsset(asset);
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}