using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class HangmanScript : MonoBehaviour {
	private Animator animator;

	public SpriteRenderer Blood;
	private float bloodFadeTimer = -1;
	public float BloodFadeSpeed = 1f;

	int fails = 0;

	[Serializable]
	public class KeyPair {
		public KeyCode Key;
		public GameObject UnhideObject;
	}

	public List<KeyPair> CorrectKeys;

	List<KeyCode> keys = new List<KeyCode> {
		KeyCode.Q,
		KeyCode.W,
		KeyCode.E,
		KeyCode.R,
		KeyCode.T,
		KeyCode.Y,
		KeyCode.U,
		KeyCode.I,
		KeyCode.O,
		KeyCode.P,
		KeyCode.A,
		KeyCode.S,
		KeyCode.D,
		KeyCode.F,
		KeyCode.G,
		KeyCode.H,
		KeyCode.J,
		KeyCode.K,
		KeyCode.L,
		KeyCode.Z,
		KeyCode.X,
		KeyCode.C,
		KeyCode.V,
		KeyCode.B,
		KeyCode.N,
		KeyCode.M
	};

	void Start() {
		animator = GetComponent<Animator>();
	}

	void Update() {
		if (bloodFadeTimer >= 0) {
			bloodFadeTimer -= Time.deltaTime * BloodFadeSpeed;
			if (bloodFadeTimer < 0) {
				Color bloodColor = Blood.color;
				bloodColor.a = 0;
				Blood.color = bloodColor;
			} else {
				Color bloodColor = Blood.color;
				bloodColor.a = bloodFadeTimer;
				Blood.color = bloodColor;
			}
		}

		if (!Input.anyKey) return;

		for (int i = 0; i < keys.Count; i++) {
			if (Input.GetKeyDown(keys[i])) {
				KeyCode pressedKey = keys[i];
				keys.RemoveAt(i);

				bool shouldBreak = false;
				foreach (var key in CorrectKeys) {

					// if (CorrectKeys.Any(key => keys[i] == key.Key)) {
					if (pressedKey == key.Key) {
						// for (int j = 0; j < fails && j < CorrectKeys.Count; j++) {
						// CorrectKeys[j].UnhideObject.SetActive(true);
						// }
						key.UnhideObject.SetActive(true);
						shouldBreak = true;
						break;
					}
				}
				if (shouldBreak) break;
				fails++;
				Debug.Log("hangman fails: " + fails);
				bloodFadeTimer = 1;
				animator.SetInteger("Fails", fails);
				break;
			}
		}
	}
}
