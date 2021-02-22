using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

	public static PlayerControllerScript MainInstance = null;

	public List<FishermanScript> Fishermen;
	public int currentFisherman = 0;
	private int fishermanResetIndex = 0;
	public int hookIndex = 0;

	public float TimeLimit = 20f;
	private float timer = 20f;
	private int score = 0;
	public TMP_Text ScoreText;
	public TMP_Text TimeText;

	void Start() {
		MainInstance = this;
		timer = TimeLimit;
		fishermanResetIndex = currentFisherman;
		UpdateFishermenAlpha();
		ScoreText?.SetText("Score: " + score);

	}

	private void OnDestroy() {
		MainInstance = null;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.A)) {
			currentFisherman--;
			if (currentFisherman < 0) currentFisherman = 0;
			UpdateFishermenAlpha();
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			currentFisherman++;
			if (currentFisherman >= Fishermen.Count) currentFisherman = Fishermen.Count - 1;
			UpdateFishermenAlpha();
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			hookIndex--;
			hookIndex = Fishermen[currentFisherman].SetHook(hookIndex);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			hookIndex++;
			hookIndex = Fishermen[currentFisherman].SetHook(hookIndex);
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
		foreach (var item in Fishermen) {
			item.DisableHooksAndRopes();
		}

		if (currentFisherman >= 0 && currentFisherman < Fishermen.Count)
			hookIndex = Fishermen[currentFisherman].SetHook(hookIndex);

	}

	public static void SetAlpha(SpriteRenderer renderer, float alpha) {
		Color color = renderer.color;
		color.a = alpha;
		renderer.color = color;
	}

	public void ResetGame() {
		score = 0;
		timer = TimeLimit;
		hookIndex = 0;
		currentFisherman = fishermanResetIndex;
		UpdateFishermenAlpha();
		ScoreText?.SetText("Score: " + score);
	}

	public void ResetHook(){
		hookIndex = 0;
		UpdateFishermenAlpha();
	}

	public void AddScore(int newScore) {
		score += newScore;
		ScoreText?.SetText("Score: " + score);
	}
}
