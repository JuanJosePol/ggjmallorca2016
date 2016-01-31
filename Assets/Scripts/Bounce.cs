using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	Vector3 startPosition;
	
	void Start () {
		startPosition=transform.localPosition;
	}
	
	void Update () {
		float phase=Mathf.Abs(Mathf.Sin(Time.time*5));
		transform.localPosition=startPosition+phase*Vector3.up*0.5f;
		transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
		transform.localScale=new Vector3(1.5f-phase*0.5f, 1f+phase, 1.5f-phase*0.5f);
	}
}
