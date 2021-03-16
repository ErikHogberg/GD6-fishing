using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogPlatfomerUIScript : MonoBehaviour {

	private static DogPlatfomerUIScript mainInstance;

	public Image TrustBar;

	private RectTransform trustBarRect;
	// private float trustBarMaxWidth;

	public float FillSpeed = 1;
	private float targetTrust = 1;
	private float fillBuffer = 1;

	public bool SetStartTrust = false;
	public float StartTrust = 1;

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
		if (SetStartTrust) {
			SetInstanceBarPercent(StartTrust);
			fillBuffer = StartTrust;
		}
	}

	private void Update() {
		if (fillBuffer != targetTrust) {
			fillBuffer = Mathf.MoveTowards(fillBuffer, targetTrust, FillSpeed * Time.deltaTime);
			SetInstanceBarPercent(fillBuffer);
		}
	}

	public void SetInstaceTargetFill(float percentage){
		targetTrust = percentage; 
	}

	public static void SetTargetFill(float percentage){
		mainInstance?.SetInstaceTargetFill(percentage);
	}

	public void SetInstanceBarPercent(float percentage) {
		Vector2 size = trustBarRect.localScale;
		// size.x = mainInstance.trustBarMaxWidth * percentage;
		size.x = percentage;
		trustBarRect.localScale = size;
	}

	public static void SetBarPercent(float percentage) {
		mainInstance?.SetInstanceBarPercent(percentage);
	}
}
