using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour, ITrapTrigger
{
    [SerializeField]
    private Trap _trap;

    [SerializeField]
    private float _triggerDelay;

    private float _timePassed;

    private bool _triggered;

    private void Start()
    {
        _timePassed = 0;

        _triggered = false;

        Debug.Assert(_trap != null, "There is no trap on the provided object");
    }

    private void FixedUpdate()
    {
        if(_triggered && _timePassed >= _triggerDelay)
        {
            this.TriggerTrap();

            Destroy(this.gameObject);
        }
        else if(_triggered && _timePassed < _triggerDelay)
        {
            _timePassed += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _triggered = true;
        }
    }
    
    public void TriggerTrap()
    {
        _trap.Activate();
    }
}
