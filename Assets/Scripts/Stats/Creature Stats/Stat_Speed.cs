using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Speed : Stat
{
    public void Start()
    { 
        this.SetBaseValue(15.0f);

        this.SetBaseValue(5.0f);

        base.Start();
    }
}
