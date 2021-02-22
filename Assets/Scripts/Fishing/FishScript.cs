using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

	public bool EnabledOnStart;
	private bool fishActive;

	public int CatchScore = 10;

	public float TickSpacing = .5f;
	private float timer = 0f;
	public bool WaitForHook;
	public HookScript HookToWaitFor;

	[Space]
	public List<FishScript> NextFish;
	public HookScript HookToCross;
	public bool CutLineOnCross = false;
	public bool Unhookable = false;
	public int StruggleAmount = 1;

	private SpriteRenderer fishRenderer;

	private void Start() {
		fishRenderer = GetComponent<SpriteRenderer>();
		if (EnabledOnStart) {
			Enter();
		} else {
			PlayerControllerScript.SetAlpha(fishRenderer, .1f);
		}

	}

	private void Update() {
		if (!fishActive)
			return;

		if (WaitForHook) {
			if (HookToWaitFor.HookActive)
				if (!Exit())
					EnterAllNextFish();
		} else {
			timer += Time.deltaTime;
			if (timer > TickSpacing) {
				if (!Exit())
					EnterAllNextFish();
			}
		}
	}

	public void EnterAllNextFish() {
		foreach (var fish in NextFish) {
			fish?.Enter();
		}
	}

	public void Enter() {
		PlayerControllerScript.SetAlpha(fishRenderer, 1f);
		fishActive = true;
		timer = 0;
	}

	public bool Exit() {
		PlayerControllerScript.SetAlpha(fishRenderer, .1f);
		fishActive = false;
		return HookToCross && HookToCross.Cross(this);
	}
}
