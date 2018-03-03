using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage; // the damage to deal to the object on contact (is >= 0)
    public bool owner; // true if bullet is from the Player, false if bullet is from Enemies, Map, etc.

	// Add this to the bullet prefab in order to check collisions, the bullet is the trigger

	// The enemy script will have a takeAwayHealth function or some way to take it away
	void OnTriggerEnter2D (Collider2D collision)
	{
        /* Comment out by Kermit -> get their Stat_Health to check
		if(collision.gameObject.tag == "Enemy")
		{
			GameObject enemy = collision.gameObject;
			// HEALTH DECREASE GOES HERE
			// if the enemy has 0 health destroy it
			//if (enemy.health <= 0) {
			Destroy (enemy);
			Destroy (gameObject);
			//}
		}*/

        // Deal damage if possible, but not to the Owner of the source of damage
        if(collision.gameObject.GetComponent<Stat_Health>() != null)
        {
            if(collision.gameObject.tag == "Player")
            {
                if(!owner)
                {
                    collision.gameObject.GetComponent<Stat_Health>().ChangeHealth(damage * -1);
                }
            }
            else
            {
                if(owner)
                {
                    collision.gameObject.GetComponent<Stat_Health>().ChangeHealth(damage * -1);
                }
            }
            
        }

        // Destroy bullets on impact

        Destroy(this.gameObject);


	}
}
