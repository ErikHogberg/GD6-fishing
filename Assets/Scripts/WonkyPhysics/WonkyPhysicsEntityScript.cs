using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonkyPhysicsEntityScript : MonoBehaviour {

	public static List<WonkyPhysicsEntityScript> Instances = new List<WonkyPhysicsEntityScript>();

	private Rigidbody2D rb;

	public float AttractionForce = 0;
	public bool OppositeAttraction = true;

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
		foreach (var instance in Instances) {
			if (instance == this) continue;

			instance.rb.AddForce(
				(transform.position - instance.transform.position).normalized
					* AttractionForce
					* Time.deltaTime,
				ForceMode2D.Force
			);

			if (OppositeAttraction){
				rb.AddForce(
					(transform.position - instance.transform.position).normalized
						* -AttractionForce
						* Time.deltaTime,
					ForceMode2D.Force
				);
			}
		}
	}
}
