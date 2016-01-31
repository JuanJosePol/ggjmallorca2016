using UnityEngine;
using System.Collections;

public class StaffRenderer : MonoBehaviour {
	
	public RenderTexture cameraTexture;
	
	void Start () {
		CreateRenderTexture();
	}
	
	void Update () {
	
	}
	
	void CreateRenderTexture() {
		cameraTexture=new RenderTexture(64,64,16);
		GetComponent<Camera>().targetTexture=cameraTexture;
	}
}
