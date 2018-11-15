using UnityEngine;
using System.Collections;

public class PlayerJump : EntityJump
{
    // Update is called once per frame
    protected override void Update()
    {
        //CanJumpTwice = false;

        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            CanJumpTwice = !CanJumpTwice;
        }
        */

        base.Update();
    }
    
    protected override bool ShouldJump()
    {
        return Input.GetKeyDown(KeyCode.W) && (!this.InAir || !this.UsedSecondJump);
    }
}
