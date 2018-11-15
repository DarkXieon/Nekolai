using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	private InputState _inputState;
	private WalkBehavior _walkBehavior;
	private Animator _animator;
	private CollisionState _collisionState;

	private void Awake()
    {
        _inputState = GetComponent<InputState> ();
		_walkBehavior = GetComponent<WalkBehavior> ();
		_animator = GetComponent<Animator> ();
		_collisionState = GetComponent<CollisionState> ();
	}
    
	private void Update ()
    {
        Debug.Log("Fix animations");
        /*
		if (_collisionState.standing)
        {
			ChangeAnimationState(0);
		}

		if (InputState.AbsoluteXVelocity > 0)
        {
			ChangeAnimationState(1);
		}

		if (InputState.AbsoluteYVelocity > 0)
        {
			ChangeAnimationState(2);
		}

		_animator.Speed = _walkBehavior.running ? _walkBehavior.runMultiplier : 1;
        */
	}

    private void ChangeAnimationState(int value)
    {
		_animator.SetInteger("AnimState", value);
	}
}
