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
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody2D bullet = Instantiate(BulletPrefab, BarrelEnd.position, transform.rotation);

            bullet.MoveRotation(90);

            var localScale = this.transform.root.localScale.x;
            var barrelEnd = BarrelEnd.right;
            barrelEnd.x *= localScale;

            bullet.AddForce(barrelEnd * BulletForce * Time.deltaTime);
            
        }
    }
}
