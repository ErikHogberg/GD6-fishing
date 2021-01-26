using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

	public bool CollectFish = false;

	public HookScript UpHook;
	public HookScript DownHook;
	public HookScript LeftHook;
	public HookScript RightHook;

	public SpriteRenderer Rope;

	private SpriteRenderer hookRenderer;
	public SpriteRenderer FishRenderer;

	bool hookActive = false;
	public bool HookActive => hookActive;
	bool lineActive = false;
	public bool LineActive => LineActive;

	void Start() {
		hookRenderer = GetComponent<SpriteRenderer>();
		if (hookActive) {
			PlayerControllerScript.SetAlpha(hookRenderer, 1f);
			PlayerControllerScript.SetAlpha(Rope, 1f);
		} else {
			PlayerControllerScript.SetAlpha(hookRenderer, .1f);
			PlayerControllerScript.SetAlpha(Rope, .1f);
			PlayerControllerScript.SetAlpha(FishRenderer, .1f);
		}
	}

	public HookScript GoLeft() {
		if (LeftHook) {
			ExitSideways();
			LeftHook.EnterSideways();
		}

		return LeftHook;
	}

	public HookScript GoRight() {
		if (RightHook) {
			ExitSideways();
			RightHook.EnterSideways();
		}

		return RightHook;
	}
	public HookScript GoUp() {
		if (UpHook) {
			SetAlphaLow();
			lineActive = false;
			if (Rope)
				PlayerControllerScript.SetAlpha(Rope, .1f);
			UpHook.EnterUpwards();
		}

		return UpHook;
	}
	public HookScript GoDown() {
		if (DownHook) {
			SetAlphaLow();
			DownHook.EnterDownwards();
		}

		return DownHook;
	}

	public void EnterUpwards() {
		if (CollectFish)
			v2PlayerScript.MainInstance.CollectFish();

		SetAlphaHigh(v2PlayerScript.MainInstance.FishHeld);
	}

	public void EnterDownwards() {
		ActivateLine();
		SetAlphaHigh(v2PlayerScript.MainInstance.FishHeld);
	}

	public void ExitSideways() {
		SetInactiveUpwardsRecursive();
	}

	public void EnterSideways() {
		PlayerControllerScript.SetAlpha(hookRenderer, 1f);
		if (v2PlayerScript.MainInstance.FishHeld)
			PlayerControllerScript.SetAlpha(FishRenderer, 1f);
		hookActive = true;
		SetActiveUpwardsRecursive();
	}

	public void SetAlphaLow() {
		hookActive = false;
		PlayerControllerScript.SetAlpha(hookRenderer, .1f);
		PlayerControllerScript.SetAlpha(FishRenderer, .1f);
	}

	public void SetAlphaHigh(bool fishHooked = false) {
		hookActive = true;
		// lineActive = true;
		if (hookRenderer)
			PlayerControllerScript.SetAlpha(hookRenderer, 1f);
		if (fishHooked)
			PlayerControllerScript.SetAlpha(FishRenderer, 1f);

	}

	public void ActivateLine() {
		lineActive = true;
		if (Rope)
			PlayerControllerScript.SetAlpha(Rope, 1f);
	}

	public void SetActiveUpwardsRecursive() {
		// PlayerControllerScript.SetAlpha(hookRenderer, 1f);
		ActivateLine();
		if (UpHook) UpHook.SetActiveUpwardsRecursive();
	}

	public void SetInactiveUpwardsRecursive() {
		PlayerControllerScript.SetAlpha(hookRenderer, .1f);
		PlayerControllerScript.SetAlpha(FishRenderer, .1f);

		hookActive = false;
		lineActive = false;
		lineActive = false;

		if (Rope)
			PlayerControllerScript.SetAlpha(Rope, .1f);
		if (UpHook) UpHook.SetInactiveUpwardsRecursive();
	}

	public HookScript GoUpToFirstRecursive() {
		if (UpHook) {
			PlayerControllerScript.SetAlpha(hookRenderer, .1f);
			PlayerControllerScript.SetAlpha(FishRenderer, .1f);

			hookActive = false;
			lineActive = false;
			lineActive = false;

			if (Rope)
				PlayerControllerScript.SetAlpha(Rope, .1f);

			return UpHook.GoUpToFirstRecursive();
		} else {
			SetAlphaHigh(v2PlayerScript.MainInstance.FishHeld);
			return this;
		}
	}

	public bool Cross(int points, bool cut, bool hookable) {
		if (hookActive && !v2PlayerScript.MainInstance.FishHeld && hookable) {
			v2PlayerScript.MainInstance.HookFish(points);
			PlayerControllerScript.SetAlpha(FishRenderer, 1f);
			return true;
		} else if (lineActive && cut) {
			// NOTE: crossing a hook that already holds a fish will cut the line (if cut enabled)
			v2PlayerScript.MainInstance.CutLine();
		}

		return false;
	}

}
