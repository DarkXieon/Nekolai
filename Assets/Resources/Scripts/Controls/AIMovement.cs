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

    private Node _currentNode;
    private Node _nextNode;

    private Vector2 _previousTarget;
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
            || (_currentDirection.HasFlag(MultiDirection.Down) && transform.position.y < _previousTarget.y)
            || (_currentDirection.HasFlag(MultiDirection.Down) && Mathf.Approximately(transform.position.y, _previousTarget.y))
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
            
            if (_pathfinder.path.Count > 1)
            {
                _currentNode = _pathfinder.path.Pop();

                Debug.Assert(_pathfinder.FindTerrainClosestTo(transform.position).Index == _currentNode.Index);

                _nextNode = _pathfinder.path.Pop();
                
                _previousTarget = _currentNode.CellBounds.center;
                _targetPosition = _nextNode.CellBounds.center;

                UpdateMovementType();

                _approachingJumpPad = _currentNode.NodeType == NodeType.JumpBlock && _currentDirection.HasFlag(MultiDirection.Up);
                _approachingFallPad = _nextNode.NodeType == NodeType.FallBlock;

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
        
        if (_currentNode.Column < _nextNode.Column)
        {
            _currentDirection |= MultiDirection.Right;
        }
        else if (_currentNode.Column > _nextNode.Column)
        {
            _currentDirection |= MultiDirection.Left;
        }

        if (_currentNode.Row > _nextNode.Row) //row numbering starts from the top
        {
            _currentDirection |= MultiDirection.Up;
        }
        else if (_currentNode.Row < _nextNode.Row)
        {
            _currentDirection |= MultiDirection.Down;
        }
    }

    //bool waitForCollision = false;
    bool requiresPathChange = false;

    protected override void Update()
    {
        if (ReachedTargetPosition() && !requiresPathChange)
            requiresPathChange = true;

        if (requiresPathChange)
        {
            UpdateTargetPosition();
        }

        Node currentNode = _pathfinder.FindTerrainClosestTo(transform.position);

        if (_currentDirection.HasFlag(MultiDirection.Up) && _currentNode.NodeType == NodeType.JumpBlock)//_approachingJumpPad)//currentNode != null && currentNode.NodeType == NodeType.JumpBlock && currentNode.CellBounds.center.y < _targetPosition.y && !_jumpComponent.InAir && _moveTo.position.y > _targetPosition.y) //  && _currentDirection.HasFlag((MultiDirection)(int)currentNode.JumpMarker.JumpDirection)
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
            int direction = 0;

            if (_currentDirection.HasFlag(MultiDirection.Right))
                direction = 1;
            else if (_currentDirection.HasFlag(MultiDirection.Left))
                direction = -1;
                
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