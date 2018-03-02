using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour, ITrapTrigger
{
    [SerializeField]
    private Trap _trap;

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(_trap != null, "There is no trap on the provided object");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.TriggerTrap();
        
        Destroy(this.gameObject);
    }
    
    public void TriggerTrap()
    {
        _trap.Activate();
    }
}
