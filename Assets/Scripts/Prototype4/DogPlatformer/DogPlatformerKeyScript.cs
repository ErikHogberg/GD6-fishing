using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerKeyScript : MonoBehaviour {

	// public static List<string> Inventory = new List<string>();

	// public string Name;
	public DoorScript DoorToOpen;


	private void OnTriggerEnter2D(Collider2D other) {
		// Inventory.Add(Name);
		// Destroy(this);
		DoorToOpen?.Open();
		gameObject.SetActive(false);
	}
}
