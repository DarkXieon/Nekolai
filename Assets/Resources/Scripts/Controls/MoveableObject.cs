using UnityEngine;
using System.Collections;

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

    private bool _moving;

    // Use this for initialization
    private void Start()
    {
        _moving = false;

        _moveInput = 0f;

        _previousMoveInput = 0f;

        //Every GameObject the uses this component will have a RigidBody
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //reset the movement from the last input
        //_moveInput = Vector2.zero;

        //Get the amount to move the object by
        _body.velocity = new Vector2(Mathf.Max(0, _body.velocity.x - _moveInput), _body.velocity.y);

        _moveInput = _body.velocity.x + GetMovement() * this.GetComponent<Stat_Speed>().GetCurrentValue();// * Time.fixedDeltaTime;

        if (_moveInput != 0f)
        {
            _moving = true;
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.WALK, this.gameObject);
        }
        else if(_moving)
        {
            _moving = false;
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.NO_MOVEMENT, this.gameObject);
        }

        _previousMoveInput = _moveInput;
    }

    private void FixedUpdate()
    {
        Move();
    }

    /* This method moves the parent GameObject in a direction defined by
     * _moveInput and a speed defined by _speed */
    public void Move()
    {
        //move the RigidBody based on the speed of the fixed update ticks and the input of the controller
        //_body.MovePosition(_body.position + _moveInput * Speed * Time.fixedDeltaTime); -- commented out by: Kermit <-- Use the Speed_Stat

        _body.velocity = new Vector2(_moveInput, _body.velocity.y);

        //_body.AddForce(new Vector2(_moveInput.x * this.GetComponent<Stat_Speed>().GetCurrentValue() * Time.fixedDeltaTime, 0f));

        //_body.MovePosition(_body.position + _moveInput * this.GetComponent<Stat_Speed>().GetCurrentValue() * Time.fixedDeltaTime);
    }

    //This will be implemented by child classes and will return what direction if any, they will move in
    protected abstract float GetMovement();
}
