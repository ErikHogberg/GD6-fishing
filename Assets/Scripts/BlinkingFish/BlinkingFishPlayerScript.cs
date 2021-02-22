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

	void Start() {
		initColor = Blinker.color;
	}

	void Update() {
		foreach (var item in KeyColorEntries) {
			if (Input.GetKeyDown(item.Key)) {
				lastPressedKey = item.Key;
				Blinker.color = item.Color;
				BlinkingFishEntityScript.BroadcastBlink(item.Color);
			}
		}

		if (Input.GetKeyUp(lastPressedKey)) {
            Blinker.color = initColor;
		}
	}
}
