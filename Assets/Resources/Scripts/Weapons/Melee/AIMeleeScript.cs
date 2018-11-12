using UnityEngine;
using System.Collections;

public class AIMeleeScript : MeleeScript
{
    private Transform _moveTo;

    private Transform _moverTransform;

    protected override void Awake()
    {
        base.Awake();

        _moveTo = GameObject.FindGameObjectWithTag("Player").transform;

        _moverTransform = this.transform;
    }

    protected override bool WillAttack()
    {
        var xDistance = Mathf.Abs(_moveTo.position.x - _moverTransform.position.x);

        var yDistance = Mathf.Abs(_moveTo.transform.position.y - _moverTransform.transform.position.y);

        var inRangeAmount = this.WeaponLength + 1.5f;

        return inRangeAmount >= xDistance && yDistance < 3;
    }
}
