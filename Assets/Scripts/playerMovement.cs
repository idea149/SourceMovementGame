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
    bool bhopCheck;
    bool bhopfix;
	Rigidbody rb;
	CapsuleCollider Collider;
	Vector3 moveGoal;
    Vector3 moveGoalMod;

    float playerSpeed = 0; //Read-only speed var
	float lastGroundSpeed = 0;	//Read-only: how fast the player was on the ground only

    bool boxCastHit;
    RaycastHit hit;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Collider =  GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        playerSpeed = rb.velocity.magnitude;//Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z); //calculate speed

        speedText.GetComponent<Text>().text = "Speed: " + Mathf.Round(playerSpeed); //display speed
		moveGoal = getMoveGoal();   //Get the direction I want to go in

        //detect if on ground
        // Old Method: if (Physics.Raycast (transform.position, -gameObject.transform.up, (Collider.height / 2) + 0.01f )) {
        //Physics.CapsuleCast(new Vector3(transform.position.x, transform.position.y + (Collider.height / 4), transform.position.z), new Vector3(transform.position.x, transform.position.y - (Collider.height / 4), transform.position.z), Collider.radius - .1f, Vector3.down, .11f);
        //boxCastHit = Physics.BoxCast(transform.position + new Vector3(0,1,0), new Vector3(Collider.radius - 0.1f, Collider.height / 2, Collider.radius - 0.1f), Vector3.down, out hit, transform.rotation, (Collider.height/2) + 0.01f);

        boxCastHit = Physics.SphereCast(transform.position, Collider.radius, Vector3.down, out hit, (Collider.height / 4) + 0.01f);
        if (boxCastHit && hit.normal != Vector3.right && hit.normal != Vector3.left && hit.normal != Vector3.forward && hit.normal != Vector3.back) {
			grounded = true;
		} else {
			grounded = false;
        }

		if (grounded && bhopCheck) {	//I'm on the ground
			lastGroundSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z); //Get speed on ground
			if (moveGoal != Vector3.zero) {	//Do I want to move?
				if (playerSpeed <= maxSpeed) {
					moveGoal = (moveGoal / ((Mathf.Abs (moveGoal.x) + Mathf.Abs (moveGoal.z)) / 2) * accel);	//Make sure the player doesn't go faster diagonally
					rb.velocity += moveGoal; //Accelerate player if they want to move
				} else {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                    //rb.velocity = rb.velocity.normalized * maxSpeed; //cap player speed
                }
			} else {
                rb.velocity = new Vector3 (rb.velocity.x / decel, rb.velocity.y, rb.velocity.z / decel);//Decelerate player, they want to stop
			}
		}
		else{ //Whee I'm flying
            bhopfix = true;
            airMovement();
        }

		//Jump
		if (Input.GetKey ("space")) { 
			if (grounded && bhopfix)
            {
                Debug.Log("JUMP");
                rb.AddForce (transform.up * jumpHeight);
                bhopfix = false;
            }
            bhopCheck = false;
		} else {
            bhopCheck = true;
		}


	}

    Vector3 getMoveGoal()
    {

        Vector3 endDirection = Vector3.zero;
        if (Input.GetKey("w"))
        {
            endDirection += transform.forward;
        }
        if (Input.GetKey("s"))
        {
            endDirection -= transform.forward;
        }
        if (Input.GetKey("a"))
        {
            moveGoalMod = -transform.right;
            endDirection -= transform.right;
        }
        if (Input.GetKey("d"))
        {
            moveGoalMod = transform.right;
            endDirection += transform.right;
        }
        moveGoalMod.Normalize();

        endDirection.Normalize();
        return (endDirection);
    }

    void airMovement()
    {
        if(moveGoal == Vector3.zero)
        {
            return; //if the player doesn't want to move, don't move
        }

        Vector3 velCut = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        float currentspeed = Vector3.Dot(velCut, moveGoalMod);
        float currentspeedNormalized = Vector3.Dot(velCut.normalized, moveGoalMod);

        float rawCurrentspeedNormalized = Vector3.Dot(velCut.normalized, moveGoal);


        if (Mathf.Abs(currentspeedNormalized) < 1.1 && Mathf.Abs(currentspeedNormalized) > .9)
        {
            //rb.velocity = rb.velocity;
        }
        else
        {
            float addspeed = ((Mathf.Abs(moveGoalMod.x) + Mathf.Abs(moveGoalMod.z))) - currentspeed;
            if (moveGoalMod != Vector3.zero)
            {
                rb.velocity += (moveGoalMod / ((Mathf.Abs(moveGoalMod.x) + Mathf.Abs(moveGoalMod.z)))) * addspeed;
            }
        }

        if (Mathf.Abs(rawCurrentspeedNormalized) < .8 && Mathf.Abs(rawCurrentspeedNormalized) > .6)
        {
            rb.velocity += transform.forward * .1f;
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, Collider.radius);

        Gizmos.color = Color.red;
        //Check if there has been a hit yet
        if (boxCastHit)
        {
            Gizmos.DrawWireSphere(hit.point, Collider.radius);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector3.down * ((Collider.height / 4) + 0.01f));
            //Draw a cube at the maximum distance
            Gizmos.DrawWireSphere(transform.position + Vector3.down * ((Collider.height / 4) + 0.01f), Collider.radius);
        }
    }
}
