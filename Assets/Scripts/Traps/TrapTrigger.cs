using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour, ITrapTrigger
{
    [SerializeField]
    private GameObject _trapOn;
    
    private ITrap _trap;

    // Use this for initialization
    void Start()
    {
        _trap = _trapOn.GetComponent<ITrap>();

        Debug.Assert(_trap != null, "There is no trap on the provided object");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TriggerTrap()
    {
        _trap.Activate();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        this.TriggerTrap();

        Destroy(this);
    }
}
