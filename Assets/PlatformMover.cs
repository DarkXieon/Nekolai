using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{

    //declare the variables

    Transform myTransform; //The Transform of this object(Movements)
    Vector3 startLocation; // Starting point of object
    [SerializeField] public Vector2 direction; //The path its going
    [SerializeField] public Vector2 stopLocation; // Stopping point of object
    [SerializeField] public int platformType; // 0 = Horizontal, 1 = Vertical, 2 = Circular, 3 = Freeform


    // Use this for initialization
    void Start()
    {

        myTransform = this.transform;
        startLocation = myTransform.position; // Start location
                                           //Set stop position in unity editor


    }

    // Update is called once per frame
    //Change by Andrew: if you are using Time.deltaTime, this should be fixed update so the speed doesn't change
    void FixedUpdate()
    {
        // Logic for horizontal moving platforms
        if (this.platformType == 0)
        {
            //Move the Platform in a direction
            myTransform.Translate(direction * Time.deltaTime);
            //Reverse direction when the Platform has reached the stopping point
            if (myTransform.position.x >= stopLocation.x)
            {
                Debug.Log("No: " + Time.time);
                direction *= -1;  //Reverse Direction

                /*if (direction == direction * -1)
                {
                   // stopLocation *= -1;
                    //direction *= -1; 
                }*/

            }
            else if (myTransform.position.x <= startLocation.x)
            {
                Debug.Log("Yes: " + Time.time);
                direction *= -1;  //Reverse Direction
            }

        }
        else  // Logic for vertical moving platforms
        if (this.platformType == 1)
        {
            //Move the Platform in a direction
            myTransform.Translate(direction * Time.deltaTime);
            //Reverse direction when the Platform has reached the stopping point
            if (myTransform.position.y >= stopLocation.y)
            {
                Debug.Log("No: " + Time.time);
                direction *= -1;  //Reverse Direction

                /*if (direction == direction * -1)
                {
                   // stopLocation *= -1;
                    //direction *= -1; 
                }*/

            }
            else if (myTransform.position.y <= startLocation.y)
            {
                Debug.Log("Yes: " + Time.time);
                direction *= -1;  //Reverse Direction
            }

        }
        







    }
}

