using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraPerspectiveEditor))]
public class CPEScreenPointToRayDemonstration : MonoBehaviour
{
	private CameraPerspectiveEditor cameraPerspectiveEditor;
	private Camera thisCamera;
	private Ray screenPointRay;

	//-----------------------

	void Awake ()
	{
		cameraPerspectiveEditor = GetComponent<CameraPerspectiveEditor>();
		thisCamera = GetComponent<Camera>();
	}

	//-----------------------

	void Update ()
	{
		screenPointRay = cameraPerspectiveEditor.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(screenPointRay.origin, screenPointRay.direction * (thisCamera.farClipPlane - thisCamera.nearClipPlane));
	}
}
