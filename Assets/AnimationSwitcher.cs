using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class AnimationSwitcher : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    //[SerializeField]
    protected AnimatorOverrideController _controller;

    [SerializeField]
    private string _jumpTrigger;

    [SerializeField]
    private string _walkTrigger;

    [SerializeField]
    private string _noMovementTrigger;

    private UnityAction _jumpAction;

    private UnityAction _landAction;

    private UnityAction _walkAction;

    private UnityAction _noMovementAction;

    private void Start()
    {
        //_animator = this.GetComponent<Animator>();
        var parameters = _animator.parameters.ToList();

        var state = parameters.First(param => string.Equals(param.name, "State", StringComparison.OrdinalIgnoreCase));
        var inAir = parameters.First(param => string.Equals(param.name, "In Air", StringComparison.OrdinalIgnoreCase));
        //var walkParam = parameters.First(param => string.Equals(param.name, "Walk", StringComparison.OrdinalIgnoreCase));
        //var noMovementParam = parameters.First(param => string.Equals(param.name, "No Movement", StringComparison.OrdinalIgnoreCase));

        //Debug.Assert(_animator != null, "The chosen gameobject has no animator");

        _jumpAction = () =>
        {
            _animator.SetInteger(state.name, 1);
            _animator.SetBool(inAir.name, true);
        };
        EventManager.Instance.AddObjectSpecificListener(EventType.JUMP, _jumpAction, this.gameObject);

        _landAction = () =>
        {
            _animator.SetInteger(state.name, 2);
            _animator.SetBool(inAir.name, false);
        };
        EventManager.Instance.AddObjectSpecificListener(EventType.LAND, _landAction, this.gameObject);

        _walkAction = () => _animator.SetInteger(state.name, 3);
        EventManager.Instance.AddObjectSpecificListener(EventType.WALK, _walkAction, this.gameObject);

        _noMovementAction = () => _animator.SetInteger(state.name, 4);
        EventManager.Instance.AddObjectSpecificListener(EventType.NO_MOVEMENT, _noMovementAction, this.gameObject);
        /*
        _jumpAction = () => _animator.SetTrigger(_jumpTrigger);
        _walkAction = () => _animator.SetTrigger(_walkTrigger);
        _noMovementAction = () => _animator.SetTrigger(_noMovementTrigger);

        EventManager.Instance.AddObjectSpecificListener(EventType.JUMP, _jumpAction, this.gameObject);
        EventManager.Instance.AddObjectSpecificListener(EventType.WALK, _walkAction, this.gameObject);
        EventManager.Instance.AddObjectSpecificListener(EventType.NO_MOVEMENT, _noMovementAction, this.gameObject); */
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveObjectSpecificListener(EventType.JUMP, _jumpAction, this.gameObject);
        EventManager.Instance.RemoveObjectSpecificListener(EventType.WALK, _walkAction, this.gameObject);
        EventManager.Instance.RemoveObjectSpecificListener(EventType.NO_MOVEMENT, _noMovementAction, this.gameObject);
    }
}
