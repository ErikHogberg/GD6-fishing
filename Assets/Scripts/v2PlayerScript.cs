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
	private int highScore = 0;
	public TMP_Text HighScoreText;
	public TMP_Text ScoreText;
	public TMP_Text TimeText;

	private bool fishHeld = false;
	public bool FishHeld => fishHeld;
	private int heldScore = 0;

	public int BarrelScoreThreshold = 50;
	public List<SpriteRenderer> Barrels;
	public List<SpriteRenderer> Reels;

	private int spareReels = 3;

	// Start is called before the first frame update
	void Start() {
		MainInstance = this;
		timer = TimeLimit;
		currentHook = StartHook;
		currentHook.SetAlphaHigh();
		currentHook.ActivateLine();
		PlayerControllerScript.SetAlpha(currentHook.Rope, 1f);
		UpdateFishermenAlpha();
		ScoreText?.SetText(score.ToString());

		foreach (SpriteRenderer barrel in Barrels) {
			PlayerControllerScript.SetAlpha(barrel, .1f);
		}

		spareReels = Reels.Count;

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
		TimeText?.SetText(TimeSpan.FromSeconds(timer).ToString());
	}

	private void UpdateFishermenAlpha() {
		// foreach (var item in Fishermen) {
		// item.DisableHooksAndRopes();
		// }

		// if (currentFisherman >= 0 && currentFisherman < Fishermen.Count)
		// hookIndex = Fishermen[currentFisherman].SetHook(hookIndex);

	}

	public void ResetGame() {

		if (highScore < score) {
			highScore = score;
			HighScoreText?.SetText(highScore.ToString());
		}
		score = 0;
		timer = TimeLimit;
		UpdateFishermenAlpha();
		ScoreText?.SetText(score.ToString());
		foreach (SpriteRenderer barrel in Barrels) {
			PlayerControllerScript.SetAlpha(barrel, .1f);
		}

		spareReels = Reels.Count;
		foreach (SpriteRenderer reel in Reels) {
			PlayerControllerScript.SetAlpha(reel, 1f);
		}

	}

	public void ResetHook() {
		UpdateFishermenAlpha();
	}

	public void AddScore(int newScore) {
		score += newScore;
		ScoreText?.SetText(score.ToString());

		int barrelsFilled = score / BarrelScoreThreshold;

		for (int i = 0; i < Barrels.Count; i++) {
			PlayerControllerScript.SetAlpha(Barrels[i], i < barrelsFilled ? 1f : .1f);
		}

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

		spareReels--;
		if (spareReels < 0) {
			ResetGame();
		} else {
			for (int i = 0; i < Reels.Count; i++) {
				PlayerControllerScript.SetAlpha(Reels[i], i < spareReels ? 1f : .1f);
			}
		}

	}
}
