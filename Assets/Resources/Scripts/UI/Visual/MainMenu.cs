using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool isNewGame;
    public bool isContinue;
    public bool isOptions;
    public bool isQuit;

    void OnMouseUp() {
        if (isNewGame) 
        {
            Debug.LogWarning("HEHE_1");
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);
            
            //GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isContinue)
        {
            Debug.LogWarning("HEHE_2");
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);
           // GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isOptions)
        {
            //GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isQuit)
        {
            //Debug.LogWarning("HEHE_3");
            //GetComponent<Renderer>().material.color = Color.cyan;
            //UnityEditor.EditorApplication.isPlaying = false; //temporary for testing in Unity
            Application.Quit(); 
        }
    }

    /*
    public void CallMeNow()
    {
        if (isNewGame)
        {
            Debug.LogWarning("HEHE_1");
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);

            //GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isContinue)
        {
            Debug.LogWarning("HEHE_2");
            // Update the audio
            MasterAudioSource.mas.StopAllAudio();
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();

            // Change the scene -- Comment left by: Kermit
            SceneManager.LoadScene(1);
            // GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isOptions)
        {
            //GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (isQuit)
        {
            Debug.LogWarning("HEHE_3");
            //GetComponent<Renderer>().material.color = Color.cyan;
            //UnityEditor.EditorApplication.isPlaying = false; //temporary for testing in Unity
            //Application.Quit(); 
        }
    }*/
}
