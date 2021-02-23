using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WonkyPhysicsUIScript : MonoBehaviour {

	public static WonkyPhysicsUIScript MainInstance;

	public Slider ASlider;
	public Slider BSlider;
	public Slider CSlider;

	private WonkyPhysicsEntityScript selected;

	private void Awake() {
		MainInstance = this;
	}

	private void OnDestroy() {
		MainInstance = null;
	}

	void Start() {

	}

	void Update() {

	}

	public void UpdateSliders() {
		if (!selected) return;

		ASlider.value = selected.A;
		BSlider.value = selected.B;
		CSlider.value = selected.C;
	}

	public void ASliderCallback(float value) {
		if (selected)
			selected.A = value;
	}

	public void BSliderCallback(float value) {
		if (selected)
			selected.B = value;
	}

	public void CSliderCallback(float value) {
		if (selected)
			selected.C = value;
	}

	private void SetSelectedInternal(WonkyPhysicsEntityScript newSelectedEntity) {
		selected = newSelectedEntity;
		Debug.Log("Set selected to " + selected.name);

		UpdateSliders();
	}

	public static void SetSelected(WonkyPhysicsEntityScript newSelectedEntity) {
		MainInstance?.SetSelectedInternal(newSelectedEntity);
	}
}
