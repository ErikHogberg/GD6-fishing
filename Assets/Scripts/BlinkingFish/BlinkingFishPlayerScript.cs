using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingFishPlayerScript : MonoBehaviour {

	public SpriteRenderer Blinker;
	private Color initColor;

	[Serializable]
	public class KeyColorEntry {
		public KeyCode Key;
		public Color Color;
	}

	public List<KeyColorEntry> KeyColorEntries;
	private KeyCode lastPressedKey;
	private float pressTimer = 0;

	void Start() {
		initColor = Blinker.color;
	}

	void Update() {
		pressTimer += Time.deltaTime;
		foreach (var item in KeyColorEntries) {
			if (Input.GetKeyDown(item.Key)) {
				lastPressedKey = item.Key;
				// item.Color.a = initColor.a;
				Blinker.color = item.Color;
				// BlinkingFishEntityScript.BroadcastBlink(item.Color);
				pressTimer = 0;
			}
		}

		if (Input.GetKeyUp(lastPressedKey)) {
			BlinkingFishEntityScript.BroadcastBlink(Blinker.color, pressTimer);
			Blinker.color = initColor;
		}
	}
}
