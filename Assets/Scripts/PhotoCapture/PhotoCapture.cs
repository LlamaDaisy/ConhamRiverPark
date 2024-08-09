using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("PhotoTaker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

    [SerializeField] private JournalManager journalManager;
    [SerializeField] private PlayerInteraction PI;
    [SerializeField] private AudioSource photoTaken;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnim;

    [Header ("POI")]
    [SerializeField] GameObject[] POIs;
    POI currentPOI;

    [Header ("Raycast")]
    [SerializeField] GameObject player;
    [SerializeField] Camera polaroidCamera;
    Vector3 originOfRay;
    [SerializeField] LayerMask layerMask;

    private bool viewingPhoto;
    private PlayerMovement playerMovement;


    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckPOI();

        //handles removing photo preview on click or enter photo mode.
        if (viewingPhoto && Input.GetMouseButtonDown(0) || viewingPhoto && Input.GetKeyDown(KeyCode.E))
        {
            RemovePhoto();
        }

        //handles photo capture on click
        else if (playerMovement.polaroidView && Input.GetMouseButtonDown(0))
        {
            DisableCurrentPOI();
            StartCoroutine(CapturePhoto());
            photoTaken.Play();
        }
    }

    /// <summary>
    /// Handles photo capture and display/ preview
    /// </summary>
    /// <returns></returns>
    IEnumerator CapturePhoto()
    {
        //hide camera UI
        cameraUI.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();
        int resWidth = Screen.width;
        int resHeight = Screen.height;

        //handles creating new texture2D from captured image to create unique texture2D's for each capture.
        RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 24);
        polaroidCamera.targetTexture = renderTexture;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        polaroidCamera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        polaroidCamera.targetTexture = null;
        RenderTexture.active = null;

        //handles photo preview
        Sprite photoSprite = Sprite.Create(screenShot, new Rect(0.0f, 0.0f, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f), 100.0f);
        ShowPhoto(photoSprite);
        CapturedObject(photoSprite);
        Destroy(renderTexture);
    }

    /// <summary>
    /// Show current photo in preview
    /// </summary>
    /// <param name="photoSprite"></param>
    void ShowPhoto(Sprite photoSprite)
    {
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        fadingAnim.Play("PhotoFade");
    }

    /// <summary>
    /// Removes current photo
    /// </summary>
    void RemovePhoto()
    {

        if (playerMovement.polaroidView)
        {
            cameraUI.SetActive(true);
        }

        else
        {
            cameraUI.SetActive(false);
        }

        viewingPhoto = false;
        photoFrame.SetActive(false);
    }

    /// <summary>
    /// Updates the journal with the captured image if it a tagged POI
    /// </summary>
    /// <param name="photoSprite">Captured Photo</param>
    void CapturedObject(Sprite photoSprite)
    {
        RaycastHit hit;
        originOfRay = polaroidCamera.transform.position;

        //Get tag of captured object
        if (Physics.Raycast(originOfRay, polaroidCamera.transform.forward, out hit, layerMask))
        {
            string tag = hit.transform.tag;
            Debug.Log(tag);

            //if journal doesnt have a photo of this POI update journal with photo
            if (journalManager != null && !string.IsNullOrEmpty(tag))
            {
                journalManager.UpdateJournal(tag, photoSprite);
                Debug.Log($"Captured photo of: {hit.transform.name} with tag: {tag} and texture index: {photoSprite.name}");
            }
        }
    }

    /// <summary>
    /// Handles outline on POI's
    /// </summary>
    void CheckPOI()
    {
        RaycastHit hit;
        originOfRay = polaroidCamera.transform.position;

        if (Physics.Raycast(originOfRay, polaroidCamera.transform.forward, out hit, layerMask))
        {
            //check if object is in POIs array
            if (IsPOI(hit.collider.gameObject))
            {
                POI newPOI = hit.collider.GetComponent<POI>();

                if (newPOI != null && newPOI != currentPOI)
                {
                    if(currentPOI != null)
                    {
                        currentPOI.DisableOutline();
                    }
                }

                if (newPOI.enabled)
                {
                    SetNewCurrentPOI(newPOI);
                }
                else
                {
                    DisableCurrentPOI();
                }
            }

            else
            {
                DisableCurrentPOI();
            }
        }

        else
        {
            DisableCurrentPOI();
        }

    }

    /// <summary>
    /// Checks if object is in POI array
    /// </summary>
    /// <param name="obj">POI</param>
    /// <returns></returns>
    bool IsPOI(GameObject obj)
    {
        foreach (GameObject poi in POIs)
        {
            if (poi == obj)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Sets a new POI as the current POI and enables its outline.
    /// </summary>
    /// <param name="newPOI">Sets new POI as current</param>
    void SetNewCurrentPOI(POI newPOI)
    {
        currentPOI = newPOI;
        if(currentPOI != null)
        {
            currentPOI.EnableOutline();
        }
    }

    /// <summary>
    /// Remove Outline
    /// </summary>
    void DisableCurrentPOI()
    {
        if (currentPOI != null)
        {
            currentPOI.DisableOutline();
            currentPOI = null;
        }
    }


}