using Assets.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    float Speed { get; set; }

    void Move();
}
