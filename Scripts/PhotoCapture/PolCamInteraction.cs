using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles camera switching between main and polaroid view.
/// </summary>
public class PolCamInteraction : MonoBehaviour
{
   
    public Camera playerCam;
    public Camera polCam;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCam();
    }
    
    /// <summary>
    /// Toggles player cam
    /// </summary>
    public void PlayerCam() 
    {
        playerCam.gameObject.SetActive(true);
        polCam.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles polaroid cam
    /// </summary>
    public void PolCam()
    { 
        playerCam.gameObject.SetActive(false);
        polCam.gameObject.SetActive(true);
    }
}
