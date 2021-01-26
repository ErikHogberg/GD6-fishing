using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerControllerScript))]
public class PlayerControllerEditorScript : Editor {

	SerializedProperty timeLimit;
	SerializedProperty timeText;
	SerializedProperty scoreText;

	public override void OnInspectorGUI() {
		serializedObject.Update();

		timeLimit = serializedObject.FindProperty("TimeLimit");
		timeText = serializedObject.FindProperty("TimeText");
		scoreText = serializedObject.FindProperty("ScoreText");

		PlayerControllerScript playerController = (PlayerControllerScript)target;

		EditorGUI.BeginChangeCheck();
		

		for (int i = 0; i < playerController.Fishermen.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			bool toggle = EditorGUILayout.Toggle(playerController.currentFisherman == i);
			if (toggle && playerController.currentFisherman != i) {
				playerController.currentFisherman = i;
			}
			playerController.Fishermen[i] = (FishermanScript)EditorGUILayout.ObjectField(playerController.Fishermen[i], typeof(FishermanScript), true);
			EditorGUILayout.EndHorizontal();
		}
		if(GUILayout.Button("Add")){
			playerController.Fishermen.Add(null);
		}

		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(timeLimit);
		EditorGUILayout.PropertyField(timeText);
		EditorGUILayout.PropertyField(scoreText);

		// base.OnInspectorGUI();

		if (EditorGUI.EndChangeCheck()) {
			// FIXME: folding counts as script change
			Undo.RecordObject(playerController, "Player controller Change");
			EditorUtility.SetDirty(playerController);
		}

		serializedObject.ApplyModifiedProperties();

	}
}
