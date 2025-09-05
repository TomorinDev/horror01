using System.Collections.Generic;
using UnityEngine;

public class RandomEyesEffect : MonoBehaviour
{
    [Header("Eye Settings")]
    public GameObject eyePrefab;
    public int eyeCount = 6;
    public float maxDistance = 2.5f;
    public Vector3 eyeMaxScale = new Vector3(10f, 10f, 10f);
    public float scaleGrowDuration = 1.0f;

    [Header("Color Settings")]
    public Color initialColor = Color.clear;
    public Color finalColor = Color.red;

    [Header("Glow Settings")]
    public float glowIntensity = 1f;
    public Color glowColor = Color.red;

    [Header("Light Settings")]
    public float lightIntensity = 0.2f;
    public float lightRange = 1f; 

    [Header("Centering Settings")]
    [Range(0.3f, 0.7f)] public float horizontalRange = 0.4f;
    [Range(0.3f, 0.7f)] public float verticalRange = 0.4f;

    private List<GameObject> eyeObjects = new List<GameObject>();
    private List<Vector3> eyeOffsets = new List<Vector3>();
    private float startTime;
    private float attackDuration;
    private Transform playerTransform;
    private bool isAttacking = false;

    private void Start()
    {
        playerTransform = Camera.main.transform;

        for (int i = 0; i < eyeCount; i++)
        {
            GameObject eye = Instantiate(eyePrefab, playerTransform.position, Quaternion.identity);
            eye.transform.SetParent(playerTransform);
            eye.transform.localScale = Vector3.zero;
            eye.SetActive(false);
            eyeObjects.Add(eye);

            Light light = eye.AddComponent<Light>();
            light.intensity = lightIntensity;
            light.range = lightRange;
            light.color = glowColor;
        }
    }

    public void Initialize(float attackDuration)
    {
        this.attackDuration = attackDuration;
        startTime = Time.time;
        isAttacking = true;

        eyeOffsets.Clear();
        foreach (var eye in eyeObjects)
        {
            Vector3 randomOffset = GetRandomPointInCenteredView();
            eyeOffsets.Add(randomOffset);
            eye.SetActive(false);
            eye.transform.localScale = Vector3.zero;
        }
    }

    private void Update()
    {
        if (!isAttacking) return;

        float elapsed = Time.time - startTime;
        float eyeRevealInterval = attackDuration / eyeCount;

        for (int i = 0; i < eyeObjects.Count; i++)
        {
            float revealStartTime = i * eyeRevealInterval;
            if (elapsed >= revealStartTime)
            {
                GameObject eye = eyeObjects[i];
                if (!eye.activeSelf)
                {
                    eye.SetActive(true);
                }
                eye.transform.position = playerTransform.position + eyeOffsets[i];
                eye.transform.LookAt(playerTransform);

                float progress = Mathf.Clamp01((elapsed - revealStartTime) / scaleGrowDuration);
                eye.transform.localScale = Vector3.Lerp(Vector3.zero, eyeMaxScale, progress);

                Renderer eyeRenderer = eye.GetComponent<Renderer>();
                eyeRenderer.material.color = Color.Lerp(initialColor, finalColor, progress);
                eyeRenderer.material.SetColor("_EmissionColor", glowColor * (glowIntensity * progress));
                DynamicGI.SetEmissive(eyeRenderer, glowColor * (glowIntensity * progress));
            }
        }

        if (elapsed >= attackDuration)
        {
            isAttacking = false;
        }
    }

    public void ClearEyes()
    {
        foreach (var eye in eyeObjects)
        {
            eye.SetActive(false);
        }
        isAttacking = false;
    }

    private Vector3 GetRandomPointInCenteredView()
    {
        float randomX = Random.Range(0.5f - horizontalRange / 2f, 0.5f + horizontalRange / 2f);
        float randomY = Random.Range(0.5f - verticalRange / 2f, 0.5f + verticalRange / 2f);
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY, maxDistance));
        return worldPoint - playerTransform.position;
    }
}
