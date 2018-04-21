using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float moveSpeed = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float realtime = UnityEngine.Time.deltaTime / UnityEngine.Time.timeScale;
		//movement
		if (Input.GetKey("w")) {
			transform.Translate (Vector3.forward * moveSpeed * realtime);
		}
		if (Input.GetKey("a")) {
				transform.Translate (Vector3.left * moveSpeed * realtime);
		}
		if (Input.GetKey("s")) {
			transform.Translate (Vector3.back * moveSpeed * realtime);
		}
		if (Input.GetKey("d")) {
			transform.Translate (Vector3.right * moveSpeed * realtime);
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
}
