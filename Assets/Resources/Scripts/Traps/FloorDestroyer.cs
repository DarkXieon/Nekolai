using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class FloorDestroyer : Trap
{
    [SerializeField]
    private float _destroySelfDelay;

    private float _startingYPosition;

    private List<GameObject> _immuneObjects;
    
    public override void Activate()
    {
        var rigidbody2D = this.GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = false;
    }

    private void Start()
    {
        _immuneObjects = new List<GameObject>();

        _startingYPosition = transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;

        if(_startingYPosition == transform.position.y)
        {
            _immuneObjects.Add(otherGameObject);
        }
        else if (other.gameObject.tag == "Terrain" && !_immuneObjects.Contains(otherGameObject))
        {
            Destroy(otherGameObject);
            Destroy(this.gameObject, _destroySelfDelay);
        }
    }
}