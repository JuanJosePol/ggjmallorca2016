using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MainCameraController : MonoBehaviour {
	
	public Transform target;
	
	void Start () {
		
	}
	
	void Update () {
		if (target!=null) {
			transform.LookAt(target);
		}
	}
}
