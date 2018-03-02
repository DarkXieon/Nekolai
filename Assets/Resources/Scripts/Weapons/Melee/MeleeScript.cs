using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour, IWeapons {

	// Put this script directly on the weapon
	public Transform weapon;
	public Transform hand;

	private GameObject enemy;
	// The melee weapon should have a collider, if no weapon then collider determines if we are in range for attack
	private bool inRange = false;

	// This is a value that is set true only when we are currently doing an attack, to keep a delay between attacks
	private bool attacking = false;

	// Timer to delay the time between attacks
	private float attack_Delay = .25f;
	private float attack_Timer = 0f;


	private string _name = "Melee";
	private int _power = 4;

	public string WeaponName
	{
		get{ return _name; }
		set{ _name = value; }
	}

	public int Power
	{
		get { return _power; }
		set { _power = value; }
	}


	// If we hit the trigger we are in range of the enemy
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Enemy")
		{
			// Set the current enemy to this collision
			enemy = collision.gameObject;
			// Set in range to true
			inRange = true;
		}

	 }

	// If we exit the collider range, it will tell the inRange value we are not within range
	void OnTriggerExit2D(Collider2D col)
	{
		inRange = false;
	}

	public void Attack ()
	{
		attacking = true;
		// If we are in range we can attack our enemy
		if (inRange)
		{
			// Here we would do a MoveWeapon function which would move the sword on a hinge joint.
			// DECREASE IN HEALTH GOES HERE TO gameobject enemy
			Destroy (enemy);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown("space") && !attacking)
		{
			Attack ();
		};

		if (attacking)
		{
			// If already attacking start timer until it reaches proper delay time
			attack_Timer += Time.deltaTime;

			if (attack_Timer >= attack_Delay)
			{
				// Reset timer and set attacking to false to start the process over and allow more shooting
				attack_Timer = 0f;
				attacking = false;

			}
		}
	}
}
