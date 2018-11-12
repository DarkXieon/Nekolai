using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stat_Speed))]
public abstract class MoveableObject : MonoBehaviour, IMoveable
{
    // IMoveable property--but properties can't be serialized in unity and therefore can't be edited in the editor
    
    // ^^ Use the Speed_Stat instead -- left by: Kermit

    //To store the movement of the object before we use it
    protected float _moveInput;

    private float _previousMoveInput;

    //The GameObject's Rigidbody
    protected Rigidbody2D _body;

    protected Stat_Speed _speedStat;

    protected bool _canContinue;

    private bool _lastMovementWasPositive;

    private List<Collider2D> _collidersPreventingMovement;

    // Use this for initialization
    protected virtual void Start()
    {
        _collidersPreventingMovement = new List<Collider2D>();

        _speedStat = this.GetComponent(typeof(Stat_Speed)) as Stat_Speed;

        _lastMovementWasPositive = false;
        
        _moveInput = 0f;

        _previousMoveInput = 0f;

        //Every GameObject the uses this component will have a RigidBody
        _body = GetComponent<Rigidbody2D>();

        //Debug.Log(_body.)
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        var movementAmount = GetMovement();
        
        if(movementAmount != 0)
        {
            var movementIsPositive = movementAmount > 0;

            var changedDirectionLeftToRight = _previousMoveInput != 0
                ? movementIsPositive && !_lastMovementWasPositive
                : movementIsPositive;

            var changedDirectionRightToLeft = _previousMoveInput != 0
                ? !movementIsPositive && _lastMovementWasPositive
                : !movementIsPositive;

            if (changedDirectionLeftToRight)
            {
                EventManager.Instance.ExecuteObjectSpecificEvent(EventType.TURN_RIGHT, this.gameObject);

                _canContinue = true;
            }
            else if (changedDirectionRightToLeft)
            {
                EventManager.Instance.ExecuteObjectSpecificEvent(EventType.TURN_LEFT, this.gameObject);
                
                _canContinue = true;
            }

            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.WALK, this.gameObject);
            //Debug.Log(movementAmount);
            _lastMovementWasPositive = movementIsPositive;
        }
        else
        {
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.NO_MOVEMENT, this.gameObject);
        }

        _moveInput = movementAmount;
    }

    protected virtual void FixedUpdate()
    {
        if (_canContinue)
        {
            Move();
        }
    }

    /* This method moves the parent GameObject in a direction defined by
     * _moveInput and a speed defined by _speed */
    public virtual void Move()
    {
        //var previousSpeed = _previousMoveInput;// * 7 * 2;
        var speed = _moveInput;// * 7 * 2;
        //Debug.Log(speed + " " + this.gameObject.name);
        
        _previousMoveInput = _moveInput;
        //Debug.Log(_canContinue);
        _body.AddForce(new Vector2(speed, 0), ForceMode2D.Force);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _collidersPreventingMovement.Remove(collision.otherCollider);

        if (!_collidersPreventingMovement.Any())
        {
            _canContinue = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider is CircleCollider2D)
        {
            var circleCollider = collision.otherCollider as CircleCollider2D;

            ContactPoint2D[] contacts = collision.contacts; // new ContactPoint2D[1];
            int numberOfCollisions = contacts.Length; //.GetContacts(contacts);

            if (numberOfCollisions > 0)
            {
                var collisionPoint = contacts[0].point.y;

                var colliderCenter = circleCollider.bounds.center.y;
                var colliderRadius = circleCollider.radius;

                var collisionHorisontial =
                    collisionPoint < Mathf.Sin(Mathf.PI / 4) * colliderRadius + colliderCenter &&
                    collisionPoint > Mathf.Sin((7 * Mathf.PI) / 4) * colliderRadius + colliderCenter;

                if (collisionHorisontial)
                {
                    _collidersPreventingMovement.Add(collision.otherCollider);

                    _canContinue = false;
                }
                else if (!_collidersPreventingMovement.Any())
                {
                    _canContinue = true;
                }
            }
            else
            {
                Debug.Log("There was no collision point for some reason. . .");
            }
        }
    }
    //This will be implemented by child classes and will return what direction if any, they will move in
    protected abstract float GetMovement();
}
