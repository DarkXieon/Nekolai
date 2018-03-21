using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class FloorDestroyer : Trap
{
    private SpriteRenderer _sprite;

    private Collider2D _trapCollider;

    private Transform _trapTransform;

    private bool _activated;

    [SerializeField]
    private bool _destroyTilesStartedIn;

    [SerializeField]
    private float _stopDestroyingDelay;

    private float _startingYPosition;

    private float _timeSinceActivation;

    private List<GameObject> _immuneObjects;
    
    public override void Activate()
    {
        var rigidbody2D = this.GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = false;

        _sprite.enabled = true;

        _trapCollider.enabled = true;

        _activated = true;
    }

    private void Start()
    {
        _sprite = this.GetComponent<SpriteRenderer>();

        if (_sprite == null)
        {
            Debug.LogError("Destroy Floor Trap has no sprite renderer.");
        }
        else
        {
            _sprite.enabled = false;
        }

        _trapCollider = this.GetComponent<Collider2D>();

        _trapTransform = this.transform;

        _trapCollider.enabled = false;

        _activated = false;

        _immuneObjects = new List<GameObject>();

        _startingYPosition = transform.position.y;

        _timeSinceActivation = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;

        if(_startingYPosition == otherGameObject.transform.position.y)
        {
            _immuneObjects.Add(otherGameObject);
        }
        if(other.gameObject.tag == "Terrain" && (!_immuneObjects.Contains(otherGameObject) || _destroyTilesStartedIn))
        {
            Destroy(otherGameObject);

            //Destroy(this.gameObject, _stopDestroyingDelay);
        }
    }

    private void Update()
    {
        if (this._trapTransform.position.y < -100)
        {
            Destroy(this.gameObject);

            return;
        }

        if (_activated && _timeSinceActivation >= _stopDestroyingDelay)
        {
            _trapCollider.enabled = false;
        }
        else if (_activated)
        {
            _timeSinceActivation += Time.deltaTime;
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(_destroyTilesStartedIn && _activated)
        {

        }
    }
    */
}