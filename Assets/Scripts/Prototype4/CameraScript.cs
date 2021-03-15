using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float MoveSpeed = 10;

	public Vector2 Min;
	public Vector2 Max;

	void Start() {

	}

	void Update() {

	}

	private void OnTriggerStay2D(Collider2D other) {
		Vector3 dir = (transform.position - other.transform.position).normalized;
		dir.z = 0;

		Vector3 pos = transform.position;
		pos += -dir * MoveSpeed;

		if (pos.x < Min.x) pos.x = Min.x;
		else if (pos.x > Max.x) pos.x = Max.x;
		if (pos.y < Min.y) pos.y = Min.y;
		else if (pos.y > Max.y) pos.y = Max.y;

		transform.position = pos;

	}
}
