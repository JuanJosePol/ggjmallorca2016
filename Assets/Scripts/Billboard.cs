using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		transform.LookAt(transform.position+Camera.main.transform.forward, Camera.main.transform.up);
	}
}
