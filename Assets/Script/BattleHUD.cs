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
		NameText.text = unit.UnitName;
		HandText.text = "Hand";
		HpSlider.maxValue = unit.MaxHP;
		HpSlider.value = unit.CurrentHP;
	}

	public void SetHP(int hp)
	{
		HpSlider.value = hp;
	}
}
