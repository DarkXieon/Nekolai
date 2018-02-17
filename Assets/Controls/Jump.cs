using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D physicsApplyer;
	// Use this for initialization
	void Start ()
    {
        physicsApplyer = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            physicsApplyer.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
    }
}
