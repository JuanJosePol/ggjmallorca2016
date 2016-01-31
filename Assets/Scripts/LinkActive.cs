using UnityEngine;
using System.Collections;

public class LinkActive : MonoBehaviour {
	
	public GameObject source;
	public GameObject target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		source.active=target.active;
	}
}
