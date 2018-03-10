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
            if(Input.GetKeyDown("d") || Input.GetKey("d"))
            {
                return _speedStat.GetCurrentValue();
            }
            else if(Input.GetKeyDown("a") || Input.GetKey("a"))
            {
                return _speedStat.GetCurrentValue() * -1;
            }

            return 0;
        }
    }



}
