using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Controls
{
    public class CharacterMovement : MoveableObject
    {
        protected override Vector2 GetMovement()
        {
            //return the Horisontal move axis as a Vector2
            return new Vector2(Input.GetAxis("Horizontal"), 0f);
        }
    }
}
