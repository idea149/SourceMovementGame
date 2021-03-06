﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MorganMovement : MonoBehaviour {
	//Public stuff
	public GameObject speedText;
	public float speed;
	public float groundSpeed;
	public float airSpeed;
	public float jumpHeight;
	public float maxspeed;
	//shhh this is private
	float moveSpeed;
	float prevAngle;
	Rigidbody rb;
	CapsuleCollider Collider;
	bool grounded = false;
	int wallRun = 0; //0 is no wallrun, 1 is wallrun on left, 2 is wallrun on right

	Vector3 localVelocity;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Collider = GetComponent<CapsuleCollider> ();
		localVelocity = transform.InverseTransformDirection (rb.velocity);
	}

	//updates every frame
	void Update(){
	}

	//updates at some points in time probably
	void FixedUpdate () {
		var localVelocity = transform.InverseTransformDirection (rb.velocity);
		speed = Mathf.Abs(transform.InverseTransformDirection (rb.velocity).x) + Mathf.Abs(transform.InverseTransformDirection (rb.velocity).z);
		speedText.GetComponent<Text> ().text = "Speed: " + transform.InverseTransformDirection (rb.velocity).z;

		Debug.Log (wallRun);
		//Detect if I'm on the ground or not
		//Don't use the commented stuff it's a dumb capsule collider that doesn't work well
		/*RaycastHit[] hits = Physics.CapsuleCastAll (transform.position + Collider.center+ (Vector3.one * (Collider.height / 2 - Collider.radius)), transform.position + Collider.center - (Vector3.one * (Collider.height / 2 - Collider.radius)), Collider.radius-.1f, Vector3.down, .4f);
		foreach (RaycastHit objectHit in hits) {
			grounded = (objectHit.transform.tag == "WorldGeometry");
		}*/
		//detect if grounded
		if (Physics.Raycast (transform.position, Vector3.down, (Collider.height / 2) + Collider.radius)) {
			grounded = true;
			rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
		} else {
			grounded = false;
		}
		//movement
		if (grounded) {
			moveSpeed = groundSpeed;
		}
		else {
			moveSpeed = airSpeed;
		}
		if (grounded) {
			if (Input.GetKey ("w")) {
				//transform.Translate (Vector3.forward * moveSpeed * realtime);
				rb.AddForce (transform.forward * moveSpeed);
			}
			if (Input.GetKey ("a")) {
				//transform.Translate (Vector3.left * moveSpeed * realtime);
				rb.AddForce (-(transform.right * moveSpeed));
			}
			if (Input.GetKey ("s")) {
				//transform.Translate (Vector3.back * moveSpeed * realtime);
				rb.AddForce (-(transform.forward * moveSpeed));
			}
			if (Input.GetKey ("d")) {
				//transform.Translate (Vector3.right * moveSpeed * realtime);
				rb.AddForce (transform.right * moveSpeed);
			}

		}
		else {
			//air strafe
			if (rb.rotation.eulerAngles.y > prevAngle && Input.GetKey ("d")) {
				rb.AddForce (transform.forward * (groundSpeed));
			}
			if (rb.rotation.eulerAngles.y < prevAngle && Input.GetKey ("a")) {
				rb.AddForce (transform.forward * (groundSpeed));
			}

			//detect wallrunning
			if (Physics.Raycast (transform.position, Vector3.left, Collider.radius + .5f)) {
				wallRun = 1;
				rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			} 
			else if (Physics.Raycast (transform.position, Vector3.right, Collider.radius + 0.5f)) {
				wallRun = 2;
				rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			} 
			else {
				wallRun = 0;
			}
		}
		prevAngle = rb.rotation.eulerAngles.y;
		//top grounded speed fix
		if (grounded) {
			rb.velocity = new Vector3(Mathf.Round (rb.velocity.x * 100)/100, Mathf.Round (rb.velocity.y * 100)/100, Mathf.Round (rb.velocity.z * 100)/100);
			//woah slow down
			var totalSpeed = Mathf.Abs(rb.velocity.x + rb.velocity.y);
			rb.AddForce (-(rb.velocity.normalized) * (groundSpeed/3));
			if (localVelocity.x > maxspeed || localVelocity.z > maxspeed) {
				rb.velocity = rb.velocity.normalized * (maxspeed-1);
				//rb.AddForce (transform.right * (maxspeed - localVelocity.x));
			}
			if (localVelocity.z < -maxspeed || localVelocity.x < -maxspeed) {
				rb.velocity = rb.velocity.normalized * (maxspeed-1);
				Debug.Log ("aahaefh");
				//rb.AddForce (transform.forward * (maxspeed - localVelocity.z));
			}
		}

		//jump
		if (Input.GetKey ("space") && grounded) {
			rb.AddForce(0, jumpHeight, 0);
		}
	}
}
