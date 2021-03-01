using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvScript : MonoBehaviour, BlinkingFishPlayerScript.BlinkListener {
	private Animator animator;

	public SpriteRenderer Blinker;
	private Color initColor;

	public List<BlinkingFishEntityScript.ColorSequenceEntry> TargetSequence;
	private List<(Color, float)> currentSequence = new List<(Color, float)>();

	bool receiving = false;
	bool on = false;

	private void Awake() {
		BlinkingFishPlayerScript.BlinkListeners.Add(this);
	}

	private void OnDestroy() {
		BlinkingFishPlayerScript.BlinkListeners.Remove(this);
	}

	void Start() {
		animator = GetComponent<Animator>();

	}

	public void ReceiveBlinkStart(Color color) {
		if (!receiving) return;

		Blinker.color = color;
	}

	public bool ReceiveBlinkEnd(Color color, float time) {
		Blinker.color = initColor;
		if (!receiving) return false;
		bool sequenceAccept = false;

		currentSequence.Add((color, time));
		for (int i = 0; i < TargetSequence.Count && i < currentSequence.Count; i++) {
			// if (!CompareColor(TargetSequence[i], currentSequence[i])) {
			if (!TargetSequence[i].Equals(currentSequence[i])) {
				// Debug.Log("Sequence rejected! " + TargetSequence[i].Color + ", min:" + TargetSequence[i].MinTime + ", max:" + TargetSequence[i].MaxTime + " \nvs " + currentSequence[i].Item1 + ", " + currentSequence[i].Item2);
				Debug.Log("Tv sequence rejected!");
				currentSequence.Clear();
				return false;
			}

			if (i == TargetSequence.Count - 1) {
				Debug.Log("Tv on!");
				on = !on;
				animator.SetBool("On", on);
				currentSequence.Clear();
				sequenceAccept = true;
				break;
			}
		}


		return sequenceAccept;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		receiving = true;
	}

	private void OnTriggerExit2D(Collider2D other) {
		receiving = false;
		Blinker.color = initColor;
	}

}
