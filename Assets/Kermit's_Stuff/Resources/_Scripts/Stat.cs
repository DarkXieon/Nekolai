using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    // Declare the variables

    /*readonly*/
    [SerializeField] float baseValue; // the base stat BEFORE applying modifiers, min = 0
    [SerializeField] float preMultModifiers = 1; // the factor to scale the BASE value; 1 + the sum of all preMultMods as decimals (+30% of base = 0.3)
    [SerializeField] float postMultModifiers = 1; // the factor to scale the CURRENT value; 1 + the sum of all postMultMods as decimals (+30% overall = 0.3)
    [SerializeField] float additiveModifiers = 0; // the 0 + sum of all additive modifiers (i.e - "+5 defense" = 5)
    [SerializeField] float currentValue; // the current value of the stat AFTER modifiers; 
    //currentValue = (((baseValue * (preMultModifiers)) + (additiveModifiers)) * postMultModifiers);

    // Start

    public void Start()
    {
        this.CalculateCurrentValue();
    }

    // Constructors

    public Stat()
    {
        this.SetBaseValue(0.0f);
        this.SetCurrentValue(0.0f);
        this.SetPreMultModifier(1.0f);
        this.SetPostMultModifier(1.0f);
        this.SetAdditiveModifier(0.0f);
    }

    public Stat(float b, float c, float preM, float postM, float a)
    {
        this.SetBaseValue(b);
        this.SetCurrentValue(c);
        this.SetPreMultModifier(preM);
        this.SetPostMultModifier(postM);
        this.SetAdditiveModifier(a);
    }

    // Setters

    public void SetBaseValue(float b)
    {
        this.baseValue = b;
        this.CalculateCurrentValue();
    }

    public void SetCurrentValue(float c)
    {
        this.currentValue = c;
        this.CalculateCurrentValue();
    }

    public void SetPreMultModifier(float m)
    {
        this.preMultModifiers = m;
        this.CalculateCurrentValue();
    }

    public void SetPostMultModifier(float m)
    {
        this.postMultModifiers = m;
        this.CalculateCurrentValue();
    }

    public void SetAdditiveModifier(float m)
    {
        this.additiveModifiers = m;
        this.CalculateCurrentValue();
    }

    // Getters

    public float GetBaseValue()
    {
        return this.baseValue;
    }

    public float GetCurrentValue()
    {
        return this.currentValue;
    }

    public float GetPreMultModifiers()
    {
        return this.preMultModifiers;
    }

    public float GetPostMultModifiers()
    {
        return this.postMultModifiers;
    }

    public float GetAdditiveModifiers()
    {
        return this.additiveModifiers;
    }


    // Methods

    // Calculates the Current Stat Value; currentValue = (((baseValue * (preMultModifiers)) + (additiveModifiers)) * postMultModifiers); 
    public void CalculateCurrentValue()
    {
        /* this.currentValue = 
             (((this.baseValue * (this.preMultModifiers)) + (this.additiveModifiers)) * this.postMultModifiers);*/

        this.currentValue = this.baseValue;
        this.currentValue *= this.preMultModifiers;
        this.currentValue += this.additiveModifiers;
        this.currentValue *= this.postMultModifiers;

        // Stats cannot be less than 0
        if (this.currentValue < 0.0f)
        {
            this.currentValue = 0.0f;
        }

    }

    // Adds a new PreMult Modifier into the stat calculation
    public void AddPreMultModifier(float m)
    {
        this.preMultModifiers += m;
        this.CalculateCurrentValue();
    }

    // Adds a new PostMult Modifier into the stat calculation
    public void AddPostMultModifier(float m)
    {
        this.postMultModifiers += m;
        this.CalculateCurrentValue();
    }

    // Adds a new Additive Modifier into the stat calculation
    public void AddAdditiveModifier(float m)
    {
        this.additiveModifiers += m;
        this.CalculateCurrentValue();
    }

    // Sets all modifiers to default values
    public void ClearAllModifiers()
    {
        this.SetPreMultModifier(1.0f);
        this.SetPostMultModifier(1.0f);
        this.SetAdditiveModifier(0.0f);
    }

    

}

