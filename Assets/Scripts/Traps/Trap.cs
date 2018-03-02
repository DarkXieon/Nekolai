using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour, ITrap
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void Activate();
}
