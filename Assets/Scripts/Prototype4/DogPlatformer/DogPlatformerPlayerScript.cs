using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerPlayerScript : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D PlayerRB;
	DistanceJoint2D leash;

	public DogPlatformerDogScript Dog;

	float leashInitDistance;

	public float JumpForce;
	public float MoveSpeed;

	public float FallVelocityThreshold;

	void Start() {
		PlayerRB = GetComponent<Rigidbody2D>();
		leash = GetComponent<DistanceJoint2D>();
		leashInitDistance = leash.distance;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			leash.distance = (transform.position - Dog.transform.position).magnitude;
		} else if (Input.GetKeyUp(KeyCode.F)) {
			leash.distance = leashInitDistance;
		}

		if (Input.GetKeyDown(KeyCode.I)) {
			// TODO: tell dog to go up
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			// TODO: tell dog to go left
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			// TODO: tell dog to go down
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			// TODO: tell dog to go right
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			// TODO: tell dog to go back to you
		}

		if (Input.GetKeyDown(KeyCode.G)) {
			// TODO: give treat to dog if close enough
		}

	}

	private void OnCollisionStay2D(Collision2D other) {

		if (Input.GetKey(KeyCode.W)) {
			PlayerRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
			if (PlayerRB.velocity.y > JumpForce) {
				Vector2 newVelocity = PlayerRB.velocity;
				newVelocity.y = JumpForce;
				PlayerRB.velocity = newVelocity;
			}
		}

		if (Input.GetKey(KeyCode.A)) {
			PlayerRB.AddForce(Vector2.left * MoveSpeed, ForceMode2D.Force);
		}
		if (Input.GetKey(KeyCode.D)) {
			PlayerRB.AddForce(Vector2.right * MoveSpeed, ForceMode2D.Force);
		}

	}
}
