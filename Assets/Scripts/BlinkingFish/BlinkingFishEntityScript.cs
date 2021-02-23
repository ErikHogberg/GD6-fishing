using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingFishEntityScript : MonoBehaviour {

	public static List<BlinkingFishEntityScript> Instances = new List<BlinkingFishEntityScript>();

	public SpriteRenderer Blinker;
	private Color initColor;
	private Color currentColor;

	public float IndicationTime = .1f;
	private float timer = -1;

	public List<Color> TargetSequence;
	private List<Color> currentSequence = new List<Color>();


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

	public bool ReceiveBlink(Color color) {
		// TODO: add color to recorded sequence
		// TODO: return true if sequence matches
		// IDEA: delete/clear old sequence if it mismatches before completion

		// IDEA: only change to new color if it fits in sequence

		bool sequenceAccept =false;

		currentSequence.Add(color);
		for (int i = 0; i < TargetSequence.Count && i < currentSequence.Count; i++) {
			if (!CompareColor(TargetSequence[i], currentSequence[i])) {
				Debug.Log("Sequence rejected! " + TargetSequence[i] + " vs " + currentSequence[i]);
				Blinker.color = initColor;
				currentColor = initColor;
				currentSequence.Clear();
				return false;
			}

			if (i == TargetSequence.Count - 1) {
				Debug.Log("Sequence accepted!");
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

	public static bool BroadcastBlink(Color color) {
		foreach (var item in Instances) {
			if (item.ReceiveBlink(color))
				return true;
		}

		return false;
	}

	private static bool CompareColor(Color color, Color otherColor) {
		return true
			&& Math.Round(color.r, 2) == Math.Round(otherColor.r, 2)
			&& Math.Round(color.g, 2) == Math.Round(otherColor.g, 2)
			&& Math.Round(color.b, 2) == Math.Round(otherColor.b, 2)
			&& Math.Round(color.a, 2) == Math.Round(otherColor.a, 2)
			;
	}

}
