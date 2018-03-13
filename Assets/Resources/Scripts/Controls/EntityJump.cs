using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityJump : MonoBehaviour, IJump
{
    public float JumpHeight
    {
        get { return this.jumpHeight; }
        private set
        {
            if (value < 0)
            {
                Debug.LogError("Jump height cannot be negative!");
            }
            else
            {
                this.jumpHeight = value;
            }
        }
    }

    [SerializeField] private float jumpHeight; // one for testing o-o
    
    public bool InAir { get; private set; }

    public bool UsedSecondJump { get; private set; }

    public bool CanJumpTwice { get; set; }

    private Transform _jumperTransform;

    private Rigidbody2D _jumper;
    
    // Use this for initialization
    void Start()
    {
        //JumpHeight = 1250;

        InAir = false;

        UsedSecondJump = false;

        _jumperTransform = this.transform;

        _jumper = this.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w") && (!this.InAir || !this.UsedSecondJump))
        {
            Jump();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collider)
    {
        var colliderPosition = collider.gameObject.transform.position;

        if(colliderPosition.y <= _jumperTransform.position.y + 1)
        {
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.LAND, this.gameObject);
            
            this.InAir = false;

            this.UsedSecondJump = false;
        }
    }

    void OnCollisonExit2D(Collision2D collider)
    {
        this.InAir = true;
    }
    
    public void Jump()
    {
        var force = new Vector2(0, Mathf.Sqrt(jumpHeight * 6 * _jumper.gravityScale * Physics2D.gravity.y * -1));//jumpHeight/*JumpHeight... for testing; we couldn't set JumpHeight in the editor.*/);
        var jumpForceReset = new Vector2(_jumper.velocity.x, 0);
        
        if (!this.InAir)
        {
            this.InAir = true;
            
            _jumper.velocity = jumpForceReset;

            _jumper.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);

            Debug.Log(_jumper.velocity.y);
        }
        else if (this.InAir && !this.UsedSecondJump)
        {
            this.UsedSecondJump = true;

            _jumper.velocity = jumpForceReset;
            
            _jumper.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        Debug.Log(_jumper.velocity.y);

        /*
        var force = new Vector2(0, JumpHeight * Physics2D.gravity.y * Time.fixedDeltaTime);

        if(this.InAir && _currentJumpTime < _jumpTime)
        {
            _jumper.AddForce(force);

            _currentJumpTime += Time.fixedDeltaTime;
        }
        */
    }
}