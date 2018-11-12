using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonState
{
	public bool Value;
	public float HoldTime = 0;
}

public enum Directions
{
	RIGHT = 1,
	LEFT = -1
}

public class InputState : MonoBehaviour
{
	public Directions Direction = Directions.RIGHT;
	public float AbsoluteXVelocity = 0f;
	public float AbsoluteYVelocity = 0f;

	private Rigidbody2D _body2d;
	private Dictionary<Buttons, ButtonState> _buttonStates = new Dictionary<Buttons, ButtonState>();

	private void Awake()
    {
		_body2d = GetComponent<Rigidbody2D> ();
	}

    private void FixedUpdate()
    {
		AbsoluteXVelocity = Mathf.Abs (_body2d.velocity.x);
		AbsoluteYVelocity = Mathf.Abs (_body2d.velocity.y);
	}

	public void SetButtonValue(Buttons key, bool value)
    {
		if(!_buttonStates.ContainsKey(key))
			_buttonStates.Add(key, new ButtonState());

		var state = _buttonStates [key];

		if (state.Value && !value)
        {
			state.HoldTime = 0;
		}
        else if (state.Value && value)
        {
			state.HoldTime += Time.deltaTime;
		}

		state.Value = value;

	}

	public bool GetButtonValue(Buttons key)
    {
		if (_buttonStates.ContainsKey (key))
			return _buttonStates [key].Value;
		else
			return false;
	}

	public float GetButtonHoldTime(Buttons key)
    {
		if (_buttonStates.ContainsKey (key))
			return _buttonStates [key].HoldTime;
		else
			return 0;
	}
}