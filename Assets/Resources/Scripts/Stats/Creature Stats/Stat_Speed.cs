using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Speed : Stat
{
    public void Start()
    { 
        base.Start();

        this.SetPostMultModifier(14);
    }
}
