using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AIJump : EntityJump
{
    public Collider2D StandingOn { get; private set; }

    public Collider2D NextCollider { get; private set; }
    
    public float? LedgeAt { get; private set; }

    public bool? AnyPlatformsInRange { get; private set; }
    
    private ColliderDirection _currentDirection;

    private bool _requestingJump;

    private UnityAction _walkListener;
    
    // Use this for initialization
    protected override void Start()
    {
        _requestingJump = false;

        EventManager.Instance.AddObjectSpecificListener(EventType.TURN_LEFT, () => this.TurnLeft(), this.gameObject);
        EventManager.Instance.AddObjectSpecificListener(EventType.TURN_RIGHT, () => this.TurnRight(), this.gameObject);

        _walkListener = () => CheckForJump();
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override bool ShouldJump()
    {
        var willJump = _requestingJump;

        _requestingJump = false;

        return willJump;
    }

    protected override void OnVerticalCollision(Collider2D collidedWith)
    {
        Debug.Log("Collided");

        base.OnVerticalCollision(collidedWith);
        
        StandingOn = collidedWith;

        if(_currentDirection == default(ColliderDirection))
        {
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.TURN_RIGHT, this.gameObject);
        }
        else
        {
            SetNextCollider();
        }
    }
    
    public void RequestJump()
    {
        _requestingJump = true;

        if(!LedgeAt.HasValue)
        {
            var edge = new Vector2(StandingOn.bounds.center.x + ((int)_currentDirection * (StandingOn.bounds.size.x / 2)), StandingOn.bounds.center.y);

            LedgeAt = edge.x;

            EventManager.Instance.AddObjectSpecificListener(EventType.WALK, _walkListener, this.gameObject);
        }
    }

    private void SetNextCollider()
    {
        var edge = new Vector2(StandingOn.bounds.center.x + ((int)_currentDirection * (StandingOn.bounds.size.x / 2)), StandingOn.bounds.center.y);

        var direction = new Vector2((int)_currentDirection, 0);

        var hit = Physics2D.Raycast(edge, direction, 1f);
        
        /*
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            Debug.LogError("None Found");
        }
        */

        NextCollider = hit.collider;
        
        if(hit.collider == null)
        {
            LedgeAt = edge.x;
        }
        else
        {
            LedgeAt = null;
        }

        CalculateIfGapJumpable();

        //Debug.Log(NextCollider != null);
    }

    private void CalculateIfGapJumpable()
    {
        Debug.Log("Not in use right now");
        Debug.Log("Change this code");

        if(LedgeAt.HasValue)
        {
            var rayOrigin = new Vector2(LedgeAt.Value, StandingOn.bounds.center.y);

            var direction = new Vector2((int)_currentDirection, 0);

            var rayhit = Physics2D.Raycast(rayOrigin, direction, 3 * 7); //3 will be replaced with a jump distance variable along with possibly the 7

            AnyPlatformsInRange = rayhit.collider != null;
        }
        else
        {
            AnyPlatformsInRange = null;
        }
    }
    
    private void TurnRight()
    {
        Debug.Log("Turned Right");

        _currentDirection = ColliderDirection.RIGHT;

        SetNextCollider();
    }

    private void TurnLeft()
    {
        Debug.Log("Turned Left");

        _currentDirection = ColliderDirection.LEFT;

        SetNextCollider();
    }

    private void CheckForJump()
    {
        var jumpAt = LedgeAt.Value - 1 * (int)_currentDirection;

        if(_currentDirection == ColliderDirection.RIGHT && this.transform.position.x >= jumpAt)
        {
            Jump();

            _requestingJump = false;

            EventManager.Instance.RemoveObjectSpecificListener(EventType.WALK, _walkListener, this.gameObject);
        }
        else if(_currentDirection == ColliderDirection.LEFT && this.transform.position.x <= jumpAt)
        {
            Jump();

            _requestingJump = false;

            EventManager.Instance.RemoveObjectSpecificListener(EventType.WALK, _walkListener, this.gameObject);
        }
    }

    private enum ColliderDirection
    {
        RIGHT = 1,
        LEFT = -1
    }
}
