using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour {
	public float speed = 10.0f;
	public Camera c;
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		//set Collider/body rotation
		float MouseX = Input.GetAxis ("Mouse X") * speed;
		float MouseY = Input.GetAxis ("Mouse Y") * speed;
		//translation *= UnityEngine.Time.deltaTime;
		//straffe *= UnityEngine.Time.deltaTime;
		transform.Rotate (0, MouseX, 0);
		Vector3 zValue = transform.eulerAngles;
		zValue.z = 0;
		transform.eulerAngles = zValue;

		//set camera rotation
		Quaternion xValue = c.transform.localRotation;
		if (xValue.x > .7) {
			if (MouseY < 0) {
				MouseY = 0;
			}
			xValue.x = .7f;
			c.transform.localRotation = xValue;
		
		} else if (xValue.x < -.7) {
			if (MouseY > 0) {
				MouseY = 0;
			}
			xValue.x = -.7f;
			c.transform.localRotation = xValue;
		}
		c.transform.Rotate (-MouseY, 0, 0);

		//Disable mouselock on escape
		if (Input.GetKeyDown ("escape")) {
			Cursor.lockState = CursorLockMode.None;
		}


	}

}
