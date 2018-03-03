using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newMovement : MonoBehaviour {
	public GameObject speedText;

	public float maxSpeed;
	public float speed;
	public Vector3 moveDirection;
	public float acceleration;
	public float jumpHeight;
	public float groundDrag;
	public float airDrag;

	bool grounded = false;
	Vector3 lastDir;
	Rigidbody rb;
	CapsuleCollider Capsule;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Capsule = GetComponent<CapsuleCollider> ();
	}

	// Update is called once per frame
	void Update () {
		speedText.GetComponent<Text> ().text = "Speed: " + Mathf.Round(speed * 200);

		var moving = true;
		float realtime = UnityEngine.Time.deltaTime / UnityEngine.Time.timeScale;

		if (Input.GetKey ("w")) {
			if (Input.GetKey ("a")) {
				moveDirection = (transform.forward - transform.right);
			} else if (Input.GetKey ("d")) {
				moveDirection = (transform.forward + transform.right);
			} else {
				moveDirection = transform.forward;
			}
		}
		else if (Input.GetKey ("s")) {
			if (Input.GetKey ("a")) {
				moveDirection = (-transform.forward - transform.right);
			} else if (Input.GetKey ("d")) {
				moveDirection = (-transform.forward + transform.right);
			} else {
				moveDirection = -transform.forward;
			}
		}
		else if (Input.GetKey ("a")) {
			moveDirection = -transform.right;
		}
		else if (Input.GetKey ("d")) {
			moveDirection = transform.right;
		}
		else {
			//rb.velocity = new Vector3 (rb.velocity.x / 2, rb.velocity.y, rb.velocity.z/2);
			moving = false;
		}

		speed = Mathf.Abs(transform.InverseTransformDirection (rb.velocity).x) + Mathf.Abs(transform.InverseTransformDirection (rb.velocity).z);

		//Am I on the ground?
		grounded = detectCollision(Vector3.down);
		print (grounded);

		//Reduce speed if at maxspeed and reduce air control
		if (moving) {
			if (grounded) {
				lastDir = moveDirection;
			}
			else{
				moveDirection = Vector3.Lerp (lastDir, moveDirection, 0.2f);
			}
			rb.AddForce (moveDirection * acceleration * realtime);
			if(speed > maxSpeed){
				rb.velocity = new Vector3(rb.velocity.normalized.x * maxSpeed, rb.velocity.y, rb.velocity.normalized.z);
			}
		}



		//adjust drag for ground/air
		if (grounded) {
			rb.drag = groundDrag;
		} else {
			rb.drag = airDrag;
		}

		//Jumping
		if (Input.GetKey ("space") && grounded == true) {
			rb.AddForce (transform.up * jumpHeight);
		}

		//change timescale
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll != 0){
			if(UnityEngine.Time.timeScale + scroll < 10 && UnityEngine.Time.timeScale + scroll > 0){
				UnityEngine.Time.timeScale += scroll;
				float timescale = UnityEngine.Time.timeScale;
				Debug.Log (timescale);
			}
		}
	}

	bool detectCollision(Vector3 dir){
		RaycastHit[] hits = Physics.CapsuleCastAll (transform.position + Capsule.center + (Vector3.one * (Capsule.height / 2 - Capsule.radius)), transform.position + Capsule.center - (Vector3.one * (Capsule.height / 2 - Capsule.radius)), Capsule.radius-.1f, dir, .4f);
		foreach (RaycastHit objectHit in hits) {
			if (objectHit.transform.tag == "WorldGeometry") {
				return true;
			}
		}
		return false;
	}
}
