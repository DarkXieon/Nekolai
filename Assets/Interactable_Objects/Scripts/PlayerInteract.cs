using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    // The object we can currently interact with.
    public GameObject currentInterObj = null;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && currentInterObj)
        {
            // Do something with the object
            currentInterObj.SendMessage("DoInteraction");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When you enter the trigger object, save the object you're
        // interacting with as a GameObject
        if (other.CompareTag("interObject"))
        {
            Debug.Log(other.name);
            currentInterObj = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When you leave the trigger object, remove the GameObject you
        // can interact with
        if (other.CompareTag("interObject"))
        {
            if (other.gameObject == currentInterObj)
            {
                currentInterObj = null;
            }
        }
    }
}
