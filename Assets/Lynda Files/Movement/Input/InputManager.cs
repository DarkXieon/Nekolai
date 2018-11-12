using UnityEngine;
using System.Collections;
using System;

public enum Buttons
{
	RIGHT,
	LEFT,
	UP,
	DOWN,
	A,
	B
}

public enum Condition
{
    GREATER_THAN,
    LESS_THAN
}

[Serializable]
public class InputAxisState
{
    public string AxisName;
    public float OffValue;
    public Buttons Button;
    public Condition Condition;

    public bool IsPressed
    {
        get
        {
            var axisValue = Input.GetAxis(this.AxisName);

            switch (this.Condition)
            {
                case Condition.GREATER_THAN:
                    return axisValue > this.OffValue;
                case Condition.LESS_THAN:
                    return axisValue < this.OffValue;
            }

            return false;
        }
    }
}

public class InputManager : MonoBehaviour
{
	public InputAxisState[] Inputs;
	public InputState InputState;
    
	private void Update ()
    {
		foreach (var input in Inputs) {
			InputState.SetButtonValue(input.Button, input.IsPressed);
		}
	}
}