using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {
	public string LevelName;

	public bool Opened = false;

	private Animator doorAnimator;

	private void Start() {
		doorAnimator = GetComponent<Animator>();
		if (Opened) {
			doorAnimator.SetTrigger("Open");
		}
	}

	public void Open() {
		Opened = true;
		doorAnimator.SetTrigger("Open");
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (Opened)
			SceneManager.LoadScene(LevelName);
	}
}
