using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudioSource : MonoBehaviour
{
    // Declare the variables

    public static MasterAudioSource mas; // a singleton that accesses all audio files

    private void Awake()
    {

        // Singleton logic
        if(mas == null)
        {
            mas = this;
            DontDestroyOnLoad(mas);
        }
        else
        {
            Destroy(this);
        }
    }

    // pauses all audio in game
    public void PauseAllAudio()
    {
        foreach(AudioSource audioSource in mas.GetComponentsInChildren<AudioSource>())
        {
            if (audioSource.isPlaying)
            {
                // Debug.Log(audioSource.gameObject.name + " | " + Time.time);
                audioSource.Pause();
                audioSource.gameObject.AddComponent<Stat>(); 
                // ^^ Mark audio paused by this function with an unusual component to make unpausing them easier
            }
        }
    }

    // unpauses all audio in game, 
    public void UnPauseAllAudio()
    {
        foreach (AudioSource audioSource in mas.GetComponentsInChildren<AudioSource>())
        {
            if (audioSource.GetComponent<Stat>() != null)
            {
                // Debug.Log(audioSource.gameObject.name + " | " + Time.time);
                audioSource.UnPause();
                Destroy(audioSource.GetComponent<Stat>()); // Get rid of the unusual component from PauseAllAudio
            }
        }
    }

    // stops all audio clips in the game
    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in mas.GetComponentsInChildren<AudioSource>())
        {
            // Debug.Log(audioSource.gameObject.name + " | " + Time.time);
            audioSource.Stop();
           // audioSource.
                
        }
    }
}
