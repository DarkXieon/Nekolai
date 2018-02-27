using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer //: MonoBehaviour
{
    // Declare the variables

    private float currentTime; // the current time left on the Timer
    private float cooldown; // the number to count down from

    // Constructors
    public Timer()
    {
        this.SetCooldown(0.0f);
        this.SetCurrentTime(0.0f);
    }

    public Timer(float ct, float c)
    {
        this.SetCooldown(c);
        this.SetCurrentTime(ct);
    }

    public Timer(float c)
    {
        this.SetCooldown(c);
        this.SetCurrentTime(c);
    }

    // Makes the current time back to the cooldown
    public void ResetTimer()
    {
        this.currentTime = this.cooldown;
    }

    // Subtracts time each FixedFrame
    public void Countdown() 
    {
        this.currentTime -= Time.fixedDeltaTime;
    }

    // Checks if time is up, returns true if current time has elasped cooldown, false otherwise
    public bool IsTimeUp()
    {
        bool b = false;
        
        if(this.currentTime <= 0.0f)
        {
            b = true;
        }

        return b;
    }

    public void SetCurrentTime(float t)
    {
            this.currentTime = t;
        
    }

    public void SetCooldown(float c)
    {
        this.cooldown = c;
    }

    public float GetTimeLeft()
    {
        return this.currentTime;
    }

    public float GetCooldown()
    {
        return this.cooldown;
    }

}
