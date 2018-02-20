using UnityEngine;
using System.Collections;
using Assets.Contracts;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MoveableObject : MonoBehaviour, IMoveable
{
    // IMoveable property--but properties can't be serialized in unity and therefore can't be edited in the editor
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    //To store the movement of the object before we use it
    protected Vector2 _moveInput;

    [SerializeField] // We want to be able to edit this in the Unity editor
    private float _speed;

    //The GameObject's Rigidbody
    private Rigidbody2D _body;

    // Use this for initialization
    private void Start()
    {
        // In case we forget to set it in the unity editor
        Speed = 5f;
        
        _moveInput = Vector2.zero;

        //Every GameObject the uses this component will have a RigidBody
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //reset the movement from the last input
        _moveInput = Vector2.zero;

        //Get the amount to move the object by
        _moveInput = GetMovement();
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
        _body.MovePosition(_body.position + _moveInput * Speed * Time.fixedDeltaTime);
    }

    //This will be implemented by child classes and will return what direction if any, they will move in
    protected abstract Vector2 GetMovement();
}
