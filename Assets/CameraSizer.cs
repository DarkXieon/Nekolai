using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().aspect = (16.0f / 9.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
