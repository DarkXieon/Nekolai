using UnityEngine;
using System.Collections;

public class FaceDirection : AbstractBehavior
{
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();

        _renderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update ()
    {
		var right = this._inputState.GetButtonValue (InputButtons [0]);
		var left = this._inputState.GetButtonValue (InputButtons [1]);

		if (right)
        {
            this._inputState.Direction = Directions.RIGHT;
		} else if (left)
        {
            this._inputState.Direction = Directions.LEFT;
		}

        _renderer.flipX = _inputState.Direction == Directions.LEFT;

    }
}
