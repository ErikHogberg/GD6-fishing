using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class v2PlayerScript : MonoBehaviour {
	public static v2PlayerScript MainInstance = null;

	public HookScript StartHook;
	private HookScript currentHook;

	public float TimeLimit = 20f;
	private float timer = 20f;
	private int score = 0;
	public TMP_Text ScoreText;
	public TMP_Text TimeText;

	private bool fishHeld = false;
	public bool FishHeld => fishHeld;
	private int heldScore = 0;

	// Start is called before the first frame update
	void Start() {
		MainInstance = this;
		timer = TimeLimit;
		currentHook = StartHook;
		currentHook.SetAlphaHigh();
		currentHook.ActivateLine();
		PlayerControllerScript.SetAlpha(currentHook.Rope, 1f);
		UpdateFishermenAlpha();
		ScoreText?.SetText("Score: " + score);

	}

	private void OnDestroy() {
		MainInstance = null;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.A)) {
			HookScript nextHook = currentHook.GoLeft();
			if (nextHook)
				currentHook = nextHook;
			// UpdateFishermenAlpha();
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			HookScript nextHook = currentHook.GoRight();
			if (nextHook)
				currentHook = nextHook;
			// UpdateFishermenAlpha();
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			HookScript nextHook = currentHook.GoUp();
			if (nextHook)
				currentHook = nextHook;

		}
		if (Input.GetKeyDown(KeyCode.S)) {
			HookScript nextHook = currentHook.GoDown();
			if (nextHook)
				currentHook = nextHook;

		}

		if (Input.GetKeyDown(KeyCode.R)) {
			ResetGame();
		}

		timer -= Time.deltaTime;
		if (timer < 0)
			ResetGame();
		TimeText?.SetText("Time: " + TimeSpan.FromSeconds(timer));
	}

	private void UpdateFishermenAlpha() {
		// foreach (var item in Fishermen) {
		// item.DisableHooksAndRopes();
		// }

		// if (currentFisherman >= 0 && currentFisherman < Fishermen.Count)
		// hookIndex = Fishermen[currentFisherman].SetHook(hookIndex);

	}

	public void ResetGame() {
		score = 0;
		timer = TimeLimit;
		UpdateFishermenAlpha();
		ScoreText?.SetText("Score: " + score);
	}

	public void ResetHook() {
		UpdateFishermenAlpha();
	}

	public void AddScore(int newScore) {
		score += newScore;
		ScoreText?.SetText("Score: " + score);
	}

	public void HookFish(int points) {
		fishHeld = true;
		heldScore = points;
	}

	public void CollectFish() {
		fishHeld = false;
		AddScore(heldScore);
	}

	public void CutLine() {
		fishHeld = false;
		currentHook = currentHook.GoUpToFirstRecursive();
		// TODO: subtract one spare hook
		// TODO: reset game if out of hooks
		Debug.Log("Cut line");
	}
}
