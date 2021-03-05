using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class DriftTrailHandler : MonoBehaviour {

	private TrailRenderer trail;
	private bool active = false;

	void Start() {
		trail = GetComponent<TrailRenderer>();
	}

	private void OnTriggerStay(Collider other) {
		if(!active) return;
		trail.emitting = true;
	}

	private void OnTriggerExit(Collider other) {
		if(!active) return;
		// Debug.Log("disabled trail " + gameObject.name);
		// FIXME: not disabling trail emission
		trail.emitting = false;
	}

	public void Clear() {
		trail.Clear();
	}

	public void ActivateTrail(){
		active = true;
	}

	public void DeactivateTrail(){
		active = false;
		trail.emitting = false;
	}

}
