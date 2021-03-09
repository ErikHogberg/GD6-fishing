using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerKeyScript : MonoBehaviour {

	public static List<string> Inventory = new List<string>();

	public string Name;

	void Start() {

	}

	private void OnTriggerEnter2D(Collider2D other) {
		Inventory.Add(Name);
		Destroy(this);
	}
}
