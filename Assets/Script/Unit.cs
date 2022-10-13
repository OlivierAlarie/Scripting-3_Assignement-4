using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public string UnitName;
	public int Damage;
	public int MaxHP;
	public int CurrentHP;
	public string CurrentHand;

	public bool TakeDamage(int dmg)
	{
		CurrentHP -= dmg;

		if (CurrentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		CurrentHP += amount;
		if (CurrentHP > MaxHP)
			CurrentHP = MaxHP;
	}

	public void HandChosen(string hand)
	{
		if (hand == "rock")
		{
			CurrentHand = "Rock";
		}
		
		else if (hand == "paper")
		{
			CurrentHand = "Paper";
		}

		else {CurrentHand = "Scissors";}
	}
}
