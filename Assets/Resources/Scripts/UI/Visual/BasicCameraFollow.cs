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

        // This stops the attempt at tracking after the transform has been removed from the scene, left by: Kermit
        if(_toFollow == null)
        {
            return;
        }
        var position = _toFollow.position;
        position.z = this.transform.position.z;

        this.transform.position = position;

    }

    // Used by Stat_Health
    public void SetThingToFollow(Transform t)
    {
        this._toFollow = t;
    }
}
