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

    public bool InAir { get; private set; }
    public bool UsedSecondJump { get; private set; }
    public bool CanJumpTwice { get; set; }

    [SerializeField] private float jumpHeight; // one for testing o-o    
    [SerializeField] private Vector2 inAirDetectorOffset;
    [SerializeField] private float inAirDetectorRadius;
    [SerializeField] private LayerMask mask;
    private Rigidbody2D body;
    
    protected virtual void Start()
    {
        InAir = false;
        UsedSecondJump = false;
        CanJumpTwice = true;

        body = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if(ShouldJump())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = new Collider2D[1];
        int amount = Physics2D.OverlapCircle((Vector2)transform.position - inAirDetectorOffset, inAirDetectorRadius, new ContactFilter2D() { useLayerMask = true, layerMask = mask, useTriggers = false }, hits);

        bool wasInAir = InAir;
        InAir = amount == 0;

        if(wasInAir && !InAir)
        {
            OnVerticalCollision(hits[0]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position - inAirDetectorOffset, inAirDetectorRadius);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.otherCollider is CircleCollider2D)
    //    {
    //        var circleCollider = collision.otherCollider as CircleCollider2D;

    //        ContactPoint2D[] contacts = new ContactPoint2D[1];
    //        int numberOfCollisions = collision.GetContacts(contacts);

    //        if (numberOfCollisions > 0)
    //        {
    //            var collisionPoint = contacts[0].point;

    //            var colliderCenter = circleCollider.bounds.center;
    //            var colliderRadius = circleCollider.radius;

    //            var collisionUnder =
    //                collisionPoint.x < Mathf.Cos((7 * Mathf.PI) / 4) * colliderRadius + colliderCenter.x &&
    //                collisionPoint.x > Mathf.Cos((5 * Mathf.PI) / 4) * colliderRadius + colliderCenter.x &&
    //                collisionPoint.y < colliderCenter.y;

    //            if (collisionUnder)
    //            {
    //                OnVerticalCollision(collision.collider);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("There was no collision point for some reason. . .");
    //        }
    //    }
    //}

    //private void OnCollisonExit2D(Collision2D collider)
    //{
    //    this.InAir = true;
    //}

    public void Jump()
    {
        var force = new Vector2(0, Mathf.Sqrt(jumpHeight * 6 * body.gravityScale * Physics2D.gravity.y * -1));
        var jumpForceReset = new Vector2(body.velocity.x, 0);
        
        if (!this.InAir)
        {
            this.InAir = true;
            
            body.velocity = jumpForceReset;

            body.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);

            Debug.Log(body.velocity.y);
        }
        else if (this.InAir && !this.UsedSecondJump && this.CanJumpTwice)
        {
            this.UsedSecondJump = true;

            body.velocity = jumpForceReset;
            
            body.AddRelativeForce(force, ForceMode2D.Impulse);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);
        }
    }

    protected virtual void OnVerticalCollision(Collider2D collidedWith)
    {
        EventManager.Instance.ExecuteObjectSpecificEvent(EventType.LAND, gameObject);

        InAir = false;
        UsedSecondJump = false;
    }

    protected abstract bool ShouldJump();
}