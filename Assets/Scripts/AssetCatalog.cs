using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetCatalog : MonoBehaviour {
	
	public static AssetCatalog instance;
	
	public Mesh[] bodies;
	public Mesh[] eyes;
	public Mesh[] hairs;
	public Mesh[] hats;
	public Mesh[] heads;
	
	void Awake() {
		instance=this;
		bodies	= Resources.LoadAll<Mesh>("VoxelModels/Bodies");
		eyes	= Resources.LoadAll<Mesh>("VoxelModels/Eyes");
		hairs	= Resources.LoadAll<Mesh>("VoxelModels/Hairs");
		hats	= Resources.LoadAll<Mesh>("VoxelModels/Hats");
		heads	= Resources.LoadAll<Mesh>("VoxelModels/Heads");
	}
	
	void Update () {
	
	}
}

public static class CatalogExtensions {
	public static T GetRandom<T>(this T[] array) {
		return array[Random.Range(0, array.Length)];
	}
}