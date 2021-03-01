using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour, BlinkingFishPlayerScript.BlinkListener {
	private Animator animator;

	public SpriteRenderer Blinker;
	private Color initColor;
	public float ColorTick = .5f;
	private float alpha;

	public List<BlinkingFishEntityScript.ColorSequenceEntry> TargetSequence;
	private List<(Color, float)> currentSequence = new List<(Color, float)>();

	bool receiving = false;
	float timer = -1;
	int currentIndex = 0;

	private void Awake() {
		BlinkingFishPlayerScript.BlinkListeners.Add(this);
	}

	private void OnDestroy() {
		BlinkingFishPlayerScript.BlinkListeners.Remove(this);
	}

	void Start() {
		animator = GetComponent<Animator>();
		alpha = Blinker.color.a;
	}

	void Update() {
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = ColorTick;
			currentIndex++;
			if (currentIndex >= TargetSequence.Count) currentIndex = 0;
			Color color = TargetSequence[currentIndex].Color;
			color.a = alpha;
			Blinker.color = color;
		}
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
			if (!TargetSequence[i].Equals(currentSequence[i])) {
				Debug.Log("Chest sequence rejected!");
				currentSequence.Clear();
				return false;
			}

			if (i == TargetSequence.Count - 1) {
				Debug.Log("Chest open!");
				animator.SetTrigger("Open");
				Blinker.gameObject.SetActive(false);
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
