using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour {

	public GameObject speedText;
	public float accel;
	public float decel;
	public float maxSpeed;
	public float jumpHeight;
	public float airaccel;

	bool grounded;
	bool bhopfix;
	Rigidbody rb;
	CapsuleCollider Collider;
	Vector3 moveGoal;

	float playerSpeed = 0; //Read-only speed var
	float lastGroundSpeed = 0;	//Read-only: how fast the player was on the ground only

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Collider =  GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		playerSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z); //calculate speed
		speedText.GetComponent<Text>().text = "Speed: " + Mathf.Round(playerSpeed); //display speed
		moveGoal = getMoveGoal();	//Get the direction I want to go in

		//detect if on ground
		// Old Method: if (Physics.Raycast (transform.position, -gameObject.transform.up, (Collider.height / 2) + 0.01f )) {
		if (Physics.CapsuleCast (new Vector3(transform.position.x,transform.position.y + (Collider.height/4),transform.position.z), new Vector3(transform.position.x,transform.position.y-(Collider.height/4),transform.position.z), Collider.radius - .1f, Vector3.down, .11f)) {
			grounded = true;
			Debug.Log("grounded");
		} else {
			grounded = false;
			Debug.Log("NOT grounded");
		}

		if (grounded && bhopfix) {	//I'm on the ground
			lastGroundSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z); //Get speed on ground
			if (moveGoal != Vector3.zero) {	//Do I want to move?
				if (playerSpeed < maxSpeed) {
					moveGoal = (moveGoal / ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z)) / 2) * accel);	//Make sure the player doesn't go faster diagonally
					rb.velocity += moveGoal; //Accelerate player if they want to move
				} else {
					rb.velocity = (moveGoal / ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z))) * (maxSpeed+1)); //Cap player speed
				}
			} else {
				rb.velocity = new Vector3 (rb.velocity.x / decel, rb.velocity.y, rb.velocity.z / decel);//Decelerate player, they want to stop
			}
		}
		else{ //Whee I'm flying
			float currentspeed =  Vector3.Dot(rb.velocity, moveGoal);
			float currentspeedNormalized =  Vector3.Dot(rb.velocity.normalized, moveGoal);
			if (currentspeedNormalized < 1.1 && currentspeedNormalized > .9) {
			}
			else{
				float addspeed = ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z))) - currentspeed;
				if (moveGoal != Vector3.zero) {
					rb.velocity += (moveGoal / ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z)))) * addspeed;
				}
			}
			//Test for Air movement, Enable if you want broken air movement
			//Vector3 moveGoalNormal = Vector3.Normalize(moveGoal);
			//rb.velocity = new Vector3(moveGoalNormal.x * lastGroundSpeed,rb.velocity.y, moveGoalNormal.z * lastGroundSpeed);
		}

		//Jump
		if (Input.GetKey ("space")) {
			if (grounded == true){
				rb.AddForce (transform.up * jumpHeight);
			}
			bhopfix = false;
		} else {
			bhopfix = true;
		}


	}

    Vector3 getMoveGoal()
    {
        // This code sucks:
        /*
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
        */

        Vector3 endDirection = new Vector3(0, 0, 0);
        if (Input.GetKey("w"))
        {
            endDirection += transform.forward;
        }
        else if (Input.GetKey("s"))
        {
            endDirection -= transform.forward;
        }

        if (Input.GetKey("a"))
        {
            endDirection -= transform.right;
        }
        else if (Input.GetKey("d"))
        {
            endDirection += transform.right;
        }
        endDirection.Normalize();
        return (endDirection);
    }
}
