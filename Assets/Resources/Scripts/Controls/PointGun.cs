using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PointGun : MonoBehaviour
{
    [SerializeField] //this allows it to be seen in the inspector
    private Transform _rotateAround; //this is the object that the gun will ultamatly make it's transformations around
                                     //and will use this to calculate the the rotation of the gun

    [SerializeField] //this allows it to be seen in the inspector
    private Transform _pointWith; //this is the point of the gun that the bullets exit from

    private Transform _gun;
    private Vector2 _pointWithOffset;
    private Vector2 _gunStartingPosition;
    private float _distanceFromBody;
    
    private bool _fliped; //indicates if the player is facing backwards
    
    private void Start()
    {
        _gun = _pointWith.parent;
        _gunStartingPosition = _gun.position;
        _pointWithOffset = _gun.position - _rotateAround.position;
        _distanceFromBody = Vector2.Distance(_rotateAround.position, _gunStartingPosition);
        
        _fliped = false; //check the rotation of the gun, this should return false

        Debug.Assert(this.transform.rotation.eulerAngles == Quaternion.identity.eulerAngles, "The gun does not have an initial rotation of 0. Please set the gun's initial rotation to zero.");
    }

    private void Update()
    {
        Vector2 cursorCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 relativeCoordinates = cursorCoordinates - (Vector2)transform.position;

        if ((!_fliped && relativeCoordinates.x < 0f) || (_fliped && relativeCoordinates.x > 0f))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _gun.GetComponent<SpriteRenderer>().flipX = !_gun.GetComponent<SpriteRenderer>().flipX;
            _gun.GetComponent<SpriteRenderer>().flipY = !_gun.GetComponent<SpriteRenderer>().flipY;
            _fliped = !_fliped;
        }

        relativeCoordinates = cursorCoordinates - (Vector2)_rotateAround.position;

        float angle = relativeCoordinates.y > 0f
            ? Vector2.Angle(Vector2.right, relativeCoordinates.normalized)
            : -Vector2.Angle(Vector2.right, relativeCoordinates.normalized);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        _gun.localEulerAngles = _fliped
            ? -rotation.eulerAngles
            : rotation.eulerAngles;
        _gun.position = rotation * _pointWithOffset + _rotateAround.position;
    }
}