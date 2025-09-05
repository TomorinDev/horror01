using UnityEngine;
using UnityEngine.UI;

public class CoverImageEffect : MonoBehaviour
{
    public Image coverImage;
    public float speed = 2.0f;
    public float minBrightness = 0.5f;
    public float maxBrightness = 1.0f;

    private float targetBrightness;
    private bool isIncreasing = true;

    void Start()
    {
        if (coverImage == null)
        {
            coverImage = GetComponent<Image>();
        }
        targetBrightness = maxBrightness;
    }

    void Update()
    {
        Color currentColor = coverImage.color;
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, Mathf.PingPong(Time.time * speed, 1));
        currentColor = new Color(brightness, brightness, brightness, currentColor.a);
        coverImage.color = currentColor;
    }
}
