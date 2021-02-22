using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour {

	public bool HeadingLeft = true;
	public int hookIndex = 0;

	public int CatchScore = 10;

	public float TickSpacing = .5f;
	private float timer = 0f;

	public List<SpriteRenderer> LaneItems;
	// IDEA: use reference-based collision using a list of structs/objects with a lane item and a node to check collision with on exit

	private int currentLaneIndex = 0;

	private void Start() {
		UpdateAlpha();
	}

	private void Update() {
		timer += Time.deltaTime;
		while (timer > TickSpacing) {
			timer -= TickSpacing;

			bool crossedLine = HeadingLeft ?
				PlayerControllerScript.MainInstance.Fishermen.Count - PlayerControllerScript.MainInstance.currentFisherman - 1 == currentLaneIndex
				: PlayerControllerScript.MainInstance.currentFisherman == currentLaneIndex;

			if (crossedLine) {
				int playerHookIndex = PlayerControllerScript.MainInstance.hookIndex;
				if (playerHookIndex == hookIndex) {
					// TODO: catch fish
					PlayerControllerScript.MainInstance.AddScore(CatchScore);
					PlayerControllerScript.MainInstance.ResetHook();
				} else if (playerHookIndex > hookIndex) {
					// TODO: cut line
					PlayerControllerScript.MainInstance.ResetHook();
				}
			}

			currentLaneIndex++;
			if (currentLaneIndex >= LaneItems.Count) {
				currentLaneIndex = 0;
			}
			UpdateAlpha();
		}

	}

	private void UpdateAlpha() {
		foreach (var item in LaneItems) {
			PlayerControllerScript.SetAlpha(item, .1f);
		}

		PlayerControllerScript.SetAlpha(LaneItems[currentLaneIndex], 1f);
	}


}
