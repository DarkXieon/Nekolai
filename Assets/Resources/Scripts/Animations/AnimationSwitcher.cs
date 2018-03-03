using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class AnimationSwitcher : MonoBehaviour
{
    [SerializeField]
    private string _stateParamName;

    [SerializeField]
    private string _jumpTriggerName;

    [SerializeField]
    private string _inAirParamName;

    private Animator _animator;
    
    private UnityAction _jumpAction;

    private UnityAction _landAction;

    private UnityAction _walkAction;

    private UnityAction _noMovementAction;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        
        lock (_animator)
        {
            _jumpAction = () =>
            {
                _animator.SetInteger(_stateParamName, 1);
                _animator.SetTrigger(_jumpTriggerName);
                _animator.SetBool(_inAirParamName, true);
            };
            EventManager.Instance.AddObjectSpecificListener(EventType.JUMP, _jumpAction, this.gameObject);

            _landAction = () =>
            {
                _animator.SetInteger(_stateParamName, 2);
                _animator.SetBool(_inAirParamName, false);
            };
            EventManager.Instance.AddObjectSpecificListener(EventType.LAND, _landAction, this.gameObject);

            _walkAction = () => _animator.SetInteger(_stateParamName, 3);
            EventManager.Instance.AddObjectSpecificListener(EventType.WALK, _walkAction, this.gameObject);

            _noMovementAction = () => _animator.SetInteger(_stateParamName, 4);
            EventManager.Instance.AddObjectSpecificListener(EventType.NO_MOVEMENT, _noMovementAction, this.gameObject);
        }
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.RemoveObjectSpecificListener(EventType.JUMP, _jumpAction, this.gameObject);
        EventManager.Instance.RemoveObjectSpecificListener(EventType.WALK, _walkAction, this.gameObject);
        EventManager.Instance.RemoveObjectSpecificListener(EventType.NO_MOVEMENT, _noMovementAction, this.gameObject);
    }
}
