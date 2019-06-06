using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    // Creates a class variable to keep track of 'GameManager' instance
    static SoundManager _instance = null;

    // Create a reference to AudioSource to be used when playing sounds for music and sfx
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Use this for initialization
    void Start () {
        // Check if 'SoundManager' instance exists
        if (instance)
            // 'SoundManager' already exists, delete copy
            Destroy(gameObject);
        else
        {
            // 'SoundManager' does not exist so assign a reference to it
            instance = this;

            // Do not destroy 'SoundManager' on Scene change
            DontDestroyOnLoad(this);
        }
    }
	
    // Called when a SFX needs to be played
    // - Accessible from anywhere that the 'SoundManager' is accessible
	public void PlaySingleSound(AudioClip clip, float volume=1.0f)
    {
        // Assign 'AudioClip' when function is called
        sfxSource.clip = clip;

        // Assign 'volume' to 'AudioSource' when function is called
        sfxSource.volume = volume;

        // Play assigned 'clip' through 'AudioSource'
        sfxSource.Play();  
    }

    // Give access to private variables (instance variables)
    // - Not needed if using public variables
    // - Variable must be declared above
    // - Variable and method must be static
    public static SoundManager instance
    {
        get { return _instance; }   // can also use just 'get;'
        set { _instance = value; }  // can also use just 'set;'
    }
}
