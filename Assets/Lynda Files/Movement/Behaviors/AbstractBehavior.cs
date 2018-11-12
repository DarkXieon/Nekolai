using UnityEngine;
using System.Collections;

public abstract class AbstractBehavior : MonoBehaviour
{
	public Buttons[] InputButtons;

	protected InputState _inputState;
	protected Rigidbody2D _body2d;
	protected CollisionState _collisionState;

	protected virtual void Awake()
    {
        _inputState = GetComponent<InputState> ();
		_body2d = GetComponent<Rigidbody2D> ();
		_collisionState = GetComponent<CollisionState> ();
	}
}
