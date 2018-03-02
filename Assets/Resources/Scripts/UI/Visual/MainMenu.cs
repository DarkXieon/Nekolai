using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public bool isNewGame;
    public bool isContinue;
    public bool isOptions;
    public bool isQuit;

    void OnMouseUp() {
        if (isNewGame) 
        {
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isContinue)
        { 
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isOptions)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isQuit)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
            UnityEditor.EditorApplication.isPlaying = false; //temporary for testing in Unity
            //Application.Quit(); 
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
