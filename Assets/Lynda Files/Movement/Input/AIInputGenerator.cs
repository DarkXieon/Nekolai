using UnityEngine;
using System.Collections;

public abstract class AIButtonPress
{
    public abstract Buttons Button { get; }

    public abstract bool PressingButton { get; }

    protected Transform _aiTransform;

    public AIButtonPress(Transform aiTransform)
    {
        _aiTransform = aiTransform;
    }
}

public class AIMoveRightButtonPress : AIButtonPress
{
    public override Buttons Button { get { return Buttons.RIGHT; } }

    public override bool PressingButton
    {
        get
        {
            return _playerTransform.position.x > _aiTransform.position.x;
        }
    }

    private Transform _playerTransform;

    public AIMoveRightButtonPress(Transform aiTransform)
        : base(aiTransform)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}

public class AIMoveLeftButtonPress : AIButtonPress
{
    public override Buttons Button { get { return Buttons.LEFT; } }

    public override bool PressingButton
    {
        get
        {
            return _playerTransform.position.x < _aiTransform.position.x;
        }
    }

    private Transform _playerTransform;

    public AIMoveLeftButtonPress(Transform aiTransform)
        : base(aiTransform)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}

public class AIButtonPressFactory
{
    private Transform _aiTransform;

    public AIButtonPressFactory(Transform aiTransform)
    {
        _aiTransform = aiTransform;
    }

    public AIButtonPress MakeButton(Buttons button)
    {
        switch(button)
        {
            case Buttons.RIGHT:
                return new AIMoveRightButtonPress(_aiTransform);
            case Buttons.LEFT:
                return new AIMoveLeftButtonPress(_aiTransform);
            default:
                throw new System.NotImplementedException();
        }
    }
}

public class AIInputGenerator : MonoBehaviour
{
    public AIButtonPress[] Inputs;
    public InputState InputState;

    private AIButtonPressFactory _factory;

    private void Awake()
    {
        _factory = new AIButtonPressFactory(this.transform);

        this.Inputs = new AIButtonPress[] 
        {
            _factory.MakeButton(Buttons.LEFT),
            _factory.MakeButton(Buttons.RIGHT)
        };
    }

    private void Update()
    {
        foreach (var input in Inputs)
        {
            InputState.SetButtonValue(input.Button, input.PressingButton);
        }
    }
}