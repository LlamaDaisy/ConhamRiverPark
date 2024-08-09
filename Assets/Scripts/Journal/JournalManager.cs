using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    [SerializeField] Image[] ImagesCaptured;

    Dictionary<string, Image> journalPhotos = new Dictionary<string, Image>();
    private List<Slot> slots;

    [SerializeField] GameObject journalPageOne;
    [SerializeField] GameObject journalPageTwo;
    [SerializeField] AudioSource pageTurn;

    private void Start()
    {
        slots = new List<Slot>();   
        foreach (var image in ImagesCaptured) 
        {
            Debug.Log("start here");
            Slot slot = new Slot() {slotImage = image, tag = image.tag, isFilled = false};
            slots.Add(slot);
            Debug.Log(slots.Count());
        }
    }
    /// <summary>
    ///Updates the journal with a new photo, if a slot with the matching tag is found and is empty.
    /// </summary>
    /// <param name="tag">Assign same tag as the POI</param>
    /// <param name="photoSprite">Sprite of the photo to be added</param>
    public void UpdateJournal(string tag, Sprite photoSprite)
    {
        Debug.Log("Update J Here");
        Debug.Log(slots.Count());
 
        foreach(Slot slot in slots)
        {
            
            Debug.Log(slot.tag);
            Debug.Log(tag);
            if (slot.tag == tag && slot.isFilled == false) 
            { 
                slot.slotImage.sprite = photoSprite;
                slot.isFilled = true;
                Debug.Log("Here");
            }
        }


        
    }

    /// <summary>
    /// Turn page left
    /// </summary>
    public void LeftButton()
    {
        journalPageOne.SetActive(true);
        journalPageTwo.SetActive(false);
        pageTurn.Play();
    }

    /// <summary>
    /// Turn page right
    /// </summary>
    public void RightButton()
    {
        journalPageOne.SetActive(false);
        journalPageTwo.SetActive(true);
        pageTurn.Play();
    }
}

/// <summary>
/// Handles the slot in the journal
/// </summary>
public class Slot
{
   public Image slotImage { get; set; }
   public bool isFilled { get; set; }
    
    public string tag { get; set; }

}
