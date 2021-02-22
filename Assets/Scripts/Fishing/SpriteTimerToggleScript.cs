using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTimerToggleScript : MonoBehaviour {

	public float TimePerFrame;
	private float timer = 0;
	private int currentSprite = 0;

	public List<GameObject> SpritesParents;
	private List<List<SpriteRenderer>> sprites;

	private void Start() {

		sprites = new List<List<SpriteRenderer>>();
		foreach (var item in SpritesParents) {
			var list = new List<SpriteRenderer>();
			foreach (var sprite in item.GetComponentsInChildren<SpriteRenderer>()) {
				list.Add(sprite);
			}
			sprites.Add(list);
		}

		foreach (var sprite in sprites.SelectMany(s => s)) {
			PlayerControllerScript.SetAlpha(sprite, .1f);
		}

		currentSprite = 0;
		if (sprites.Count > 0) {
			foreach (var sprite in sprites[0]) {
				PlayerControllerScript.SetAlpha(sprite, 1f);
			}
		}
	}

	void Update() {
		timer += Time.deltaTime;
		while (timer > TimePerFrame) {
			timer -= TimePerFrame;

			foreach (var sprite in sprites[currentSprite]) {
				PlayerControllerScript.SetAlpha(sprite, .1f);
			}
			currentSprite++;
			if (currentSprite >= sprites.Count)
				currentSprite = 0;

			foreach (var sprite in sprites[currentSprite]) {
				PlayerControllerScript.SetAlpha(sprite, 1f);
			}
		}
	}
}
