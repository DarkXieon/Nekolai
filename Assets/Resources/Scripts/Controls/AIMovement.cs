using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(AIJump))]
public class AIMovement : MoveableObject
{
    [SerializeField]
    private int _aiUpdateDelay;

    [SerializeField]
    private AIMovementBehaviorType _movementBehavior;
    
    private AIJump _jumpComponent;
    
    private Transform _moverTransform;
    
    private Transform _moveTo;

    private Vector2 _movingTwordsCurrently;

    private Vector2 _actualMovingTwords;

    private Path _path;

    private UnityAction _landListener;

    private UnityAction _deathListener;

    private int _delayCounter;

    private bool _canRequestJump;

    private bool _alive;

    protected override void Start()
    {
        base.Start();

        _moveTo = GameObject.FindGameObjectWithTag("Player").transform;

        _jumpComponent = this.GetComponent<AIJump>();

        _moverTransform = _body.transform;

        _canRequestJump = true;

        _alive = true;

        _movingTwordsCurrently = _moveTo.position;

        _delayCounter = 0;

        _path = new Path(_moverTransform.position, _movingTwordsCurrently);

        _actualMovingTwords = _path.CurrentTarget();

        //Debug.LogError(_path.CurrentTarget());

        _landListener = () => CanRequestJump();

        _deathListener = () => OnDeath();

        EventManager.Instance.AddObjectSpecificListener(EventType.DEATH, _deathListener, _moverTransform.gameObject);
    }

    protected override void Update()
    {
        if (_jumpComponent.StandingOn != null)
        {
            if(_delayCounter >= _aiUpdateDelay && _canRequestJump)
            {
                _movingTwordsCurrently = _moveTo.position;

                _path.UpdatePath(this._moverTransform.position, _movingTwordsCurrently);

                _actualMovingTwords = _path.CurrentTarget();

                _delayCounter = 0;
            }
            else
            {
                _delayCounter++;
            }

            var yRotation = _moverTransform.localRotation.eulerAngles.y;

            var relativeDistance = _moverTransform.position.x - _moveTo.position.x;

            if (relativeDistance < 0 && yRotation != 0)
            {
                _moverTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (relativeDistance > 0 && yRotation != 180)
            {
                _moverTransform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            base.Update();
        }
    }

    public override void Move()
    {
        //var relativePosition = Mathf.Round(_moveTo.position.x - _moverTransform.position.x);

        if (_alive)// && relativePosition != 0)
        {
            base.Move();
        }

        var shouldAdvance = Mathf.Round(_path.CurrentTarget().x - _moverTransform.position.x) == 0 && Mathf.Round(_path.CurrentTarget().y - _moverTransform.position.y) == 0;

        Debug.Log(_path.CurrentTarget().y);
        Debug.Log(_moverTransform.position.y);

        if(shouldAdvance)
        {
            //Debug.LogError("Advanced");

            _actualMovingTwords = _path.AdvancePath();
        }
    }

    protected override float GetMovement()
    {
        switch(_movementBehavior)
        {
            case AIMovementBehaviorType.ADVANCE_CLOSE:
                return AdvanceClose();
            default:
                return 0;
        }
    }
    
    private bool TryGetDistanceToLedge(out float distance)
    {
        if(_jumpComponent.LedgeAt.HasValue)
        {
            var positionOfLedge = _jumpComponent.LedgeAt.Value;

            distance = Mathf.Abs(_moverTransform.position.x - positionOfLedge);

            return true;
        }
        else
        {
            distance = 0;

            return false;
        }
        /*
        var direction = new Vector2(1, 1).normalized;

        var rayDistance = Mathf.Sqrt(2 * Mathf.Pow(this._height, 2));

        var rayHit = Physics2D.Raycast(_moverTransform.position, direction, rayDistance);
        //rayHit.collider.
        if(rayHit.collider != null)
        {
            distance = rayHit.distance;

            return true;
        }
        else
        {
            distance = 0;

            return false;
        }
        */
    }
    /*
    private float GetHeight()
    {
        var rayHit = Physics2D.Raycast(_moverTransform.position, Vector2.down);

        return rayHit.distance;
    }
    */
    private float AdvanceClose()
    {
        /*
        Vector2 currentMovement = _movingTwordsCurrently;

        if(_actualMovingTwords != new Vector2())
        {
            currentMovement = _actualMovingTwords;
        }
        else
        {
            var relativeY = _moverTransform.position.y - _movingTwordsCurrently.y;

            var relativeX = Mathf.Round(_movingTwordsCurrently.x - _moverTransform.position.x);

            if (relativeX == 0 && relativeY > 1 && _actualMovingTwords == new Vector2())
            {
                var test = ((Vector2)_moverTransform.position).GetClosestPlatformEdge(_movingTwordsCurrently);

                if (test.HasValue)
                {
                    _actualMovingTwords = test.Value;

                    return this.AdvanceClose();
                }
            }
        }
        */


        //Debug.Log(_jumpComponent.NextCollider != null);
        if (_jumpComponent.NextCollider == null && _canRequestJump)
        {
            _jumpComponent.RequestJump();

            _canRequestJump = false;

            EventManager.Instance.AddObjectSpecificListener(EventType.LAND, _landListener, this.gameObject);
        }
        
        var direction = _path.CurrentTarget().x > _moverTransform.position.x
            ? 1
            : -1;
        //Debug.Log(direction);
        //Debug.Log(_path.CurrentTarget().x);
        //Debug.LogError(_moverTransform.position.x);
        if (Mathf.Round(_path.CurrentTarget().x - _moverTransform.position.x) == 0)
        {
            return 0;
        }

        return _speedStat.GetCurrentValue() * direction;
    }

    private void CanRequestJump()
    {
        _canRequestJump = true;

        EventManager.Instance.RemoveObjectSpecificListener(EventType.LAND, _landListener, this.gameObject);
    }

    private void OnDeath()
    {
        _alive = false;
    }
}