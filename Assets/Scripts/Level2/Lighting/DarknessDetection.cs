using UnityEngine;

public class DarknessDetection : MonoBehaviour
{
    public RenderTexture lightDetectionTexture;
    public float darknessThreshold = 0.001f; // Threshold for being in darkness
    public float nearDarknessThreshold = 0.01f; // Threshold for being near darkness

    // Darkness Bools
    [Header("Darkness Bools")]
    public bool isInDarkness;
    public bool isNearDarkness;

    private Texture2D texture2D;




    private void Start()
    {
        // Initialize the Texture2D to match the RenderTexture
        texture2D = new Texture2D(lightDetectionTexture.width, lightDetectionTexture.height, TextureFormat.RGB24, false);
    }


    private void Update()
    {
        CheckDarkness();
    }




    // Check Darkeness
    private void CheckDarkness()
    {
        // Copy the RenderTexture to Texture2D
        RenderTextureToTexture2D();

        float averageBrightness = CalculateAvgBrightness();

        // Set Darkenss Status
        SetDarknessStatus(averageBrightness);

        // Debug Log
        //Debug.Log($"Average Brightness: {averageBrightness}, In Darkness: {isInDarkness}, Near Darkness: {isNearDarkness}");
    }

    // Copy the RenderTexture to Texture2D
    private void RenderTextureToTexture2D()
    {
        RenderTexture.active = lightDetectionTexture;
        texture2D.ReadPixels(new Rect(0, 0, lightDetectionTexture.width, lightDetectionTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
    }




    // Set Darkenss Status
    private void SetDarknessStatus(float averageBrightness)
    {
        // Set darkness status based on thresholds
        isInDarkness = averageBrightness < darknessThreshold;
        isNearDarkness = averageBrightness < nearDarknessThreshold && averageBrightness >= darknessThreshold;
    }

    // Calculate Avg Brightness
    private float CalculateAvgBrightness()
    {
        // Calculate average brightness
        Color[] pixels = texture2D.GetPixels();
        float totalBrightness = 0;

        foreach (Color pixel in pixels)
        {
            totalBrightness += pixel.grayscale; // Use grayscale to get brightness level
        }

        float averageBrightness = totalBrightness / pixels.Length;
        return averageBrightness;
    }
}
