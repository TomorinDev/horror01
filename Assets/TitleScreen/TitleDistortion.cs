using UnityEngine;
using TMPro;

public class TitleDistortion : MonoBehaviour
{
    public TextMeshProUGUI gameTitle;
    public float distortionAmount = 1.0f;
    public float distortionSpeed = 2.0f;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * distortionSpeed) * distortionAmount;
        gameTitle.rectTransform.anchoredPosition = new Vector2(offset, gameTitle.rectTransform.anchoredPosition.y);
    }
}
