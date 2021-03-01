using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingFishPlayerScript : MonoBehaviour {

	public interface BlinkListener {
		void ReceiveBlinkStart(Color color);
		bool ReceiveBlinkEnd(Color color, float time);
	}

	public static List<BlinkListener> BlinkListeners = new List<BlinkListener>();

	public SpriteRenderer Blinker;
	public SpriteMask Mask;
	private Color initColor;

	[Serializable]
	public class KeyColorEntry {
		public KeyCode Key;
		public Color Color;
		public String SortingLayer;
	}

	public List<KeyColorEntry> KeyColorEntries;
	private KeyCode lastPressedKey;
	public Color CurrentColor => Blinker.color;

	public bool OverrideAlpha = false;
	[Range(0, 1)]
	public float Alpha = 1;

	private float pressTimer = 0;

	void Start() {
		initColor = Blinker.color;
	}

	void Update() {
		pressTimer += Time.deltaTime;
		foreach (var item in KeyColorEntries) {
			if (Input.GetKeyDown(item.Key)) {
				Mask.frontSortingLayerID = SortingLayer.NameToID(item.SortingLayer);
				lastPressedKey = item.Key;
				if (OverrideAlpha) {
					Color tempColor = item.Color;
					tempColor.a = Alpha;
					Blinker.color = tempColor;
				} else {
					Blinker.color = item.Color;
				}
				// BlinkingFishEntityScript.BroadcastBlink(item.Color);
				foreach (var listener in BlinkListeners) {
					listener.ReceiveBlinkStart(Blinker.color);
				}
				pressTimer = 0;
			}
		}

		if (Input.GetKeyUp(lastPressedKey)) {
			// BlinkingFishEntityScript.BroadcastBlink(Blinker.color, pressTimer);
			foreach (var item in BlinkListeners) {
				item.ReceiveBlinkEnd(Blinker.color, pressTimer);
			}
			Blinker.color = initColor;
			Mask.frontSortingLayerID = SortingLayer.NameToID("Default");
		}
	}
}
