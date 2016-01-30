using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraPerspectiveEditor))]
public class ClassicFPSCameraController : MonoBehaviour
{
	public float sensitivity = 0.3f;
	public float min = -0.8f;
	public float max = 0.8f;
	public bool inverted = false;

	//-------------------------

	private CameraPerspectiveEditor cameraOffsetController;

	//-------------------------

	void Awake ()
	{
		cameraOffsetController = GetComponent<CameraPerspectiveEditor>();
	}

	//-------------------------

	void Update ()
	{
		if (inverted)
		{
			cameraOffsetController.lensShift.y = Mathf.Clamp(cameraOffsetController.lensShift.y - Input.GetAxis("Mouse Y") * sensitivity, min, max);
		}
		else
		{
			cameraOffsetController.lensShift.y = Mathf.Clamp(cameraOffsetController.lensShift.y + Input.GetAxis("Mouse Y") * sensitivity, min, max);
		}
	}
}
