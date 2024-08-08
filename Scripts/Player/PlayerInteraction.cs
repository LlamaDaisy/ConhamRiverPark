using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header ("Journal")]
    [SerializeField] GameObject journal;
    [SerializeField] public bool journalOpen = false;

    [Header ("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    public bool pause = false;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        //makes sure pause and journal are inactive
        if (journal != null)
        {
            journal.SetActive (false);
            pauseMenu.SetActive (false);
        }

        playerMovement = GetComponent<PlayerMovement> ();

        //hides cursor and locks pos
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //handles opening/ closing journal
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            ToggleJournal();

        }
        //toggles pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    /// <summary>
    /// handles visability of journal canvas
    /// </summary>
    void ToggleJournal()
    {
        journalOpen = !journalOpen;
        journal.SetActive (journalOpen);
    }
    
    /// <summary>
    /// handles pause functionality 
    /// </summary>
    public void TogglePause()
    {
        pause = !pause;
        pauseMenu.SetActive (pause);

        if(pause)
        {
            //pauses the game
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //disables player movement while paused
            if(playerMovement != null)
            {
                playerMovement.enabled = false;
            }
        }

        else
        {
            //resume game
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //re-enable player movement
            if(playerMovement != null)
            {
                playerMovement.enabled = true;
            }
        }
    }
}
