using UnityEngine;
using System.Collections;

public class JumpBehavior : AbstractBehavior
{
	public float JumpSpeed = 200f;
	public float JumpDelay = .1f;
	public int JumpCount = 2;

	protected float _lastJumpTime = 0;
	protected int _jumpsRemaining = 0;
    
	private void Update ()
    {
		var canJump = this._inputState.GetButtonValue (InputButtons [0]);
		var holdTime = this._inputState.GetButtonHoldTime (InputButtons [0]);

		if (this._collisionState.Standing)
        {
			if (canJump && holdTime < .1f)
            {
                this._jumpsRemaining = this.JumpCount - 1;
                this.Jump();
			}
		}
        else
        {
			if(canJump && holdTime < .1f && Time.time - this._lastJumpTime > this.JumpDelay)
            {
				if(this._jumpsRemaining > 0)
                {
                    this.Jump();
                    this._jumpsRemaining --;
				}
			}
		}
	}

	protected virtual void Jump()
    {
		var vel = _body2d.velocity;
		_lastJumpTime = Time.time;
		_body2d.velocity = new Vector2 (vel.x, JumpSpeed);
	}
}
