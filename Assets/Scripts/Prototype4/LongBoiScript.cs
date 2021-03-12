using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LongBoiScript : MonoBehaviour {

	private SpriteRenderer dogRenderer;

	public AnimationCurve ExpandCurve;

	float timer = -1;

	bool clicked = false;
	bool shrinking = false;
	bool flipShrinkCurve = false;

	public float ExpandTime = 1;

	private float initWidth;
	private float initX;
	public float TargetWidth;

	public UnityEvent WinEvent;

	void Start() {
		dogRenderer = GetComponent<SpriteRenderer>();
		initWidth = dogRenderer.size.x;
		initX = dogRenderer.transform.localPosition.x;
	}

	void Update() {
		if (timer < 0) return;
		timer -= Time.deltaTime;

		float t = ExpandCurve.Evaluate(timer / ExpandTime);

		if (shrinking) {
			// TODO: shrink doggo
			float width = initWidth + (TargetWidth - initWidth);
			Vector2 size = new Vector2(width * t, dogRenderer.size.y);
			dogRenderer.size = size;
			Vector3 pos = dogRenderer.transform.localPosition;
			pos.x = initX + (width + width * (1 - t)) * dogRenderer.transform.localScale.x;
			dogRenderer.transform.localPosition = pos;
		} else {
			// TODO: E X P A N D doggo
			float width = initWidth + (TargetWidth - initWidth) * (1 - t);
			Vector2 size = new Vector2(width, dogRenderer.size.y);
			dogRenderer.size = size;
			Vector3 pos = dogRenderer.transform.localPosition;
			pos.x = initX + width * dogRenderer.transform.localScale.x;
			dogRenderer.transform.localPosition = pos;

		}

		if (timer >= 0) return;

		if (shrinking) {
			// TODO: trigger "you win" thingie
			WinEvent.Invoke();
		} else {
			shrinking = true;
			timer = ExpandTime;
		}

	}

	private void OnMouseDown() {
		if (clicked) return;

		clicked = true;
		timer = ExpandTime;
	}

}
