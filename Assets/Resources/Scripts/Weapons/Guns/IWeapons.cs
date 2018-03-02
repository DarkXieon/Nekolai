using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IWeapons
{
	string WeaponName { get; set; }
	// The amount of damage the weapon will give to enemies
	int Power { get; set; }

	void Attack ();
}
