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
	
	public Texture2D[] gameCovers;
	public Sprite[] dialogs;
	public AudioClip[] sounds;
	
	void Awake() {
		instance=this;
		bodies	= Resources.LoadAll<Mesh>("VoxelModels/Bodies");
		eyes	= Resources.LoadAll<Mesh>("VoxelModels/Eyes");
		hairs	= Resources.LoadAll<Mesh>("VoxelModels/Hairs");
		hats	= Resources.LoadAll<Mesh>("VoxelModels/Hats");
		heads	= Resources.LoadAll<Mesh>("VoxelModels/Heads");
		gameCovers=Resources.LoadAll<Texture2D>("GameCovers");
		dialogs=Resources.LoadAll<Sprite>("Dialogs");
		sounds=Resources.LoadAll<AudioClip>("Sounds");
	}
	
	public void PlaySound(string clipName) {
		AudioClip selectedClip=null;
		foreach (AudioClip clip in sounds) {
			if (clip.name==clipName) {
				selectedClip=clip;
			}
		}
		if (selectedClip!=null) {
			AudioSource.PlayClipAtPoint(selectedClip, Camera.main.transform.position);
		} else {
		}
	}
	
}

public static class CatalogExtensions {
	
	public static T GetRandom<T>(this T[] array) {
		return array[Random.Range(0, array.Length)];
	}
	
	public static T GetByName<T>(this T[] array, string name) where T : UnityEngine.Object
    {
		for (int i = 0; i < array.Length; i++) {
			if (array[i].name.Equals(name)) {
				return array[i];
			}
		}
		return default(T);
	}
	
}
