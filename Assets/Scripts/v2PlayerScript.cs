using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class v2PlayerScript : MonoBehaviour {

	[Serializable]
	public class InDemandBoardEntry {
		public SpriteRenderer Fish;
		public int ExpectedFishScore;
		public int ResultingScore;
	}

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
	public int FishStruggleAmount = 3;
	private int currentStruggle = 0;

	public int BarrelScoreThreshold = 50;
	public List<SpriteRenderer> Barrels;
	public List<SpriteRenderer> Reels;
	public List<SpriteRenderer> Sweat;

	public float InDemandBoardTickTime = 10f;
	private float boardTimer = 0;
	public List<InDemandBoardEntry> InDemandBoard;
	private int currentBoardEntry = 0;

	private int spareReels = 3;

	void Start() {
		MainInstance = this;
		timer = TimeLimit;
		currentHook = StartHook;
		currentHook.SetAlphaHigh();
		currentHook.ActivateLine();
		PlayerControllerScript.SetAlpha(currentHook.Rope, 1f);
		ScoreText?.SetText(score.ToString());

		foreach (SpriteRenderer barrel in Barrels) {
			PlayerControllerScript.SetAlpha(barrel, .1f);
		}

		foreach (var item in InDemandBoard) {
			PlayerControllerScript.SetAlpha(item.Fish, .1f);
		}
		if (InDemandBoard.Count > currentBoardEntry) {
			PlayerControllerScript.SetAlpha(InDemandBoard[currentBoardEntry].Fish, 1f);
		}

		DisableSweat();

		spareReels = Reels.Count;

	}

	private void OnDestroy() {
		MainInstance = null;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.A)) {
			HookScript nextHook = currentHook.GoLeft();
			if (nextHook)
				currentHook = nextHook;
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			HookScript nextHook = currentHook.GoRight();
			if (nextHook)
				currentHook = nextHook;
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			if (fishHeld) {
				currentStruggle--;
				if (currentStruggle <= 0) {
					currentStruggle = FishStruggleAmount;
					HookScript nextHook = currentHook.GoUp();
					if (nextHook)
						currentHook = nextHook;
					DisableSweat();
				} else {
					UpdateSweat();
				}
			} else {
				HookScript nextHook = currentHook.GoUp();
				if (nextHook)
					currentHook = nextHook;
			}

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

		boardTimer += Time.deltaTime;
		if (boardTimer > InDemandBoardTickTime) {
			boardTimer -= InDemandBoardTickTime;
			PlayerControllerScript.SetAlpha(InDemandBoard[currentBoardEntry].Fish, .1f);
			currentBoardEntry = UnityEngine.Random.Range(0, InDemandBoard.Count - 1);
			PlayerControllerScript.SetAlpha(InDemandBoard[currentBoardEntry].Fish, 1f);
			Debug.Log("Board set to " + currentBoardEntry);

		}
	}

	public void ResetGame() {

		if (highScore < score) {
			highScore = score;
			HighScoreText?.SetText(highScore.ToString());
		}
		score = 0;
		timer = TimeLimit;
		ScoreText?.SetText(score.ToString());
		foreach (SpriteRenderer barrel in Barrels) {
			PlayerControllerScript.SetAlpha(barrel, .1f);
		}

		spareReels = Reels.Count;
		foreach (SpriteRenderer reel in Reels) {
			PlayerControllerScript.SetAlpha(reel, 1f);
		}

	}

	public void AddScore(int newScore) {
		score += newScore;
		ScoreText?.SetText(score.ToString());

		int barrelsFilled = score / BarrelScoreThreshold;

		for (int i = 0; i < Barrels.Count; i++) {
			PlayerControllerScript.SetAlpha(Barrels[i], i < barrelsFilled ? 1f : .1f);
		}

	}

	private void UpdateSweat() {
		for (int i = 0; i < Sweat.Count; i++) {
			PlayerControllerScript.SetAlpha(Sweat[i], i < currentStruggle - 1 ? .1f : 1f);
		}
	}

	private void DisableSweat() {
		foreach (SpriteRenderer sweat in Sweat) {
			PlayerControllerScript.SetAlpha(sweat, .1f);
		}
	}

	public void HookFish(FishScript fish) {
		fishHeld = true;
		heldScore = fish.CatchScore;
		FishStruggleAmount = fish.StruggleAmount;
		currentStruggle = FishStruggleAmount;
		UpdateSweat();
	}

	public void CollectFish() {
		if (!fishHeld)
			return;

		fishHeld = false;
		if (InDemandBoard.Count > currentBoardEntry && heldScore == InDemandBoard[currentBoardEntry].ExpectedFishScore) {
			AddScore(InDemandBoard[currentBoardEntry].ResultingScore);
		} else {
			AddScore(heldScore);
		}
		DisableSweat();
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
