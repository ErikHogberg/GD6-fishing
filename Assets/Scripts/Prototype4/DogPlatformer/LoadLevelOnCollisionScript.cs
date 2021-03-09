using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnCollisionScript : MonoBehaviour
{
	public string LevelName;

    private void OnTriggerEnter2D(Collider2D other) {
		SceneManager.LoadScene(LevelName);
	}
}
