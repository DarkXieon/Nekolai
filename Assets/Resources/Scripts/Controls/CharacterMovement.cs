using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Controls
{
    public class CharacterMovement : MoveableObject
    {
        protected override float GetMovement()
        {
            //return the Horisontal move axis as a Vector2
            return Input.GetAxis("Horizontal");
        }
    }
}
