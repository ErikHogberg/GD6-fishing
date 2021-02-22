using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingFishEntityScript : MonoBehaviour {

	public static List<BlinkingFishEntityScript> Instances = new List<BlinkingFishEntityScript>();

	public SpriteRenderer Blinker;

	private void Awake() {
		Instances.Add(this);
	}

	private void OnDestroy() {
		Instances.Remove(this);
	}

	void Start() {

	}

	void Update() {

	}

	public bool ReceiveBlink(Color color) {
		// TODO: add color to recorded sequence
		// TODO: return true if sequence matches
		// IDEA: delete/clear old sequence if it mismatches before completion

		// IDEA: only change to new color if it fits in sequence
		Blinker.color = color;

		return false;
	}

	public static bool BroadcastBlink(Color color) {
		foreach (var item in Instances) {
			if (item.ReceiveBlink(color))
				return true;
		}

		return false;
	}

}
