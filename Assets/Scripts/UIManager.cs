using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	CanvasGroup group;
	
	void Start () {
		group=FindObjectOfType<CanvasGroup>();
		group.alpha=0;
	}
	
	float alphaTimer=0;
	
	void Update () {
		alphaTimer+=Time.deltaTime;
		if (alphaTimer>5) {
			group.alpha+=Time.deltaTime*0.5f;
		}
	}
}
