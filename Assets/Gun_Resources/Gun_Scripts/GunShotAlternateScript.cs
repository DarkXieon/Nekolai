using UnityEngine;
using System.Collections;

public class GunShotAlternateScript : MonoBehaviour
{
    public Rigidbody2D BulletPrefab;

    public Transform BarrelEnd;
    
    // Enter the Speed of the Bullet from the Component Inspector 
    public float BulletForce = 5f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Rigidbody2D bullet = Instantiate(BulletPrefab, BarrelEnd.position, transform.rotation);

            bullet.MoveRotation(90);
            
            bullet.AddForce(BarrelEnd.right * BulletForce * Time.deltaTime);
        }
    }
}
