using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Must be added if using SceneManager functions
using UnityEngine.SceneManagement;
// Must be added if using UI functions
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Creates a class variable to keep track of 'GameManager' instance
    static GameManager _instance = null;

    // Used to keep track of 'energy' in game
    int _energy;
    int _rockets;
    public Text energyText;
    public Text rocketsText;

    // Used to instantiate 'Character'
    public GameObject playerPrefab;
    // Use this for initialization
    void Start () {

        // Check if 'GameManager' instance exists
        if (instance)
            // 'GameManager' already exists, delete copy
            Destroy(gameObject);
        else
        {
            // 'GameManager' does not exist so assign a reference to it
            instance = this;

            // Do not destroy 'GameManager' on Scene change
            DontDestroyOnLoad(this);
        }

        // Assign a starting energy
        _energy = 1;
        _rockets = 3;
	}
	
	// Update is called once per frame
	void Update () {

        // Check if 'Escape' was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If player is on 'StartScreen' (Scene Name)
            if (SceneManager.GetActiveScene().name == "StartScreen")
            {
                // Go to 'Metroid-Remake' Scene
                // - Scene must be loaded in Build Settings or it will not work
                // - Build Settings are located at Menu Bar: Edit->Build Settings
                // - Drag the Scenes in the project into 'Scenes in Build' space
                SceneManager.LoadScene("Metroid-Remake");
                this.enabled = false;
            }
            // If player is on 'Metroid-Remake' (Scene Name)
            else if (SceneManager.GetActiveScene().name == "Metroid-Remake")
            {
                // Go to 'StartScreen' Scene
                // - Scene must be loaded in Build Settings or it will not work
                // - Build Settings are located at Menu Bar: Edit->Build Settings
                // - Drag the Scenes in the project into 'Scenes in Build' space
                SceneManager.LoadScene("StartScreen");
                spawnPlayer(1);
                this.enabled = true;
            }
            else if(SceneManager.GetActiveScene().name == "GameOver")
            {
                SceneManager.LoadScene("StartScreen");
                this.enabled = false;
            }
        }
        if (_energy <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOver");
            this.enabled = false;
        }

    
    }

    // Called when 'Character' is spawned
    public void spawnPlayer(int spawnLocation)
    //public void spawnPlayer(Transform spawnLocation)
    //public void spawnPlayer(Vector3 spawnLocation)
    //public void spawnPlayer(GameObject spawnLocation)
    {
        //// Requires spawnPoint to be named (SceneName)_(number)
        //// - Metroid-Remake_0
        //string spawnPointName = SceneManager.GetActiveScene().name
        //    + "_" + spawnLocation;

        //// Find location to spawn 'Character' at
        //Transform spawnPointTransform = 
        //    GameObject.Find(spawnPointName).GetComponent<Transform>();

        //// Check if 'playerPrefab' and 'spawnPointTransform' exist
        //if (playerPrefab && spawnPointTransform)
        //{
        //    // Instantiate (Create) 'Character' GameObject
        //    Instantiate(playerPrefab, spawnPointTransform.position,
        //        spawnPointTransform.rotation);
        //}
        //else
        //    // Prints a message to Console (Shortcut: Control+Shift+C)
        //    Debug.LogError("Missing Player Prefab or SpawnPoint");

    }

    // Give access to private variables (instance variables)
    // - Not needed if using public variables
    // - Variable must be declared above
    // - Variable and method must be static
    public static GameManager instance
    {
        get { return _instance; }   // can also use just 'get;'
        set { _instance = value; }  // can also use just 'set;'
    }

    // Gets called to Start game on button click
    public void StartGame()
    {
        // Loads Metroid-Remake Scene
        SceneManager.LoadScene("Metroid-Remake");
        spawnPlayer(1);
    }

    // Gets called to Quit game on button click
    public void QuitGame()
    {
        // Display a message that the game is quitting
        Debug.Log("Quitting...");

        // Quits game (only works on EXE, not in Editor)
        Application.Quit();
    }

    // Give access to private variables (instance variables)
    // - Not needed if using public variables
    public int energy
    {
        get { return _energy; }      // can also use just 'get;'
        set { _energy = value;       // can also use just 'set;'

            // Check if 'energyText' was set before trying to update HUD
            if (energyText)
                // Update HUD on every energy change
                energyText.text = "Energy: " + energy;  
        }     
    }
    public int rockets
    {
        get { return _rockets; }      // can also use just 'get;'
        set
        {   _rockets = value;       // can also use just 'set;'

            // Check if 'rocketsText' was set before trying to update HUD
            if (rocketsText)
                // Update HUD on every energy change
                rocketsText.text = "Rockets: " + rockets;
        }
    }
}
