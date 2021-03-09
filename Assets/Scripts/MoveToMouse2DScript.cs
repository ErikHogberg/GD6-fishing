using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse2DScript : MonoBehaviour {

	public KeyCode ToggleKey;

	public bool Active = true;

	void Update() {

		if (Input.GetKeyDown(ToggleKey)) {
			Active = !Active;
		}

		if (!Active) return;

		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var tf = transform.position;
		tf.z = 0;
		transform.position = tf;

	}
}
