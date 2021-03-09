using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlayerScript : MonoBehaviour {

    public float MoveSpeed = 1f;
    public float ReverseSpeed = 1f;
    public float TurnSpeed = 1f;
    public float JumpForce = 1f;

	private Rigidbody dogRB;

	private Vector3 resetPos;
	private Quaternion resetRot;

	bool touchingGround = false;
    
    void Start() {
        dogRB = GetComponent<Rigidbody>();
		UpdateReset();
    }

    void FixedUpdate() {
        
        if (Input.GetKey(KeyCode.R)) {
			Reset();
		}

        if (Input.GetKey(KeyCode.A)) {
			dogRB.AddTorque(transform.up * -TurnSpeed * Time.deltaTime, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.D)) {
			dogRB.AddTorque(transform.up * TurnSpeed * Time.deltaTime, ForceMode.Acceleration);
        }        

		if(!touchingGround) return;

        if (Input.GetKey(KeyCode.W)) {
			dogRB.AddForce(transform.forward * MoveSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.S)) {
			dogRB.AddForce(transform.forward * -ReverseSpeed * Time.deltaTime, ForceMode.Acceleration);
        }

		if (Input.GetKey(KeyCode.Space)) {
			dogRB.AddForce(transform.up * JumpForce, ForceMode.Force);
		}

    }

	private void OnCollisionStay(Collision other) {
		touchingGround = true;
	}

	private void OnCollisionExit(Collision other) {
		touchingGround = false;
	}

	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Checkpoint")) {
			UpdateReset();
			other.gameObject.SetActive(false);
		}
	}

	void UpdateReset(){
		resetPos = transform.position;
		resetRot = transform.rotation;
	}

	void Reset(){
		transform.position = resetPos;
		transform.rotation = resetRot;
	}
}
