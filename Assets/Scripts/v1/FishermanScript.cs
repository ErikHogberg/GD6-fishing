using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FishermanScript : MonoBehaviour {
	public List<SpriteRenderer> Ropes;
	public List<SpriteRenderer> Hooks;

	private SpriteRenderer fishermanRenderer;

	private void Awake() {
		fishermanRenderer = GetComponent<SpriteRenderer>();
	}

	public void DisableHooksAndRopes() {
		PlayerControllerScript.SetAlpha(fishermanRenderer, .1f);
		foreach (var item in Hooks.Concat(Ropes)) {
			PlayerControllerScript.SetAlpha(item, .1f);
		}
	}

	public int SetHook(int hookIndex) {

		DisableHooksAndRopes();
		PlayerControllerScript.SetAlpha(fishermanRenderer, 1f);

		if (hookIndex < 0) hookIndex = 0;
		if (hookIndex >= Hooks.Count) hookIndex = Hooks.Count - 1;

		for (int i = 0; i <= hookIndex; i++) {
			if (i < Ropes.Count && Ropes[i] != null) {
				PlayerControllerScript.SetAlpha(Ropes[i], 1f);
			}
		}

		if (hookIndex < Hooks.Count && Hooks[hookIndex] != null) {
			PlayerControllerScript.SetAlpha(Hooks[hookIndex], 1f);
		}

		return hookIndex;
	}


}
