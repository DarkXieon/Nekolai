using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    // The object we can currently interact with.
    private IInteractable _currentInterObj = null;

    void Update()
    {
        if (Input.GetKeyDown("e") && _currentInterObj != null)
        {
            // Do something with the object
            _currentInterObj.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When you enter the trigger object, save the object you're
        // interacting with as a GameObject
        var foundInteractable = other.gameObject.GetComponent(typeof(IInteractable)) as IInteractable;

        if (foundInteractable != null)
        {
            _currentInterObj = foundInteractable;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When you leave the trigger object, remove the GameObject you
        // can interact with
        var foundInteractable = other.gameObject.GetComponent(typeof(IInteractable)) as IInteractable;

        if (foundInteractable != null)
        {
            if (foundInteractable == _currentInterObj)
            {
                _currentInterObj = null;

                Debug.Log("Interaction terminated");
            }
        }
    }
}
