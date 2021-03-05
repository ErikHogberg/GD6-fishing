using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using UnityEngine.InputSystem;
// using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class SteeringScript1 : MonoBehaviour {

	public GameObject WheelFL;
	public GameObject WheelFR;
	public GameObject WheelRL;
	public GameObject WheelRR;

	public TMP_Text debugText;

	private Rigidbody carRB;

	public float AccelerationRate = 1;
	public float TurnSpeed = 1;
	public float VelocityCap = 100;

	[Space]
	public float WheelTurnMul = 1;
	private Quaternion leftWheelRot;
	private Quaternion rightWheelRot;

	private float steeringAngle = 0;
	private float gas = 0;
	private float brake = 0;
	private float handbrake = 0;
	public bool InvertSteeringOnReverse = true;

	[Space]
	[Min(0.001f)]
	public float MaxMouseGasDistance;
	public AnimationCurve MouseGasCurve;


	// public float MaxMouseSteer = 30;
	public float MouseSteerScale = 1;
	// public AnimationCurve MouseSteerCurve;

	private Vector3 resetPos;
	private Quaternion resetRot;

	[Space]
	public List<DriftTrailHandler> DriftTrails;

	public Camera MouseCamera;


	void Start() {
		carRB = GetComponent<Rigidbody>();
		resetPos = transform.position;
		resetRot = transform.rotation;

		leftWheelRot = WheelFL.transform.localRotation;
		rightWheelRot = WheelFR.transform.localRotation;

		if (!MouseCamera) MouseCamera = Camera.main;
	}

	void Update() {

		bool mouseDown = Input.GetMouseButton(0);//Pointer.current.press.isPressed;

		if(Input.GetMouseButtonUp(0)){
			gas = 0;
			steeringAngle = 0;
		}

		if(Input.GetKeyDown(KeyCode.R)) Reset();

		float clickAngle = 0;
		float clickDistance = 0;

		if (mouseDown) {

			// TODO: get mouse point on car plane, steer car towards mouse location

			Plane plane = new Plane(carRB.transform.up, carRB.position);
			Ray mousePickRay = MouseCamera.ScreenPointToRay(Input.mousePosition);//Pointer.current.position.ReadValue());

			float hitDistance;
			bool hit = plane.Raycast(mousePickRay, out hitDistance);

			if (hit) {
				Vector3 posOnPlane = mousePickRay.GetPoint(hitDistance);
				clickAngle = Vector3.SignedAngle(posOnPlane - carRB.position, carRB.transform.forward, carRB.transform.up);
				Vector3 clickPosInFrontOfCar = Vector3.Project(posOnPlane, carRB.transform.forward);
				clickDistance = Vector3.Distance(carRB.position, clickPosInFrontOfCar);

				float steeringDirSign = Mathf.Sign(clickAngle);

				float dirMul = steeringDirSign > 90 ? -1 : 1;
				float clickDistanceAbs = Mathf.Abs(clickDistance);

				if (clickDistanceAbs > 0) {
					float gasRatio = clickDistanceAbs / MaxMouseGasDistance;
					gas = MouseGasCurve.Evaluate(gasRatio) * dirMul;
				}

				steeringAngle = steeringDirSign * ((Mathf.Abs(clickAngle) * MouseSteerScale) / WheelTurnMul);
				if (Mathf.Abs(steeringAngle) > 1) {
					steeringAngle = steeringDirSign;
				}

			}

		}

		// TODO: spin wheels
		WheelFL.transform.localRotation = Quaternion.AngleAxis(steeringAngle * WheelTurnMul, Vector3.down) * leftWheelRot;
		WheelFR.transform.localRotation = Quaternion.AngleAxis(steeringAngle * WheelTurnMul, Vector3.down) * rightWheelRot;

		debugText?.SetText(
			"steering: " + steeringAngle +
			"\ngas: " + gas +
			"\nbrake: " + brake +
			"\nhandbrake: " + handbrake +
			"\nangle: " + clickAngle +
			"\nmouse distance: " + clickDistance
		);

	}

	private void OnCollisionStay(Collision other) {
		if (handbrake > 0) return;

		carRB.velocity += transform.forward * gas * AccelerationRate;
		if (carRB.velocity.sqrMagnitude > VelocityCap * VelocityCap) {
			carRB.velocity = carRB.velocity.normalized * VelocityCap;
		}

		carRB.angularVelocity += Vector3.down * steeringAngle * TurnSpeed;
	}

	// public void OnSteerLeft(InputValue value) {
	// 	steeringAngle = value.Get<float>();
	// }

	// public void OnSteerRight(InputValue value) {
	// 	steeringAngle = -value.Get<float>();
	// }

	// public void OnGas(InputValue value) {
	// 	gas = value.Get<float>();
	// }

	// public void OnBrake(InputValue value) {
	// 	// brake = value.Get<float>();
	// 	gas = -value.Get<float>();
	// }

	// public void OnHandbrake(InputValue value) {
	// 	handbrake = value.Get<float>();
	// }

	// public void OnReset(InputValue value) {
	// 	transform.position = resetPos;
	// 	transform.rotation = resetRot;
	// 	carRB.velocity = Vector3.zero;
	// 	foreach (var item in DriftTrails) {
	// 		item.Clear();
	// 	}
	// }

	public void Reset() {
		transform.position = resetPos;
		transform.rotation = resetRot;
		carRB.velocity = Vector3.zero;
		foreach (var item in DriftTrails) {
			item.Clear();
		}
	}

}
