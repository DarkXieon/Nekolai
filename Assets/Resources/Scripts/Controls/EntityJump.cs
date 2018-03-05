using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityJump : MonoBehaviour, IJump
{
    [SerializeField] public float JumpHeight { get; private set; }

    public bool InAir { get; private set; }

    public bool UsedSecondJump { get; private set; }

    private Transform _jumperTransform;

    private Rigidbody2D _jumper;
    
    // Use this for initialization
    void Start()
    {
        JumpHeight = 1250;

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
        var force = new Vector2(0, JumpHeight);

        if (!this.InAir)
        {
            this.InAir = true;

            _jumper.AddRelativeForce(force);
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);
        }
        else if (this.InAir && !this.UsedSecondJump)
        {
            this.UsedSecondJump = true;
            
            _jumper.AddRelativeForce(new Vector2(0, force.y));
            
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.JUMP, this.gameObject);
        }
    }
}