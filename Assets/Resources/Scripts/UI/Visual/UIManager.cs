using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    private GameObject _nekolai;
    private GameObject _pauseCanvas;
    private CameraFollow _cameraFollow;

    GameObject[] pauseObjects;

    private bool _paused;

    //**ADDED BY ANDREW FOR PAUSE MENU FUNCTIONALLITY**//
    
    private UnityAction _resumeAction;
    private UnityAction _restartAction;
    private UnityAction _quitAction;

    //**END ADDITION**//

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        _paused = false;
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _nekolai = GameObject.FindGameObjectWithTag("Player");
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        _pauseCanvas = pauseObjects[0];
        hidePaused();

        //**ADDED BY ANDREW FOR PAUSE MENU FUNCTIONALLITY**//
        
        _resumeAction = () => this.pauseControl();
        _restartAction = () => this.Reload();
        _quitAction = () => this.LoadLevel("Main Menu");


        EventManager.Instance.AddListener(EventType.RESUME_ON_CLICK, _resumeAction);
        EventManager.Instance.AddListener(EventType.RESTART_ON_CLICK, _restartAction);
        EventManager.Instance.AddListener(EventType.QUIT_ON_CLICK, _quitAction);
        EventManager.Instance.AddListener(EventType.PLAYER_FELL_OFF_MAP, _restartAction);
        EventManager.Instance.AddObjectSpecificListener(EventType.DEATH, _restartAction, _nekolai);
        //**END ADDITION**//
    }

    // Update is called once per frame
    void Update()
    {
        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            this.Reload();
        }
    }

    //CHANGE BY: ANDREW-----MAKE METHOD PRIVATE//
    //Reloads the Level - Restart Button
    private void Reload() 
    {
        MasterAudioSource.mas.StopAllAudio(); // ALWAYS stop all audio before changing scenes

        // Play the appropiate audio depeneding on the scene name

        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            MasterAudioSource.mas.transform.Find("Level_1").GetComponent<AudioSource>().Play();
        }

        Application.LoadLevel(Application.loadedLevel);
    }

    //CHANGE BY: ANDREW-----MAKE METHOD PRIVATE//
    //controls the pausing of the scene - Resume Button
    private void pauseControl()
    {
        if (!_paused)
        {
            // Pauses the game -- Comment left by: Kermit
            MasterAudioSource.mas.PauseAllAudio();
            Time.timeScale = 0;
            _paused = !_paused;
            showPaused();
            _cameraFollow.ThingToFollow = _pauseCanvas.transform;
            _cameraFollow.YOffset = 0;
        }
        else if (_paused)
        {

            // Unpauses the game -- Comment left by: Kermit
            Time.timeScale = 1;
            hidePaused();
            _paused = !_paused;
            MasterAudioSource.mas.UnPauseAllAudio();
            _cameraFollow.ThingToFollow = _nekolai.transform;
            _cameraFollow.YOffset = 10;
        }
    }

    //CHANGE BY: ANDREW-----MAKE METHOD PRIVATE//
    //shows objects with ShowOnPause tag
    private void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //CHANGE BY: ANDREW-----MAKE METHOD PRIVATE//
    //hides objects with ShowOnPause tag
    private void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //CHANGE BY: ANDREW-----MAKE METHOD PRIVATE//
    //loads inputed level - Main Menu button
    private void LoadLevel(string level)
    {
        MasterAudioSource.mas.StopAllAudio();
        SceneManager.LoadScene(0);
        //Application.LoadLevel(level);
    }
}
