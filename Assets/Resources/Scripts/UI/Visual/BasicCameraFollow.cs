using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _toFollow;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var position = _toFollow.position;
        position.z = this.transform.position.z;

        this.transform.position = position;

    }
}
