using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MorganMovement : MonoBehaviour {
	public GameObject speedText;
	public float speed;
	public float moveSpeed = 5f;
	public float gravity = 5;
	public float maxspeed;
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
	
	// Update is called once per frame
	void FixedUpdate () {

		var localVelocity = transform.InverseTransformDirection (rb.velocity);
		speed = Mathf.Abs(transform.InverseTransformDirection (rb.velocity).x) + Mathf.Abs(transform.InverseTransformDirection (rb.velocity).z);
		speedText.GetComponent<Text> ().text = "Speed: " + transform.InverseTransformDirection (rb.velocity).z;

		RaycastHit[] hits = Physics.CapsuleCastAll (transform.position + Collider.center+ (Vector3.one * (Collider.height / 2 - Collider.radius)), transform.position + Collider.center - (Vector3.one * (Collider.height / 2 - Collider.radius)), Collider.radius-.1f, Vector3.down, .4f);
		foreach (RaycastHit objectHit in hits) {
			grounded = (objectHit.transform.tag == "WorldGeometry");
		}

		float realtime = UnityEngine.Time.deltaTime / UnityEngine.Time.timeScale;
		//movement
		if (Input.GetKey("w")) {
			//transform.Translate (Vector3.forward * moveSpeed * realtime);
			rb.AddForce(transform.forward * 50);
		}
		if (Input.GetKey("a")) {
			//transform.Translate (Vector3.left * moveSpeed * realtime);
			rb.AddForce(-(transform.right * 50));
		}
		if (Input.GetKey("s")) {
			//transform.Translate (Vector3.back * moveSpeed * realtime);
			rb.AddForce(-(transform.forward * 50));
		}
		if (Input.GetKey("d")) {
			//transform.Translate (Vector3.right * moveSpeed * realtime);
			rb.AddForce(transform.right * 50);
		}
		//top grounded speed fix

		if (localVelocity.x > maxspeed) {
			rb.AddForce(transform.right * (maxspeed-localVelocity.x));
		}
		if (localVelocity.z > maxspeed) {
			rb.AddForce(transform.forward * (maxspeed-localVelocity.z));
			Debug.Log (localVelocity.z);
		}
		rb.AddForce(0, -gravity, 0);
		//jump?
		if (Input.GetKey ("space") && grounded) {
			rb.AddForce(0, 250, 0);
		}



		/*
		var prevAngle = rb.rotation.eulerAngles.y;
		//air strafe
		if (rb.rotation.eulerAngles.y > prevAngle && !grounded && Input.GetKey ("d")) {
			rb.AddForce(transform.forward * 500);
		}
		if (rb.rotation.eulerAngles.y < prevAngle && !grounded && Input.GetKey ("a")) {
			rb.AddForce(transform.forward * 500);
		}
		*/

	}
}
