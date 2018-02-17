using UnityEngine;
using UnityEditor;

public interface IMoveable
{
    Vector2 Speed { get; set; }

    void Move();
}