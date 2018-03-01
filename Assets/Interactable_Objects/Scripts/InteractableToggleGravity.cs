using UnityEngine;
using System.Collections;

public class InteractableToggleGravity : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Rigidbody2D _toToggle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        _toToggle.isKinematic = !_toToggle.isKinematic;
    }
}
