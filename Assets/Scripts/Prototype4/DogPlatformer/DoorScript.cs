using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {
	public string LevelName;

	bool opened = false;

	private Animator doorAnimator;

	private void Start() {
		doorAnimator = GetComponent<Animator>();
	}

	public void Open() {
		opened = true;
		doorAnimator.SetTrigger("Open");
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (opened)
			SceneManager.LoadScene(LevelName);
	}
}
