using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	public Text NameText;
	public Text HandText;
	public Slider HpSlider;

	public void SetHUD(Unit unit)
	{
		Debug.Log($"Setting HUD of {unit.UnitName}");
		NameText.text = unit.UnitName;
		Debug.Log($"Setting HUD of {unit.UnitName}");
		HandText.text = "Hand";
		HpSlider.maxValue = unit.MaxHP;
		HpSlider.value = unit.CurrentHP;
		Debug.Log($"End HUD of {unit.UnitName}");
	}

	public void SetHP(int hp)
	{
		HpSlider.value = hp;
	}
}
