using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour
{
    public float LifeLength = 5f;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
