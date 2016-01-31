using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainTitle : MonoBehaviour {
	
	public GameObject creditsCanvas;
	
	Vector3 startPosition;
	
	void Awake () {
		startPosition=transform.position;
		transform.position+=Vector3.up*35;
		transform.DOMove(startPosition, 3, false).SetEase(Ease.OutBounce).OnComplete(ShowTitle);
	}
	
	void ShowTitle() {
		Invoke("LeaveScreen", 3);
	}
	
	void LeaveScreen() {
		transform.DOMove(startPosition+Vector3.up*35, 1, false).SetEase(Ease.InExpo).OnComplete(DisplayGUI);
	}
	
	void DisplayGUI() {
		fadeOut=true;
	}
	
	bool fadeOut=false;
	
	void Update() {
		if (fadeOut) {
			creditsCanvas.transform.DOScaleY(0, 0.1f);
		}
	}
}
