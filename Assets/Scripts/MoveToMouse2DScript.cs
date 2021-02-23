using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse2DScript : MonoBehaviour {

	void Update() {

		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var tf = transform.position;
		tf.z = 0;
		transform.position = tf;

	}
}
