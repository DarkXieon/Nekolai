using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour, ITrap
{
    public abstract void Activate();
}
