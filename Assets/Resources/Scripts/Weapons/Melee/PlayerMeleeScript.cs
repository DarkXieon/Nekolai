using UnityEngine;
using System.Collections;

public class PlayerMeleeScript : MeleeScript
{
    protected override bool WillAttack()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
