using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour {

	public string Prompt = "";
	
	void Start() {

	}

	void Update() {

	}

	private void OnDisable() {
		DogObstacleCourseUIScript.SetPrompt(Prompt);
	}
}
