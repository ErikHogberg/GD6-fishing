using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugScript : MonoBehaviour {

	private int progress = 0;

	private Animator animator;

	private float timer = -1;
	public float TimeOutTime = .5f;

	public bool SpaceProgression = false;

	void Start() {
		animator = GetComponent<Animator>();

	}

	void Update() {
		if (timer >= 0) timer -= Time.deltaTime;

		if (SpaceProgression && Input.GetKeyDown(KeyCode.Space)) {
			progress++;
			animator.SetInteger("Progress", progress);
		}

	}

	private void OnTriggerStay2D(Collider2D other) {
		if (timer >= 0) return;

		progress++;
		animator.SetInteger("Progress", progress);
		timer = TimeOutTime;
	}
}
