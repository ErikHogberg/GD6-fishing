using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonkyPhysicsEntityScript : MonoBehaviour {

	public static List<WonkyPhysicsEntityScript> Instances = new List<WonkyPhysicsEntityScript>();

	public enum WonkyProperty {
		A,
		B,
		C
	}

	[Serializable]
	public class WonkyPhysicsLaw {
		public WonkyProperty Property;
		public WonkyProperty OpposingProperty;
		public float Magnitude = 0;
		public bool OppositeAttraction = true;
		// IDEA: list of thresholds or modifiers, such as larget than value, less than value, less than distance, magnitude/multiplier, curve
	}

	private Rigidbody2D rb;

	[Range(-1, 1)]
	public float A = 0;
	[Range(-1, 1)]
	public float B = 0;
	[Range(-1, 1)]
	public float C = 0;

	// IDEA: shift color and change size with sliders
	// IDEA: add strange halo with alpha tied to slider

	public List<WonkyPhysicsLaw> Laws;

	private void Awake() {
		Instances.Add(this);
	}

	private void OnDestroy() {
		Instances.Remove(this);
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {

		foreach (var law in Laws) {

			foreach (var instance in Instances) {
				if (instance == this) continue;

				float amount = 0;
				switch (law.Property) {
					case WonkyProperty.A:
						amount = A;
						break;
					case WonkyProperty.B:
						amount = B;
						break;
					case WonkyProperty.C:
						amount = C;
						break;
				}

				float opposingAmount = 0;
				switch (law.OpposingProperty) {
					case WonkyProperty.A:
						opposingAmount = instance.A;
						break;
					case WonkyProperty.B:
						opposingAmount = instance.B;
						break;
					case WonkyProperty.C:
						opposingAmount = instance.C;
						break;
				}

				instance.rb.AddForce(
					(transform.position - instance.transform.position).normalized
						* (amount + opposingAmount)
						* law.Magnitude
						* Time.deltaTime,
					ForceMode2D.Force
				);

				if (law.OppositeAttraction) {


					rb.AddForce(
						(transform.position - instance.transform.position).normalized
							* (amount + opposingAmount)
							* -law.Magnitude
							* Time.deltaTime,
						ForceMode2D.Force
					);
				}
			}
		}
	}

	private void OnMouseDown() {
		Debug.Log("Clicked " + name);
		WonkyPhysicsUIScript.SetSelected(this);
	}
}
