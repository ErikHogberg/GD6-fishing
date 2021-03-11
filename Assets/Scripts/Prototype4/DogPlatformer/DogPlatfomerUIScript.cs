using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogPlatfomerUIScript : MonoBehaviour {

	private static DogPlatfomerUIScript mainInstance;

	public Image TrustBar;

	private RectTransform trustBarRect;
	// private float trustBarMaxWidth;

	private void Awake() {
		mainInstance = this;
	}

	private void OnDestroy() {
		mainInstance = null;
	}

	void Start() {
		// trustBarMaxWidth = TrustBar.rectTransform.rect.width;
		// trustBarRect = TrustBar.GetComponent<RectTransform>();
		trustBarRect = TrustBar.rectTransform;
	}

	// float percentage = 1;
	// void Update() {
	// 	if (Input.GetKeyDown(KeyCode.Alpha1)) {
	// 		percentage *= 1.2f;
	// 		SetBarPercent(percentage);
	// 	} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
	// 		percentage *= 0.8f;
	// 		SetBarPercent(percentage);
	// 	}
	// }

	public static void SetBarPercent(float percentage) {
		Vector2 size = mainInstance.trustBarRect.localScale;
		// size.x = mainInstance.trustBarMaxWidth * percentage;
		size.x = percentage;
		mainInstance.trustBarRect.localScale = size;
	}
}
