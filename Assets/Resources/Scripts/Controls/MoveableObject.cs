using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stat_Speed))]
public abstract class MoveableObject : MonoBehaviour, IMoveable
{
    // IMoveable property--but properties can't be serialized in unity and therefore can't be edited in the editor
    
    // ^^ Use the Speed_Stat instead -- left by: Kermit

    //To store the movement of the object before we use it
    protected float _moveInput;

    protected float _previousMoveInput;

    //The GameObject's Rigidbody
    protected Rigidbody2D _body;

    protected Stat_Speed _speedStat;

    private bool _lastMovementWasPositive;

    // Use this for initialization
    private void Start()
    {
        _speedStat = this.GetComponent(typeof(Stat_Speed)) as Stat_Speed;

        _lastMovementWasPositive = false;
        
        _moveInput = 0f;

        _previousMoveInput = 0f;

        //Every GameObject the uses this component will have a RigidBody
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        var movementAmount = GetMovement();
        
        if(movementAmount != 0)
        {
            var movementIsPositive = movementAmount > 0;
            var changedDirectionLeftToRight = movementIsPositive && !_lastMovementWasPositive;
            var changedDirectionRightToLeft = !movementIsPositive && _lastMovementWasPositive;

            if (changedDirectionLeftToRight)
            {
                EventManager.Instance.ExecuteObjectSpecificEvent(EventType.TURN_RIGHT, this.gameObject);
            }
            else if (changedDirectionRightToLeft)
            {
                EventManager.Instance.ExecuteObjectSpecificEvent(EventType.TURN_LEFT, this.gameObject);
            }

            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.WALK, this.gameObject);

            _lastMovementWasPositive = movementIsPositive;
        }
        else
        {
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.NO_MOVEMENT, this.gameObject);
        }

        _moveInput = movementAmount;
        
        /*
        //reset the movement from the last input
        //_moveInput = Vector2.zero;

        //Get the amount to move the object by
        var axis = GetMovement();
        
        _body.velocity = new Vector2(Mathf.Max(0, _body.velocity.x - _moveInput), _body.velocity.y);

        _moveInput = (float)Math.Round(_body.velocity.x + axis * this.GetComponent<Stat_Speed>().GetCurrentValue(), 3);// * Time.fixedDeltaTime;

        var yVeclocity = (float)Math.Round(_body.velocity.y, 3);

        Debug.Log(axis);

        if (_moveInput != 0f)
        {
            _moving = true;

            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.WALK, this.gameObject);
        }
        else if (_moveInput == 0f)*//* && yVeclocity == 0f)*//*
        {
            _moving = false;
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.NO_MOVEMENT, this.gameObject);
        }

        _previousMoveInput = _moveInput; 
        */
    }

    private void FixedUpdate()
    {
        Move();
    }

    /* This method moves the parent GameObject in a direction defined by
     * _moveInput and a speed defined by _speed */
    public void Move()
    {
        _body.velocity = new Vector2(_moveInput, _body.velocity.y);
    }

    //This will be implemented by child classes and will return what direction if any, they will move in
    protected abstract float GetMovement();
    
    public void OnCollisionStay2D(Collision2D collision)
    {
        // We want to carry the force of the moving platform, but the velocity keeps resetting to 0... | TODO
        if(collision.gameObject.GetComponent<PlatformMover>() != null)
        {
            

            Debug.Log("Stop! We'll get 'em next time... | " + Time.time);
            Debug.Log("Velocity BEFORE: " + this.GetComponent<Rigidbody2D>().velocity);
            Debug.Log("Direction Vector" + collision.gameObject.GetComponent<PlatformMover>().direction);
            this.GetComponent<Rigidbody2D>().velocity += collision.gameObject.GetComponent<PlatformMover>().direction;
            Debug.Log("Velocity AFTER: " + this.GetComponent<Rigidbody2D>().velocity);
        }
    }

    

}
