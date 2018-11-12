using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Assets.Controls;

[RequireComponent(typeof(AIJump))]
public class AIMovement : MoveableObject
{
    [SerializeField] private float _startDelay;
    [SerializeField] private AIMovementBehaviorType _movementBehavior;

    private WorldPathfinder _pathfinder;
    private AIJump _jumpComponent;
    private Transform _moveTo;
    private float _offset;

    private Vector2 _targetPosition;
    private UnityAction _deathListener;
    private UnityAction _landListener;

    private MultiDirection _currentDirection = MultiDirection.Stationary;
    //private bool _movingRight;
    private bool _approachingJumpPad;
    private bool _approachingFallPad;
    private bool _alive;

    protected override void Start()
    {
        base.Start();

        _moveTo = GameObject.FindGameObjectWithTag("Player").transform;

        _jumpComponent = this.GetComponent<AIJump>();

        _alive = true;

        _deathListener = () => OnDeath();
        _landListener = () => OnLand();

        EventManager.Instance.AddObjectSpecificListener(EventType.DEATH, _deathListener, gameObject);
        EventManager.Instance.AddObjectSpecificListener(EventType.LAND, _landListener, gameObject);

        enabled = false;

        StartCoroutine(BeginCoroutine());
    }

    private IEnumerator BeginCoroutine()
    {
        while (Time.time < _startDelay)
        {
            yield return null;
        }

        enabled = true;

        _pathfinder = new WorldPathfinder(int.MaxValue);

        UpdateTargetPosition();

        _offset = (int)(transform.position.y - _targetPosition.y) + .1f;
    }

    bool passedTargetHorizontally = false;
    bool passedTargetVertically = false;

    private bool ReachedTargetPosition()
    {
        passedTargetHorizontally = (_currentDirection.HasFlag(MultiDirection.Right) && transform.position.x >= _targetPosition.x)
            || (_currentDirection.HasFlag(MultiDirection.Left) && transform.position.x <= _targetPosition.x)
            || (!_currentDirection.HasFlag(MultiDirection.Right) && !_currentDirection.HasFlag(MultiDirection.Left));
        //|| (Mathf.Abs(_targetPosition.x - transform.position.x) < .5f);

        passedTargetVertically = (_currentDirection.HasFlag(MultiDirection.Up) && transform.position.y > _targetPosition.y)
            || (_currentDirection.HasFlag(MultiDirection.Up) && Mathf.Approximately(transform.position.y, _targetPosition.y))
            || (_currentDirection.HasFlag(MultiDirection.Down) && transform.position.y < previousTarget.y)
            || (_currentDirection.HasFlag(MultiDirection.Down) && Mathf.Approximately(transform.position.y, previousTarget.y))
            || (!_currentDirection.HasFlag(MultiDirection.Up) && !_currentDirection.HasFlag(MultiDirection.Down));
        //|| (Mathf.Abs(_targetPosition.y - transform.position.y) < .5f);

        return passedTargetHorizontally && passedTargetVertically;
    }

    private void UpdateTargetPosition()
    {
        bool started = _pathfinder.StartSearch(transform.position, _moveTo.transform.position);

        if (started)
        {
            _pathfinder.SearchStep();

            Node previousNode = null;
            Node nextNode = null;

            if (_pathfinder.path.Count > 1)
            {
                previousNode = _pathfinder.path.Pop();
                nextNode = _pathfinder.path.Pop();
                previousTarget = previousNode.CellBounds.center;
                _targetPosition = nextNode.CellBounds.center;

                UpdateMovementType();
                _approachingJumpPad = previousNode.NodeType == NodeType.JumpBlock && _currentDirection.HasFlag(MultiDirection.Up);
                _approachingFallPad = nextNode.NodeType == NodeType.FallBlock;

                passedTargetHorizontally = false;
                passedTargetVertically = false;

                requiresPathChange = false;
            }
        }
        else
        {
            Debug.LogWarning("Failed to start");
        }
    }

    private void UpdateMovementType()
    {
        _currentDirection = MultiDirection.Stationary;

        bool hasHorizontalMovement = !Mathf.Approximately(_targetPosition.x - transform.position.x, 0f);
        bool hasVerticalMovement = !Mathf.Approximately(_targetPosition.y - previousTarget.y, 0f);

        if (hasHorizontalMovement && _targetPosition.x - transform.position.x > 0f)
        {
            _currentDirection |= MultiDirection.Right;
        }
        else if (hasHorizontalMovement)
        {
            _currentDirection |= MultiDirection.Left;
        }

        if (hasVerticalMovement && _targetPosition.y - previousTarget.y > 0f)
        {
            _currentDirection |= MultiDirection.Up;
        }
        else if (hasVerticalMovement)
        {
            _currentDirection |= MultiDirection.Down;
        }
    }

    //bool waitForCollision = false;
    bool requiresPathChange = false;
    Vector2 previousTarget = Vector2.zero;

    protected override void Update()
    {
        if (ReachedTargetPosition() && !requiresPathChange)
            requiresPathChange = true;

        if (requiresPathChange)
        {
            UpdateTargetPosition();
        }

        Node currentNode = _pathfinder.FindTerrainClosestTo(transform.position);

        if (_approachingJumpPad)//currentNode != null && currentNode.NodeType == NodeType.JumpBlock && currentNode.CellBounds.center.y < _targetPosition.y && !_jumpComponent.InAir && _moveTo.position.y > _targetPosition.y) //  && _currentDirection.HasFlag((MultiDirection)(int)currentNode.JumpMarker.JumpDirection)
        {
            _jumpComponent.RequestJump();
        }

        var yRotation = transform.localRotation.eulerAngles.y;

        if (_currentDirection.HasFlag(MultiDirection.Right) && yRotation != 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_currentDirection.HasFlag(MultiDirection.Left) && yRotation != 180)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        base.Update();
    }

    public override void Move()
    {
        if (_alive)
        {
            base.Move();
        }
    }

    protected override float GetMovement()
    {
        switch (_movementBehavior)
        {
            case AIMovementBehaviorType.ADVANCE_CLOSE:
                return AdvanceClose();
            default:
                return 0;
        }
    }

    private float AdvanceClose()
    {
        bool shouldAdvance = !requiresPathChange && enabled && !passedTargetHorizontally && (!_jumpComponent.InAir || _currentDirection.HasFlag(MultiDirection.Up)) && (_currentDirection.HasFlag(MultiDirection.Right) || _currentDirection.HasFlag(MultiDirection.Left)); //Mathf.Abs(_targetPosition.x - transform.position.x) > stayThreshold; //!ReachedTargetPosition() && enabled;

        if (shouldAdvance)
        {
            //int direction = _movingRight ? 1 : -1;

            int direction = _targetPosition.x - transform.position.x > 0f ? 1 : -1;

            return _speedStat.GetCurrentValue() * direction;
        }
        else if ((_jumpComponent.InAir && _currentDirection.HasFlag(MultiDirection.Down)) || (!_currentDirection.HasFlag(MultiDirection.Right) && !_currentDirection.HasFlag(MultiDirection.Left)) || passedTargetHorizontally)
        {
            _body.velocity = new Vector2(0f, _body.velocity.y);
        }
        //else if(waitForCollision && _jumpComponent.InAir)
        //{
        //    _body.velocity = new Vector2(0f, _body.velocity.y);
        //    _body.position = new Vector3(_targetPosition.x, _body.position.y, transform.position.z);
        //}

        return 0f;
    }

    private void OnLand()
    {
        //waitForCollision = false;
    }

    private void OnDeath()
    {
        _alive = false;
    }
}