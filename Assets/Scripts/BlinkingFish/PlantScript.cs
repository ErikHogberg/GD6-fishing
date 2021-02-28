using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour {

	public Color GrowColor;
	public Color WitherColor;

	private Animator animator;

	// Start is called before the first frame update
	void Start() {
		animator = GetComponent<Animator>();
		animator.enabled = false;
	}

	private void OnTriggerStay2D(Collider2D other) {
		// Debug.Log("plant trigger stay");

		BlinkingFishPlayerScript player = other.GetComponent<BlinkingFishPlayerScript>();
		if (!player) return;

		if (BlinkingFishEntityScript.CompareColor(player.CurrentColor, GrowColor)) {
			// TODO: play
			// Debug.Log("playing plant");
			// animator.Play("PlantSAnimation");
			animator.SetFloat("mul", 1);
			animator.enabled = true;
		} else if (BlinkingFishEntityScript.CompareColor(player.CurrentColor, WitherColor)) {
			animator.SetFloat("mul", -1);
			animator.enabled = true;
			// TODO: play in reverse
		} else {
			animator.enabled = false;
		}

	}

	private void OnTriggerExit2D(Collider2D other) {
		// TODO: stop animation
		animator.enabled = false;
	}
}
