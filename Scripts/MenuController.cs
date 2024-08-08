using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] PlayerInteraction playerInteraction;
    
    /// <summary>
    /// Load main menu scene
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quit Game
    /// </summary>
   public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    /// <summary>
    /// Load first level
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);

    }

    /// <summary>
    /// Handles pause interaction
    /// </summary>
    public void ReturnToGame()
    {
        playerInteraction.TogglePause();
    }
}
