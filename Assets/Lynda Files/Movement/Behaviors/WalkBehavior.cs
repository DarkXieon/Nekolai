using UnityEngine;
using System.Collections;

public class WalkBehavior : AbstractBehavior
{
	public float Speed = 50f;
    
	private void Update ()
    {
		var isRightDown = this._inputState.GetButtonValue(this.InputButtons[0]);
		var isLeftDown = this._inputState.GetButtonValue(this.InputButtons[1]);
        
		if ( (isRightDown && !this._collisionState.HittingRightWall) || (isLeftDown && !this._collisionState.HittingLeftWall) )
        {
			var xVelocity = this.Speed * (float)this._inputState.Direction;
            
			this._body2d.velocity = new Vector2(xVelocity, this._body2d.velocity.y);
		}
        else
        {
            this._body2d.velocity = new Vector2(0, this._body2d.velocity.y);
        }
	}
}
