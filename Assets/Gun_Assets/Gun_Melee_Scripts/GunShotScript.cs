using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotScript : MonoBehaviour {

	// Drag in the Bullet Spawner from the Component Inspector to tell where the bullets will spawn from (tip of the gun)
	public GameObject Bullet_Spawn_Point;

	// Drag in the Bullet Prefab from the Component Inspector
	public GameObject Bullet;

	// Enter the Speed of the Bullet from the Component Inspector 
	public float Bullet_Force = .02f;

	// Need to know if we are already shooting
	private bool shooting = false;

	// Timer to delay the time between shots
	private float bullet_Delay = .25f;
	private float bullet_Timer = 0f;

	// This will tell the bullet to go left or right depending on which way the sprite is facing
	private Rigidbody2D positionOfParent;
	private float OldPosition;


	//IMPORTANT enemies will need triggers to check if a bullet hit them!!! We can use tags to check if it was a bullet or another enemy


	// Use this for initialization
	void Start ()
	{
		positionOfParent = Bullet_Spawn_Point.GetComponentInParent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update ()
	{
		OldPosition = positionOfParent.transform.position.x;

		if (Input.GetKeyDown("space") && !shooting)
		{
			// Set shooting to true
			shooting = true;

			// The Bullet instantiation, to make a copy whenever firing
			GameObject Bullet_Handler;
			Bullet_Handler = Instantiate (Bullet, Bullet_Spawn_Point.transform.position, Bullet_Spawn_Point.transform.rotation);

			// Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original, can be corrected here
			Bullet_Handler.transform.Rotate(Vector3.forward * 90);

			// Retrieve the Rigidbody2D component from the instantiated Bullet and control it.
			Rigidbody2D RigidBody;
			RigidBody = Bullet_Handler.GetComponent<Rigidbody2D>();

			// If the old position is greater or equal to the current, the bullet will be going right
			if (OldPosition >= positionOfParent.transform.position.x) 
			{
				// Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
				RigidBody.AddForce(transform.right * Bullet_Force);
			}
			// Else we have to send it left because we are facing the opposite direction
			else 
				RigidBody.AddForce(transform.right * -(Bullet_Force));

			// Set the Bullets to self destruct after 3 Seconds
			Destroy(Bullet_Handler, 3.0f);
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
