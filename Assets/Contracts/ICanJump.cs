using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Contracts
{
    public interface ICanJump
    {
        float JumpHeight { get; set; }
        
        bool IsGrounded { get; set; }

        void Jump();
    }
}
