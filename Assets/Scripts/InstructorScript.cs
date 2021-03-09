using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorScript : MonoBehaviour {

	[Serializable]
	public class PromptEntry {
		public string Prompt;
		public CheckpointScript Checkpoint;
	}

	public float MinPromptRandomWait;
	public float MaxPromptRandomWait;
	public float RandomPromptChance;

	public List<PromptEntry> Prompts;
	public List<string> RandomPrompts;


	void Start() {

		foreach (var item in Prompts) {
			item.Checkpoint.Prompt = item.Prompt;
		}

	}

	void Update() {

	}
}
