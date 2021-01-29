using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawnerScript : MonoBehaviour {
	float timer = 0;
	public float TickSpacing = 5f;

	public FishScript SpawnLocation;

	void Update() {
		timer += Time.deltaTime;
		while (timer > TickSpacing) {
			timer -= TickSpacing;
			SpawnLocation?.Enter();
		}
	}
}
