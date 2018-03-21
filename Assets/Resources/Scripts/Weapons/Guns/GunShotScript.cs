using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunShotScript : MonoBehaviour {

	// Drag in the Bullet Spawner from the Component Inspector to tell where the bullets will spawn from (tip of the gun)
	public Transform BulletSpawnPoint;

	// Drag in the Bullet Prefab from the Component Inspector
	public GameObject Bullet;

	[HideInInspector]
	public float BulletForce = .02f;

	[HideInInspector]
	public AudioSource GunShotSound;

	// Need to know if we are already shooting
	private bool shooting = false;

	// Timer to delay the time between shots
	private float bullet_Delay = .25f;
	private float bullet_Timer = 0f;

	// This will tell the bullet to go left or right depending on which way the sprite is facing
	private Transform positionOfParent;
	private float OldPosition;

    //private Stat_Power power; // used to determine damage 


	//IMPORTANT enemies will need triggers to check if a bullet hit them!!! We can use tags to check if it was a bullet or another enemy


	// Use this for initialization
	void Awake()
	{
		positionOfParent = BulletSpawnPoint.parent;

        GunShotSound = this.GetComponent(typeof(AudioSource)) as AudioSource;
       // power = this.GetComponent<Stat_Power>();
    }

	// Update is called once per frame
	void Update ()
	{
		OldPosition = positionOfParent.transform.position.x;

		if (Input.GetMouseButtonDown(0) && !shooting)
		{
			// Set shooting to true
			shooting = true;

			Debug.Assert (GunShotSound != null, "That weapon has no soundbite attached");
			GunShotSound.Play();


			// The Bullet instantiation, to make a copy whenever firing
			GameObject Bullet_Handler;
			Bullet_Handler = Instantiate (Bullet, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation);

         

			// Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original, can be corrected here
			Bullet_Handler.transform.Rotate(Vector3.forward * 90);

			// Retrieve the Rigidbody2D component from the instantiated Bullet and control it.
			Rigidbody2D RigidBody;
			RigidBody = Bullet_Handler.GetComponent<Rigidbody2D>();

			// If the old position is greater or equal to the current, the bullet will be going right
			if (OldPosition >= positionOfParent.transform.position.x) 
			{
				// Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
				RigidBody.AddForce(transform.right * BulletForce);
			}
			// Else we have to send it left because we are facing the opposite direction
			else 
				RigidBody.AddForce(transform.right * -(BulletForce));

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
