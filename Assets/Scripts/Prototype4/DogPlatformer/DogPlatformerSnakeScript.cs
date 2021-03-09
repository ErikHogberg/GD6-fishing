using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerSnakeScript : MonoBehaviour {

	Vector2 initPos;
	public Vector2 MoveDistance;
	public AnimationCurve MoveCurve;

	[Min(0.01f)]
	public float MoveTime;
	public float WaitTime;

	bool direction = true;
	bool wait = true;
	float timer = -1;

	void Start() {
		initPos = transform.position;
	}

	void Update() {

		timer -= Time.deltaTime;
		if (timer < 0) {
			if (wait) {
				timer += MoveTime;
				wait = false;
			} else {
				if (direction) {
					wait = true;
					timer += WaitTime;
				} else {
					timer += MoveTime;
				}
				direction = !direction;
			}

		}

		float t = MoveCurve.Evaluate(timer / MoveTime);
		if (!direction)
			t = 1 - t;

		if (wait) t = 0;

		transform.position = initPos + MoveDistance * t;

	}
}
