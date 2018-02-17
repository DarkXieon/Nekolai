using UnityEngine;
using System.Collections;
using Assets.Contracts;

public class EntityMovement : MoveableObject
{
    //public LayerMask Ground;

    protected override Vector2 GetMovementDirection()
    {
        //return the Horisontal move axis as a Vector2
        return new Vector2(Input.GetAxis("Horizontal"), 0f);
    }
}
