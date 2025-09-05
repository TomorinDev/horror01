using UnityEngine;
using UnityEngine.UI;

public class ClueSlotController : MonoBehaviour
{
    public Image clueImage;
    public Sprite unknownSprite;
    public Sprite evidenceSprite;
    private bool isRevealed = false;

    // Set unknown when start
    void Start()
    {
        clueImage.sprite = unknownSprite;
    }

    // Change to image
    public void RevealClue()
    {
        if (!isRevealed && clueImage != null && evidenceSprite != null)
        {
            clueImage.sprite = evidenceSprite;
            isRevealed = true; // Reveal
        }
    }

}
