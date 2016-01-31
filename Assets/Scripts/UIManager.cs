using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	public Text notificationText;
	
	CanvasGroup group;
	
	public static UIManager instance;
	
	void Start () {
		instance=this;
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
