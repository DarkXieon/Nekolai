using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;

    private bool _paused;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        _paused = false;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {

        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseControl(); // How come you're not using this function instead? Left by: Kermit
            /*
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                hidePaused();
            }*/
        }
    }


    //Reloads the Level - Restart Button
    public void Reload() 
    {
        MasterAudioSource.mas.StopAllAudio(); // ALWAYS stop all audio before changing scenes

        // Play the appropiate audio depeneding on the scene name

        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();
        }

        Application.LoadLevel(Application.loadedLevel);
    }

    //controls the pausing of the scene - Resume Button
    public void pauseControl()
    {
        if (!_paused)
        {
            // Pauses the game -- Comment left by: Kermit
            MasterAudioSource.mas.PauseAllAudio();
            Time.timeScale = 0;
            _paused = !_paused;
            showPaused();
            
        }
        else if (_paused)
        {

            // Unpauses the game -- Comment left by: Kermit
            Time.timeScale = 1;
            hidePaused();
            _paused = !_paused;
            MasterAudioSource.mas.UnPauseAllAudio();

        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //loads inputed level - Main Menu button
    public void LoadLevel(string level)
    {
        MasterAudioSource.mas.StopAllAudio();
        SceneManager.LoadScene(0);
        //Application.LoadLevel(level);
    }
}
