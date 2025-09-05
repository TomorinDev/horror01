using UnityEngine;
using TMPro;

public class TextFadeEffect : MonoBehaviour
{
    public TextMeshProUGUI continueText;
    public float fadeSpeed = 2.0f;

    private bool fadingIn = true;

    void Update()
    {
        Color color = continueText.color;
        if (fadingIn)
        {
            color.a += fadeSpeed * Time.deltaTime;
            if (color.a >= 1.0f) fadingIn = false;
        }
        else
        {
            color.a -= fadeSpeed * Time.deltaTime;
            if (color.a <= 0.3f) fadingIn = true;
        }
        continueText.color = color;
    }
}
