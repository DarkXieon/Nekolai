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
    
    private float _x0; //the spot on the x axis where y = 0 and _x0 and the current x location are equadistant to _rotateAround
    private float _yOffset;
    private bool _fliped; //indicates if the player is facing backwards

    // Use this for initialization
    void Start()
    {
        //this gets the x value where _rotateAround is the origin and _x0 is a x value 
        //where (_x0, 0) and it's current location are equidistant to the origin
        this._x0 = _pointWith.position.x - _rotateAround.position.x;//this.GetDistanceFromOrigin(_pointWith.position - _rotateAround.position);
        this._yOffset = _pointWith.position.y - _rotateAround.position.y;

        this._fliped = false; //check the rotation of the gun, this should return false

        Debug.Assert(this.transform.rotation.eulerAngles == Quaternion.identity.eulerAngles, "The gun does not have an initial rotation of 0. Please set the gun's initial rotation to zero.");
    }

    // Update is called once per frame
    void Update()
    {
        var cursorCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (this.ShouldFlip(cursorCoordinates))
        {
            this.Flip();
        }

        var angle = this.GetRotationAngleRadian(cursorCoordinates);

        var endPosition = this.GetTranslatedPosition(angle);

        var actualAngle = this.GetActualRotationAngle(angle);

        this._rotateAround.localRotation = _fliped
            ? Quaternion.Euler(0, 0, -actualAngle)
            : Quaternion.Euler(0, 0, actualAngle);
    }
    
    /*
    private float GetDistanceFromOrigin(Vector2 currentPosition)
    {
        var slope = currentPosition.y / currentPosition.x;

        var origin = currentPosition.x * Mathf.Cos(Mathf.Atan(slope)) - currentPosition.y * Mathf.Sin(Mathf.Atan(slope));

        return origin;
    }
    */
    private float GetRotationAngleRadian(Vector3 cursorCoordinates)
    {
        var angle = Mathf.Atan((cursorCoordinates.y - (this._rotateAround.position.y + this._yOffset)) / (cursorCoordinates.x - this._rotateAround.position.x));

        return angle;
    }

    private float GetActualRotationAngle(float radianAngle)
    {
        float degreeAngle = radianAngle * Mathf.Rad2Deg;

        return degreeAngle;

        /*
        float currentRotation = _gunPointerTransform.rotation.eulerAngles.z;
        float degreeAngle = _fliped
            ? radianAngle * Mathf.Rad2Deg
            : radianAngle * Mathf.Rad2Deg;
        
        float actualRotationAngle = _fliped
            ? 180 - degreeAngle - currentRotation
            : degreeAngle - currentRotation;
        return actualRotationAngle;
        */
    }

    private Vector3 GetTranslatedPosition(float angle)
    {
        var x1 = this._x0 * Mathf.Cos(angle) + this._rotateAround.position.x;
        var y1 = this._x0 * Mathf.Sin(angle) + (this._rotateAround.position.y + this._yOffset);

        return new Vector3(x1, y1, 0);
        /*
        //calculates the new x value for the gun and modifies the equation based on if the gun is fliped
        var x1 = _fliped
            ? this._x0 * -1 * Mathf.Cos(angle) + this._rotateAround.position.x
            : this._x0 * Mathf.Cos(angle) + this._rotateAround.position.x;

        //calculates the new y value for the gun and modifies the equation based on if the gun is fliped
        var y1 = _fliped
            ? this._x0 * -1 * Mathf.Sin(angle) + this._rotateAround.position.y
            : this._x0 * Mathf.Sin(angle) + this._rotateAround.position.y;

        return new Vector3(x1, y1, 0);
        */
    }
    /*
    private Vector3 GetRotationAxis()
    {
        return _fliped
            ? Vector3.back
            : Vector3.forward;
    }
    */
    //This determines if the player and gun should face the other direction
    private bool ShouldFlip(Vector3 cursorCoordinates)
    {
        bool cursorOnRight = cursorCoordinates.x >= this.transform.root.position.x;
        bool shouldFlip = (cursorOnRight && _fliped) || (!cursorOnRight && !_fliped);

        return shouldFlip;
        /*
        //indicates if it was fliped over the y axis
        bool isParentFliped = this._rotateAround.rotation.eulerAngles.y == 180 || this._rotateAround.rotation.eulerAngles.y == -180;
        bool isCursorRightOfParent = cursorCoordinates.x >= this._rotateAround.position.x;

        //make sure the values are what we expect, both the gun and parent should ALWAYS be rotated at the same time
        Debug.Assert((_fliped && isParentFliped) || (!_fliped && !isParentFliped), "The gun or player was incorrectly flipped, likely the gun since it is the child object");

        return (!_fliped && !isParentFliped && !isCursorRightOfParent) || (_fliped && isParentFliped && isCursorRightOfParent);
        */
    }
    private void Flip()
    {
        //var rootScale = this.this.transform.root.localScale;

        this.transform.root.localRotation *= Quaternion.Euler(0, 180, 0);

        //rootScale.x *= -1;
        this._x0 *= -1;
        this._fliped = !this._fliped;

        //this.this.transform.root.localScale = rootScale;


        /*
        this._rotateAround.rotation = _fliped
            ? Quaternion.Euler(new Vector3(0, 0, 0))
            : Quaternion.Euler(new Vector3(0, 180, 0));

        _fliped = !_fliped;
        */
    }
}