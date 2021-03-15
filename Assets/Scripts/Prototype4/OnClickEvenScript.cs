using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClickEvenScript : MonoBehaviour {

	public UnityEvent Events;

	private void OnMouseDown() {
		Events.Invoke();
	}
}
