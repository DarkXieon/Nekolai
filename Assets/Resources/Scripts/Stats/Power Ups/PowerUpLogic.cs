using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleEffect))]
public class PowerUpLogic : MonoBehaviour
{
    [SerializeField] public BattleEffect battleEffect; // the effect to add to the player on collision

    private void Start()
    {
        // Setup this partiuclar battleEffect
        battleEffect = new BattleEffect
            (
                this.GetComponent<BattleEffect>().GetEffectType(), 
                this.GetComponent<BattleEffect>().GetAmount(), 
                this.GetComponent<BattleEffect>().GetModifierType(), 
                this.GetComponent<BattleEffect>().GetDuration(), 
                this.GetComponent<BattleEffect>().GetInfiniteStatus()
            );

        //battleEffect = //new BattleEffect("Haste", 0.5f, "PreMult", 15, false);
        //battleEffect = new BattleEffect("Steroid", 2, "PostMult", 15, false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("YEEHAH!: | " + Time.time + " | " + other.name);

        // Add a power up when the player collides
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<BattleEffectManager>() == null)
            {
                Debug.LogError("This gameobject is missing a BattleEffectManager");
            }
            else
            {
                other.gameObject.GetComponent<BattleEffectManager>().activeEffects.Add(battleEffect);

                // Pause the level music  
                MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Pause();
                //MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().time = 0.0f;

                switch (battleEffect.GetEffectType())
                {
                    default:
                        Debug.LogError("Invalid effect type: " + battleEffect.GetEffectType());
                        break;

                    case "Haste":
                        // Add the stat modifier
                        other.gameObject.GetComponent<BattleEffectManager>().AddStatModifier("Speed", "PreMult", battleEffect.GetAmount());
                        // Play the PowerUp Theme
                        MasterAudioSource.mas.transform.Find("PowerUp_Haste").GetComponent<AudioSource>().Play();
                        break;

                    case "Slow":
                        other.gameObject.GetComponent<BattleEffectManager>().AddStatModifier("Speed", "PreMult", battleEffect.GetAmount());
                        break;

                    case "Steroid":
                        other.gameObject.GetComponent<BattleEffectManager>().AddStatModifier("Power", "PostMult", battleEffect.GetAmount());
                        break;

                }

                
            }


            // Prevent the PowerUp from stacking and make it disappears

            Destroy(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Nani?!");
        }

        

        
    }
}
