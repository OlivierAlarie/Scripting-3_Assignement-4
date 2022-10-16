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
}
