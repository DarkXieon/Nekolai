using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffect : MonoBehaviour
{
    // Declare the variables

    [SerializeField] private bool isInfiniteDuration; // false if the effect is on a timer; true otherwise
    [SerializeField] private float duration; // how long the effect lasts (in seconds)
    [SerializeField] private float amount; // how much to stat change to multiply or add by
    [SerializeField] private string modifierType; // "PreMult", "PostMult", "Additive"; refer to the Stat class
    public Timer timer; // a Timer to see when time runs out and the effect should be disabled 
    [SerializeField] private string effectType; // the name of the type of effect ("Haste", "Slow", "Steroid", etc.)

    public static List<string> AllEffectTypes; // the list of all valid effect types
    public static bool isAllEffectsLoaded; // true if AllEffectTypes has been initalized with all of the effect names, false otherwise

    // Constructors
    public BattleEffect()
    {
        // Setup list if not created already
        if (!isAllEffectsLoaded)
        {
            AllEffectTypes = new List<string>();

            // Load all BattleEffects into AllBattleEffects list

            AllEffectTypes.Add("Null"); // Null, an effect that does nothing
            AllEffectTypes.Add("Haste"); // Haste, an effect that increases speed
            AllEffectTypes.Add("Slow"); // Slow, an effect that decreases speed
            AllEffectTypes.Add("Steroid"); // Steroid, an effect that increases damage output/power/strength



            isAllEffectsLoaded = true;
        }

        // Assume these are the default conditions
        //this.timer = new Timer(this.duration);
        this.SetInfiniteStatus(false);
        //this.SetModifierType();
        this.SetAmount(1.0f);
        this.SetDuration(0.0f);
        this.SetEffectType("Null");
        
    }

    public BattleEffect(string et, float a, string s, float d, bool isInfinite)
    {

        // Setup list if not created already
        if (!isAllEffectsLoaded)
        {
            AllEffectTypes = new List<string>();

            // Load all BattleEffects into AllBattleEffects list

            AllEffectTypes.Add("Null"); // Null, an effect that does nothing
            AllEffectTypes.Add("Haste"); // Haste, an effect that increases speed
            AllEffectTypes.Add("Slow"); // Slow, an effect that decreases speed
            AllEffectTypes.Add("Steroid"); // Steroid, an effect that increases damage output/power/strength



            isAllEffectsLoaded = true;
        }

        //this.timer = new Timer(d);
        this.SetEffectType(et);
        this.SetAmount(a);
        this.SetModifierType(s);
        this.SetDuration(d);
        this.SetInfiniteStatus(isInfinite);
        
    }

    // Setters
    public void SetInfiniteStatus(bool b)
    {
        this.isInfiniteDuration = b;
    }

    public void SetDuration(float d)
    {
        if(this.timer == null)
        {
            this.timer = new Timer(d);
        }
        this.duration = d;
        this.timer.SetCooldown(d);
        //Debug.Log(this.timer.GetTimeLeft());
    }

    public void SetAmount(float a)
    {
        this.amount = a;
    }

    public void SetModifierType(string s)
    {
        // Only allow valid ModifierTypes
        if(s == "PreMult" || s == "PostMult" || s == "Additive")
        {
            this.modifierType = s;
        }
        else
        {
            Debug.LogError("Invalid valid for ModifierType: " + s);
        }
    }

    public void SetEffectType(string s)
    {
        // Return an error if not found in the list of valid effect types
        if(!AllEffectTypes.Contains(s))
        {
            Debug.LogError("Invalid value for EffectType: " + s);
        }
        else
        {
            this.effectType = s;
        }
    }

    // Getters
    public bool GetInfiniteStatus()
    {
        return this.isInfiniteDuration;
    }

    public float GetDuration()
    {
        return this.duration;
    }

    public float GetAmount()
    {
        return this.amount;
    }

    public string GetModifierType()
    {
        return this.modifierType;
    }

    public string GetEffectType()
    {
        return this.effectType;
    }

    
    private void Awake()
    {
        

    }
    


}
