using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour {
	public bool AnyHover = false;
	public Color ReactColor;

	private Animator animator;

	public bool Fired = false;

	void Start() {
		animator = GetComponent<Animator>();
		// animator.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (Fired) return;
		BlinkingFishPlayerScript player = other.GetComponent<BlinkingFishPlayerScript>();
		if (!player) return;

		if (AnyHover || BlinkingFishEntityScript.CompareColor(player.CurrentColor, ReactColor)) {
			Fired = true;
			Debug.Log("bugs hovered");
			animator.Play("BugsRunningSAnimation2");
		}
	}

}
