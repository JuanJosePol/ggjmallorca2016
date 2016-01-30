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
	        
	        if (asset.Contains(".png") && asset.Contains("Resources/UI"))
	        {
		        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(asset);
		        importer.npotScale=TextureImporterNPOTScale.None;
		        //importer.filterMode=FilterMode.Point;
		        importer.textureFormat=TextureImporterFormat.RGBA32;
		        importer.textureType=TextureImporterType.Advanced;
		        importer.spriteImportMode=SpriteImportMode.Single;
		        importer.isReadable=true;
		        importer.mipmapEnabled=false;
		        //importer.spritePixelsPerUnit=100;
		        importer.spriteBorder=new Vector4(8,8,8,8);
	        }
        }
    }
	
}