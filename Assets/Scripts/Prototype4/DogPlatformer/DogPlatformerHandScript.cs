using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerHandScript : MonoBehaviour {

	public DogPlatformerDog2Script Dog;

	public float MinCommandDistance;

	void Start() {

	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2 delta = transform.position - Dog.transform.position;
			float sqrDistance = delta.sqrMagnitude;
			if (sqrDistance > MinCommandDistance * MinCommandDistance) {
				float angle= Vector2.SignedAngle(Vector2.up, delta);
				Dog.SendCommand(angle, sqrDistance);
			}
		}
	}
}
