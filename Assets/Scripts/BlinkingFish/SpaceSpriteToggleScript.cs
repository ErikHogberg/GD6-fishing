using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpriteToggleScript : MonoBehaviour {
	public SpriteRenderer Sprite;

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Sprite.enabled = !Sprite.enabled;
		}
	}
}
