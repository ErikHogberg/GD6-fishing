using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionContainerScript : MonoBehaviour {

	private static PotionContainerScript mainInstance;

	private float colorIntensity = 0;
	private float healingEffectiveness = 0;
	private float water = 0;

	// public float ColorPercentReq = 30;
	// public float HealingPercentReq = 30;
	public AnimationCurve ColorValueCurve;
	public AnimationCurve HealingValueCurve;

	public float Volume => colorIntensity + healingEffectiveness + water;

	public GameObject ContentsContainer;
	public TMP_Text UIText;

	private void Awake() {
		mainInstance = this;
	}

	private void OnDestroy() {
		mainInstance = null;
	}

	void Start() {
		UpdateColor();
	}

	void UpdateColor(){
		Vector3 contentsScale = ContentsContainer.transform.localScale;
		contentsScale.y = Volume * 0.01f;
		ContentsContainer.transform.localScale = contentsScale;
	}

	public bool AddIngredient(float color, float healing, float water) {
		if (Volume > 100)
			return false;

		colorIntensity += color;
		healingEffectiveness += healing;
		this.water += water;

		if (Volume > 100) {
			water -= Volume - 100;
			if (water < 0) {
				colorIntensity += water;
				water = 0;
				if (colorIntensity < 0) {
					healingEffectiveness += colorIntensity;
					colorIntensity = 0;
				}
			}

			// TODO: ensure volume is below 100

		}

		UpdateColor();

		UIText?.SetText(
			"color: " + colorIntensity
			+ "\nhealing: " + healingEffectiveness
			+ "\nwater: " + this.water
		);

		return true;
	}

	public static bool AddIngredientToPotion(float color, float healing, float water) {
		if (mainInstance) {
			return mainInstance.AddIngredient(color, healing, water);
		} else {
			return false;
		}
	}

}
