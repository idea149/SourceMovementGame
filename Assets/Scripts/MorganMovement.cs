using System.Collections;
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


		//Detect if I'm on the ground or not
		//Don't use the commented stuff it's a dumb capsule collider that doesn't work well
		/*RaycastHit[] hits = Physics.CapsuleCastAll (transform.position + Collider.center+ (Vector3.one * (Collider.height / 2 - Collider.radius)), transform.position + Collider.center - (Vector3.one * (Collider.height / 2 - Collider.radius)), Collider.radius-.1f, Vector3.down, .4f);
		foreach (RaycastHit objectHit in hits) {
			grounded = (objectHit.transform.tag == "WorldGeometry");
		}*/

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

		//top grounded speed fix
		if (grounded) {
			float totalSpeed = rb.velocity.x + rb.velocity.z;
			//woah slow down
			if (localVelocity.x > maxspeed || localVelocity.x < maxspeed) {
				rb.AddForce (transform.right * (maxspeed - localVelocity.x));
			}
			if (localVelocity.z > maxspeed || localVelocity.z < maxspeed) {
				rb.AddForce (transform.forward * (maxspeed - localVelocity.z));
			}
		}

		//jump
		if (Input.GetKey ("space") && grounded) {
			rb.AddForce(0, jumpHeight, 0);
		}





		//air strafe
		if (rb.rotation.eulerAngles.y > prevAngle && !grounded && Input.GetKey ("d")) {
			rb.AddForce(transform.forward * (groundSpeed * 2));
		}
		if (rb.rotation.eulerAngles.y < prevAngle && !grounded && Input.GetKey ("a")) {
			rb.AddForce(transform.forward * (groundSpeed * 2));
		}

		prevAngle = rb.rotation.eulerAngles.y;

	}
}
