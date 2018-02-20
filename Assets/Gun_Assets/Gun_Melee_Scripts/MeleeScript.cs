using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {

	public GameObject Player;

	// The melee weapon should have a collider, if no weapon then collider determines if we are in range for attack
	bool inRange = false;

	// This is a value that is set true only when we are currently doing an attack, to keep a delay between attacks
	bool attacking = false;

	// Timer to delay the time between attacks
	private float attack_Delay = .25f;
	private float attack_Timer = 0f;

	// This will tell the bullet to go left or right depending on which way the sprite is facing
	private Rigidbody2D positionOfPlayer;
	private float OldPosition;

	private bool forward = true;

	private GameObject currentEnemy;

	// If we hit the trigger we are in range of the enemy
	void OntriggerEnter(Collider2D col)
	{
		inRange = true;
		// This will get the component we are currently touching and make them our current enemy
		// If we need to filter it I can do this with tags, just a general test here
		currentEnemy = col.GetComponentInParent<GameObject> ();
	}

	// If we exit the collider range, it will tell the inRange value we are not within range 
	void OntriggerExit(Collider2D col)
	{
		inRange = false;
	}

	// Use this for initialization
	void Start () {
		positionOfPlayer = Player.GetComponent<Rigidbody2D> ();
	}


	void DealDamage() 
	{
		// This will be implemented when we know how much damage to deal
		// currentEnemy.Health -= damageDealt;
	}
	// Update is called once per frame
	void Update () 
	{
		OldPosition = positionOfPlayer.transform.position.x;

		if (inRange && !attacking) 
		{
			DealDamage ();
		}

		// If the old position is greater or equal to the current, the player is facing forward
		if (OldPosition >= positionOfPlayer.transform.position.x) {
			forward = true;
		}
		// Else we have to send it left because we are facing the opposite direction
		else
			forward = false;

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
