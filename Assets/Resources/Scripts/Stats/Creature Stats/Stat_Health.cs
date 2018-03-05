using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat_Health : Stat
{
    private bool isAlive = true; // returns false if currentValue <= 0, true otherwise
    [SerializeField] private Text healthText; // the UI text for the health
    public static GameObject prefabCanvas; // the prefab of the health canvas

    // Use this for initialization
    void Start ()
    {
        base.Start();

        // Grab the data for the Canvas Prefab:

        if (prefabCanvas == null)
        {
            prefabCanvas = Resources.Load("Prefabs/UI/HealthCanvas") as GameObject;
        }

        // Instantiate a HealthCanvas for this game object
        var healthCanvas = Instantiate(prefabCanvas) as GameObject;
        healthCanvas.name = this.name + "_" +  "HealthCanvas";
        healthCanvas.GetComponent<BasicCameraFollow>().SetThingToFollow(this.gameObject.transform);
        this.healthText = healthCanvas.transform.GetChild(0).GetComponent<Text>();
        this.healthText.text = "HP: " + this.currentValue + " / " + this.baseValue;
    }


    public void ChangeHealth(float amount)
    {
        Debug.Log("They're after me Lucky Charms! | " + Time.time);

        this.SetCurrentValue(this.currentValue + amount);

    }

    // Override set current value to update UI on change

    public void SetCurrentValue(float v)
    {
        // Health must always be within [0, MaxHealth]
        this.currentValue = v;
        if (this.currentValue >= this.baseValue)
        {
            this.currentValue = this.baseValue;
        }
        else if (this.currentValue <= 0)
        {
            this.currentValue = 0;
            this.isAlive = false;
        }

        // Update health UI

        this.healthText.text = "HP: " + this.currentValue + " / " + this.baseValue;

        // Kill this game object if not alive

        if (!this.isAlive)
        {
            //**EDITED BY ANDREW KAUS TO ADD DEATH ANIMATION FUNCTIONALLITY**//
            EventManager.Instance.ExecuteObjectSpecificEvent(EventType.DEATH, this.gameObject);
            Destroy(this.gameObject, 3f);
            Destroy(this.healthText.transform.parent.gameObject, 3f);
        }

    }
}
