using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour {

	public GameObject speedText;
	public float accel;
	public float decel;
	public float maxSpeed;

	bool grounded;
	Rigidbody rb;
	CapsuleCollider Collider;
	Vector3 moveGoal;

	float playerSpeed = 0; //Read-only speed var

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Collider =  GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//detect if on ground
		if (Physics.Raycast (transform.position, Vector3.down, (Collider.height / 2) )) {
			grounded = true;
			rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
		} else {
			grounded = false;
		}

		moveGoal = getMoveGoal();	//Get the direction I want to go in
		if (grounded) {
			if (moveGoal != Vector3.zero) {	//Avoid divide by 0
				moveGoal = (moveGoal / ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z)) / 2) * accel);	//Make sure the player doesn't go faster diagonally
				if (Mathf.Abs (playerSpeed) <= maxSpeed) {
					rb.velocity += moveGoal; //Accelerate player if they want to move
				} else {
					rb.velocity = new Vector3 (rb.velocity.x / 2, rb.velocity.y, rb.velocity.z / 2f); //Decelerate player, they're going to fast
				}
			} else {
				rb.velocity = new Vector3 (rb.velocity.x / decel, rb.velocity.y, rb.velocity.z / decel);//Decelerate player, they want to stop
			}
		}

		playerSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z); //calculate speed
		speedText.GetComponent<Text>().text = "Speed: " + Mathf.Round(playerSpeed); //display speed
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
