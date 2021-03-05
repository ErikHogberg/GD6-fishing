using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionIngredientScript : MonoBehaviour {

	public int Amount = 5;

	[Space]
	public float ColorIntensity = 0;
	public float HealingEffectiveness = 0;
	public float Water = 0;

	private void OnMouseDown() {
		if(Amount < 1) return;

		Amount--;
		PotionContainerScript.AddIngredientToPotion(ColorIntensity, HealingEffectiveness, Water);
	}

}
