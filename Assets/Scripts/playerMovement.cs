using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

	public GameObject speedText;
	public float accel;
	public float speed;


	Rigidbody rb;
	CapsuleCollider Collider;
	Vector3 moveGoal;

	float acceltime = 0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Collider =  GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		moveGoal = getMoveGoal();	//Get the direction I want to go in
		if (moveGoal != Vector3.zero) {	//Avoid divide by 0
			moveGoal = (moveGoal / ((Mathf.Abs(moveGoal.x) + Mathf.Abs(moveGoal.z)) / 2) *speed);	//Make sure the player doesn't go faster diagonally
		}

		//rb.velocity = moveGoal * speed;
		rb.velocity = new Vector3 (moveGoal.x, rb.velocity.y, moveGoal.z); //Set velocity. Made this way so that Y velocity is always physics-based

		Debug.Log (rb.velocity);
		//delet this
		if (rb.velocity == moveGoal) {
			acceltime = 0;
		} else {
			acceltime += .1f;
		}

	}

	Vector3 getMoveGoal(){
		if (Input.GetKey ("w")) {
			if (Input.GetKey ("a")) {
				return(transform.forward - transform.right);
			} else if (Input.GetKey ("d")) {
				return(transform.forward + transform.right);
			} else {
				return(transform.forward);
			}
		} else if (Input.GetKey ("s")) {
			if (Input.GetKey ("a")) {
				return(-transform.forward - transform.right);
			} else if (Input.GetKey ("d")) {
				return(-transform.forward + transform.right);
			} else {
				return(-transform.forward);
			}
		} else if (Input.GetKey ("a")) {
			return(-transform.right);
		} else if (Input.GetKey ("d")) {
			return(transform.right);
		} else {
			return(Vector3.zero);
		}
	}
}
