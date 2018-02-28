using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffectManager : MonoBehaviour
{
    // Declare the variables
    
    public List<BattleEffect> activeEffects; // the list of BattleEffects currently affecting this object
    public List<BattleEffect> removedEffects; // the list of BattleEffects no longer affecting this object

    // Methods

    public void AddStatModifier(string statName, string modifierType, float amount)
    {
        // Figure out the stat to change, then the modifier type, and then call the add modifier methods with the amount as the argument
        
        if(statName == "Speed")
        {
            if(modifierType == "PreMult")
            {
                this.GetComponent<Stat_Speed>().AddPreMultModifier(amount);
            }
            else if(modifierType == "PostMult")
            {
                this.GetComponent<Stat_Speed>().AddPostMultModifier(amount);
            }
            else if(modifierType == "Additive")
            {
                this.GetComponent<Stat_Speed>().AddAdditiveModifier(amount);
            }
            else
            {
                Debug.LogError("Invalid modifier type: " + modifierType);
            }
        }
        else if (statName == "Power")
        {
            if (modifierType == "PreMult")
            {
                this.GetComponent<Stat_Power>().AddPreMultModifier(amount);
            }
            else if (modifierType == "PostMult")
            {
                this.GetComponent<Stat_Power>().AddPostMultModifier(amount);
            }
            else if (modifierType == "Additive")
            {
                this.GetComponent<Stat_Power>().AddAdditiveModifier(amount);
            }
            else
            {
                Debug.LogError("Invalid modifier type: " + modifierType);
            }
        }
        else
        {
            Debug.LogError("Invalid stat name: " + statName);
        }



    }

    private void Start()
    {
        this.activeEffects = new List<BattleEffect>();
        this.removedEffects = new List<BattleEffect>();
    }

    public void FixedUpdate()
    {
        if (this.activeEffects.Count > 0)
        {
            foreach (BattleEffect battleEffect in activeEffects)
            {
                /*// Display the current status of the battle effect
                Debug.Log("BattleEffect Type: " + battleEffect.GetEffectType() + " | " +
                    "Duration: " + battleEffect.GetDuration() + " | " +
                    "Time Left: " + battleEffect.timer.GetTimeLeft());*/

                // Countdown from the timer
                battleEffect.timer.Countdown();

                // Remove the battle effect from the list if time is up
                if ((battleEffect.GetInfiniteStatus() == false) && battleEffect.timer.IsTimeUp())
                {
                    Debug.LogWarning("Time's up! | " + Time.time);

                    
                    

                    // Reset the Timer time
                    battleEffect.timer.ResetTimer();

                     // Remove the stat modifiers

                    switch(battleEffect.GetEffectType())
                    {
                        default:
                            Debug.LogError("Invalid effect type: " + battleEffect.GetEffectType());
                            break;

                        case "Haste":
                            AddStatModifier("Speed", "PreMult", battleEffect.GetAmount() * -1);
                            // Stop the power up music
                            MasterAudioSource.mas.transform.Find("PowerUp_Haste").GetComponent<AudioSource>().Stop();
  

                            break;

                        case "Slow":
                            AddStatModifier("Speed", "PreMult", battleEffect.GetAmount() * -1);
                            break;

                        case "Steroid":
                            AddStatModifier("Power", "PostMult", battleEffect.GetAmount() * -1);
                            break;

                    }

                    // Resume the level music
                    MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().UnPause();

                    // Add this item to list of effects to be removed
                    // Remove the battle effect from the list of active effects <-- 
                    this.removedEffects.Add(battleEffect);


                }

            }
        }

        if(this.removedEffects.Count > 0)
        {
            // Remove each item in this list from the active effects

            foreach(BattleEffect battleEffect in removedEffects)
            {
                this.activeEffects.Remove(battleEffect);
            }

            // Clear this list
            this.removedEffects.Clear();
        }
    }
}
