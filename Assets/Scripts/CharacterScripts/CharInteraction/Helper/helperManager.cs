using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helperManager : MonoBehaviour
{
    public static helperManager instance;

    // UI Text
    public Text hintText;

    // Display Duration
    public float displayDuration = 2f;

    // Is Display
    private bool isDisplayingMessage = false;

    // Initialize
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hintText.text = ""; // Initalize text be hidden
    }

    // Show Hint Message
    public void showHint(string message)
    {
        // If Not Display Message
        if (!isDisplayingMessage)
        {
            StartCoroutine(DisplayHint(message));
        }
    }

    // Hide Hint Message
    public void hideHint()
    {
        // Stop the Coroutine
        StopAllCoroutines();

        // Hide hint
        hintText.text = "";
        isDisplayingMessage = false;
    }

    // Display Hint
    private IEnumerator DisplayHint(string message)
    {
        isDisplayingMessage = true;

        // Set Hint Text
        hintText.text = message;

        // Wait displayDuration
        yield return new WaitForSeconds(displayDuration);

        // Hide Text
        hintText.text = "";
        isDisplayingMessage = false;
    }
}
