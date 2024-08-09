using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controls the camera movement")]
    float yRotation;
    float xRotation;

    [Header("Controls camera speed")]
    [SerializeField]
    float lookSensitivity;

    [Header("3D Rigid Body")]
    Rigidbody rb;

    [Header("Player Movement")]
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    Camera polaroidCam;
    [SerializeField] GameObject polCamUI;

    public bool polaroidView;

    [SerializeField] AudioSource walkingSFX;

    PolCamInteraction camSwitch;

    [SerializeField] PlayerInteraction PI;


    void Start()
    {
        //locks the cursor to the centre of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //Hides the cursor
        Cursor.visible = false;
        polaroidView = false;

        rb = GetComponent<Rigidbody>();
        camSwitch = GetComponent<PolCamInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        //toggles polaroid/ photo mode
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            TogglePolaroidView();
        }

        //if journal is open toggle the polaroid view
        if (PI.journalOpen)
        {
            if (polaroidView)
            {
                TogglePolaroidView();
            }

            //renable cursor if journal is open
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //set camera to player cam
            camSwitch.PlayerCam();
            //hide polaroid UI
            polCamUI.SetActive(false);
        }

        else
        {
            //lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //if polaroid view true - look mode in polaroid cam
            if (polaroidView)
            {
                Look(polaroidCam);
            }

            //set look mode to main cam
            else
            {
                Look(Camera.main);
            }

        }

    }

    private void FixedUpdate()
    {
        //handles movement based on what cam view/ mode you are in
        if (!PI.journalOpen)
        {
            if (polaroidView)
            {
                Move(polaroidCam);
            }

            else
            {
                Move(Camera.main);
            }
        }

        playWalkingSFX();
    }

    /// <summary>
    /// Controls Camera Movement
    /// </summary>
    void Look(Camera camera)
    {
        xRotation -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;

        //Binds the look within 90 degree parameters up and down.
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        yRotation += Input.GetAxisRaw("Mouse X") * lookSensitivity;


        //Rotating the camera based on the local rotation of the player. 
        //Quaternion used to represent rotation, Euler represents angles expressed in degrees of rotation 
        camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    void Move(Camera cam)
    {
        Vector2 axis = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")).normalized * walkSpeed;

        //getting the forward position based on the camera's right position
        Vector3 forward = new Vector3(-cam.transform.right.z, 0, cam.transform.right.x);

        //combine these together to get the move direction
        Vector3 moveDirection = (forward * axis.x + cam.transform.right * axis.y + Vector3.up * rb.velocity.y);

        rb.velocity = moveDirection;
    }

    /// <summary>
    /// Toggle polaroid view/ cam mode
    /// </summary>
    void TogglePolaroidView()
    {
        polaroidView = !polaroidView;
        if(polaroidView)
        {
            polCamUI.SetActive(true);
            camSwitch.PolCam();
        }

        else
        {
            camSwitch.PlayerCam();
            polCamUI.SetActive(false);    
        } 
    }

    /// <summary>
    /// Checks is player is walking
    /// </summary>
    /// <returns></returns>
    bool isWalking()
    {
        return rb.velocity.x != 0 ||  rb.velocity.z != 0;
    }

    /// <summary>
    /// handles walking sfx when player isWalking true
    /// </summary>
    void playWalkingSFX()
    {
        if(isWalking())
        {
            if (!walkingSFX.isPlaying)
            {
                walkingSFX.Play();
            }
        }

        else
        {
            if(walkingSFX.isPlaying)
            {
                walkingSFX.Stop();
            }
        }
    }

    //functionality not in game yet
/*    public void ResetPlayer()
    {
        Debug.Log("ResetPlayer");
        rb.velocity = Vector3.zero;
        xRotation = 0;
        yRotation = 0;
        polaroidView = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (walkingSFX.isPlaying)
        {
            walkingSFX.Stop();
        }
    }*/
}