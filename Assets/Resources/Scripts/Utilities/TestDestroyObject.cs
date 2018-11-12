using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDestroyObject : MonoBehaviour
{
    public GameObject ToDestroy;

    public CompositeCollider2D CompositeCollider;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(CompositeCollider.bounds.size);
        Debug.Log(CompositeCollider.shapeCount);
        Destroy(ToDestroy, 0);
        Debug.Log(CompositeCollider.shapeCount);
    }
}
