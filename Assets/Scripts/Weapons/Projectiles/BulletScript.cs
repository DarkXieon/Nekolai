using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	// Add this to the bullet prefab in order to check collisions, the bullet is the trigger

	// The enemy script will have a takeAwayHealth function or some way to take it away
	void OnTriggerEnter2D (Collider2D collision)
	{

		if(collision.gameObject.tag == "Enemy")
		{
			GameObject enemy = collision.gameObject;
			// HEALTH DECREASE GOES HERE
			// if the enemy has 0 health destroy it
			//if (enemy.health <= 0) {
			Destroy (enemy);
			Destroy (gameObject);
			//}
		}


	}
}
