using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour, IWeapons {

	// Drag in the Bullet Spawner from the Component Inspector to tell where the bullets will spawn from (tip of the gun)
	public Transform barrelEnd;

	// Drag in the Bullet Prefab from the Component Inspector
	public Rigidbody2D bulletPrefab;

	// Enter the Speed of the Bullet from the Component Inspector
	public float bullet_Force = .02f;

	// Need to know if we are already shooting
	private bool shooting = false;

	// Timer to delay the time between shots
	private float bullet_Delay = .25f;
	private float bullet_Timer = 0f;

	private string _name = "Gun";
	private int _power = 2;

	public string WeaponName {
		get { return _name; }
		set { _name = value; }

	}

	public int Power {
		get { return _power; }
		set { _power = value; }
	}


	public void Attack()
	{
		// Set shooting to true
		shooting = true;
		Rigidbody2D bullet = Instantiate (bulletPrefab, barrelEnd.transform.position, barrelEnd.rotation);
		// Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original, can be corrected here
		bullet.MoveRotation(90);
		// Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
		bullet.AddForce(barrelEnd.up * bullet_Force * Time.deltaTime);
		Destroy (bullet.gameObject, 2f);
	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetMouseButtonDown(0) && !shooting)
		{
			Attack ();
		}
		if (shooting)
		{
			// If already shooting start timer until it reaches proper delay time
			bullet_Timer += Time.deltaTime;
			if (bullet_Timer >= bullet_Delay)
			{
				// Reset timer and set shooting to false to start the process over and allow more shooting
				bullet_Timer = 0f;
				shooting = false;
			}

		}
	}
}
