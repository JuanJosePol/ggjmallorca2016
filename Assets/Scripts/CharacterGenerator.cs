using UnityEngine;
using System.Collections;

public class CharacterGenerator : MonoBehaviour {
	
	public GameObject headParent;
	public GameObject bodyParent;
	
	public MeshFilter faceBase;
	public MeshFilter hatBase;
	public MeshFilter hairBase;
	public MeshFilter eyesBase;
	public MeshFilter bodyBase;
	
	public Texture2D skinPalette;
	public Texture2D hairPalette;
	
	void Start () {
		GenerateChar();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.G)) {
			GenerateChar();
		}
	}
	
	void GenerateChar() {
		faceBase.GetComponent<MeshRenderer>().material.color=GetRandomColorFromPalette(skinPalette);
		hairBase.GetComponent<MeshRenderer>().material.color=GetRandomColorFromPalette(hairPalette);
		
		hairBase.mesh=AssetCatalog.instance.hairs.GetRandom();
		bodyBase.mesh=AssetCatalog.instance.bodies.GetRandom();
		hatBase.mesh=AssetCatalog.instance.hats.GetRandom();
	}
	
	Color GetRandomColorFromPalette(Texture2D palette) {
		int selected=Random.Range(0, palette.width-1);
		return palette.GetPixel(selected, 0);
	}
}
