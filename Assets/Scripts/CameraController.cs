using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	private float mousePosX;
	private float mousePosY;
	public int scrollDistance = 2;
	public float scrollSpeed = 105f;
	public float maxFOV = 120f;					// maximum field of view
	public float minFOV = 10f;					// minimum field of view
	public float zoomOffset = 0.25f;
	private float startFOV;						// start field of view
	
	public void Start () 
	{
		startFOV = Mathf.Min (Camera.main.fieldOfView, maxFOV);
	}

	void Update () 
	{
		Debug.Log (Camera.main.transform.position);
		// track mouse movement in x and y directions (screen space)
		mousePosX = Input.mousePosition.x;
		mousePosY = Input.mousePosition.y;

		// camera horizontal / vertical movement
		// left 
		if (mousePosX < scrollDistance)
		{ 
			transform.Translate(-1, 0, 0);
		}

		// right  
		else if (mousePosX >= Screen.width - scrollDistance)
		{ 
			transform.Translate(1, 0, 0);
		} 
		
		// down  
		else if (mousePosY < scrollDistance)
		{
			transform.position -= Vector3.forward;
		} 

		// up  
		else if (mousePosY >= Screen.height - scrollDistance)
		{
			transform.position += Vector3.forward;
		}

		// we zoom in direction of cameras z axis
		// zoom in
		if ((Input.GetAxis("Mouse ScrollWheel") > 0) || (Input.GetKey ("a")) && Camera.main.fieldOfView > minFOV)
		{
			Camera.main.fieldOfView -= zoomOffset;
		}
		
		// zoom out
		if ((Input.GetAxis("Mouse ScrollWheel") < 0) || (Input.GetKey ("d")) && Camera.main.fieldOfView < maxFOV)
		{
			Camera.main.fieldOfView += zoomOffset;
		}
		
		// reset zoom
		if (Input.GetKeyDown(KeyCode.Mouse2))
		{
			Camera.main.orthographicSize = startFOV;
		}

		if ((Input.GetMouseButtonDown(1) && Input.GetMouseButtonDown(2)) || Input.GetKey ("q"))
		{
			// rotation around Y axis
			// hold middle and right button to rotate clockwise
			transform.Rotate(Vector3.up * Time.deltaTime * -scrollSpeed, Space.World);
		}
		else if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(2)) || Input.GetKey ("e"))
		{
			// rotation around Y axis
			// hold middle and left button to rotate counter clockwise
			transform.Rotate(Vector3.down * Time.deltaTime * -scrollSpeed, Space.World);
		}

		/*
		else if ((Input.GetMouseButtonDown(4)) || Input.GetKey ("w"))
		{
			// rotation around X axis
			transform.Rotate(Vector3.right * Time.deltaTime * -scrollSpeed, Space.Self);
		}
		else if ((Input.GetMouseButtonDown(5)) || Input.GetKey ("s"))
		{
			// rotation around X axis
			transform.Rotate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
		}
		*/
		
	}
}