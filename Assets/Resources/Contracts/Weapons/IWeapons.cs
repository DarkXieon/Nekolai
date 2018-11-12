using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IWeapons
{
	string WeaponName { get; set; }
    // The amount of damage the weapon will give to enemies
    Stat_Power Power { get; } //<-- Use Stat_Power instead


	void Attack ();
}
