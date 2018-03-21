using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EntityJump : MonoBehaviour, IJump
{
    public float JumpHeight
    {
        get { return this.jumpHeight; }
        set
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
    protected virtual void Start()
    {
        //JumpHeight = 1250;

        InAir = false;

        UsedSecondJump = false;

        CanJumpTwice = true;

        _jumperTransform = this.transform;

        _jumper = this.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    }

    protected virtual void Update()
    {
        if(ShouldJump())
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider is CircleCollider2D)
        {
            var circleCollider = collision.otherCollider as CircleCollider2D;

            ContactPoint2D[] contacts = new ContactPoint2D[1];
            int numberOfCollisions = collision.GetContacts(contacts);

            if (numberOfCollisions > 0)
            {
                var collisionPoint = contacts[0].point;

                var colliderCenter = circleCollider.bounds.center;
                var colliderRadius = circleCollider.radius;

                var collisionUnder =
                    collisionPoint.x < Mathf.Cos((7 * Mathf.PI) / 4) * colliderRadius + colliderCenter.x &&
                    collisionPoint.x > Mathf.Cos((5 * Mathf.PI) / 4) * colliderRadius + colliderCenter.x &&
                    collisionPoint.y < colliderCenter.y;

                if (collisionUnder)
                {
                    OnVerticalCollision(collision.collider);
                }
            }
            else
            {
                Debug.Log("There was no collision point for some reason. . .");
            }
        }
    }

    private void OnCollisonExit2D(Collision2D collider)
    {
        this.InAir = true;
    }
    
    public void Jump()
    {
        var force = new Vector2(0, Mathf.Sqrt(jumpHeight * 6 * _jumper.gravityScale * Physics2D.gravity.y * -1));
        var jumpForceReset = new Vector2(_jumper.velocity.x, 0);
        
        if (!this.InAir)
        {
            this.InAir = true;
            
            _jumper.velocity = jumpForceReset;

            _jumper.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);

            Debug.Log(_jumper.velocity.y);
        }
        else if (this.InAir && !this.UsedSecondJump && this.CanJumpTwice)
        {
            this.UsedSecondJump = true;

            _jumper.velocity = jumpForceReset;
            
            _jumper.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);
        }
    }

    protected virtual void OnVerticalCollision(Collider2D collidedWith)
    {
        EventManager.Instance.ExecuteObjectSpecificEvent(EventType.LAND, this.gameObject);

        this.InAir = false;

        this.UsedSecondJump = false;
        /*
        var edge = new Vector2(collidedWith.bounds.center.x + ((collidedWith.bounds.size.x / 2)), collidedWith.bounds.center.y);

        var direction = new Vector2(1, 0);

        var hit = Physics2D.Raycast(edge, direction, 1f);
        
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            Debug.LogError("None Found");
        }
        */
    }

    protected abstract bool ShouldJump();
}