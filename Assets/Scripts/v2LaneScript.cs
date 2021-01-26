using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class v2LaneScript : MonoBehaviour {
	public FishScript StartFish;
	private FishScript currentFish;

	public int CatchScore = 10;

	public float TickSpacing = .5f;
	private float timer = 0f;

	void Start() {
		currentFish = StartFish;
	}

	void Update() {
		timer += Time.deltaTime;
		while (timer > TickSpacing) {
			timer -= TickSpacing;
		
			
		}
	}
}
