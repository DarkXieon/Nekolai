using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IJump
{
    float JumpHeight { get; }

    bool InAir { get; }
    
    bool UsedSecondJump { get; }

    void Jump();
}