using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_onclick : MonoBehaviour {
    public Button btn;
	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TaskOnClick() {
        Debug.LogWarning("You have clicked the button");
    }
}
