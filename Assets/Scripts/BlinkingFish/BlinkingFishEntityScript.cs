using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingFishEntityScript : MonoBehaviour {

	public static List<BlinkingFishEntityScript> Instances = new List<BlinkingFishEntityScript>();

	[Serializable]
	public class ColorSequenceEntry : IEquatable<(Color, float)> {
		public Color Color;
		[Range(0, 1)]
		public float MinTime = 0;
		[Range(0, 1)]
		public float MaxTime = 1;

		public bool Equals((Color, float) other) {
			return CompareColor(Color, other.Item1)
				&& other.Item2 >= MinTime
				&& other.Item2 <= MaxTime;
		}
	}

	public SpriteRenderer Blinker;
	private Color initColor;
	private Color currentColor;

	public float IndicationTime = .1f;
	private float timer = -1;

	public List<ColorSequenceEntry> TargetSequence;
	private List<(Color, float)> currentSequence = new List<(Color, float)>();


	private void Awake() {
		Instances.Add(this);
	}

	private void OnDestroy() {
		Instances.Remove(this);
	}

	void Start() {
		initColor = Blinker.color;
		currentColor = Blinker.color;
	}

	void Update() {
		if (timer >= 0) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				Blinker.color = currentColor;
			}
		}
	}

	public void ReceiveBlinkStart(Color color) {

		if (color == currentColor) {
			Blinker.color = initColor;
			timer = IndicationTime;
		} else {
			Blinker.color = color;
		}
		currentColor = color;

	}

	public bool ReceiveBlink(Color color, float time) {

		bool sequenceAccept = false;

		currentSequence.Add((color, time));
		for (int i = 0; i < TargetSequence.Count && i < currentSequence.Count; i++) {
			// if (!CompareColor(TargetSequence[i], currentSequence[i])) {
			if (!TargetSequence[i].Equals(currentSequence[i])) {
				// Debug.Log("Sequence rejected! " + TargetSequence[i].Color + ", min:" + TargetSequence[i].MinTime + ", max:" + TargetSequence[i].MaxTime + " \nvs " + currentSequence[i].Item1 + ", " + currentSequence[i].Item2);
				Blinker.color = initColor;
				currentColor = initColor;
				currentSequence.Clear();
				return false;
			}

			if (i == TargetSequence.Count - 1) {
				// Debug.Log("Sequence accepted!");
				currentSequence.Clear();
				sequenceAccept = true;
				break;
			}
		}

		if (color == currentColor) {
			Blinker.color = initColor;
			timer = IndicationTime;
		} else {
			Blinker.color = color;
		}
		currentColor = color;

		return sequenceAccept;
	}

	public static bool BroadcastBlink(Color color, float time) {
		foreach (var item in Instances) {
			if (item.ReceiveBlink(color, time))
				return true;
		}

		return false;
	}

	public static void BroadcastBlinkStart(Color color) {
		foreach (var item in Instances) {
			item.ReceiveBlinkStart(color);
		}
	}

	public static bool CompareColor(Color color, Color otherColor) {
		return true
			&& Math.Round(color.r, 2) == Math.Round(otherColor.r, 2)
			&& Math.Round(color.g, 2) == Math.Round(otherColor.g, 2)
			&& Math.Round(color.b, 2) == Math.Round(otherColor.b, 2)
			// && Math.Round(color.a, 2) == Math.Round(otherColor.a, 2)
			;
	}

}
