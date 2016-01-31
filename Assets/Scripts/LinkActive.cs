using UnityEngine;
using System.Collections;

public class LinkActive : MonoBehaviour {
	
	public GameObject source;
	public GameObject target;
	
	void Update () {
		source.SetActive(target.activeSelf);
	}
}
