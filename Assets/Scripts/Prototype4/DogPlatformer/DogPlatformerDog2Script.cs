using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerDog2Script : MonoBehaviour {

	public DogPlatformerHandScript Player;

	[HideInInspector]
	public Rigidbody2D DogRB;

	public bool Follow = true;
	public float FollowDistance;
	public float FollowSpeed;

	public float Trust = .5f;
	private bool distracted = false;

	void Start() {
		DogRB = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		if (!distracted
		&& Follow
		&& FollowDistance * FollowDistance < (transform.position - Player.transform.position).sqrMagnitude
		) {
			DogRB.AddForce((transform.position - Player.transform.position).normalized * -FollowSpeed, ForceMode2D.Force);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// TODO: lose trust when hit by snake
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (Trust < .3f && other.CompareTag("Squirrel")) {
			distracted = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Squirrel")) {
			distracted = false;
		}
	}
}
