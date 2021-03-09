using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DogObstacleCourseUIScript : MonoBehaviour {

	public TMP_Text PromptBox;

	private static DogObstacleCourseUIScript mainInstance = null;

	private void Awake() {
		mainInstance = this;
	}

	private void OnDestroy() {
		mainInstance = null;
	}

	void Start() {

	}

	void Update() {

	}

	public static void SetPrompt(string text){
		mainInstance?.PromptBox.SetText(text);
	}
}
