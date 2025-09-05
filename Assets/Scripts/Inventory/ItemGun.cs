using System.Collections;
using System.Net;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemGun : MonoBehaviour, IUsable
{
    // Public Fields
    public Transform hardpoint; // Muzzle
    public GameObject hitEffect;
    public LineRenderer lineRenderer;

    [Header("Audio")]
    public AudioSource audioSource; // Audio source component
    public AudioClip shootSound; // Sound when shooting
    public AudioClip hitSound; // Sound when hitting target

    [Header("Shooting Settings")]
    public float damage = 25f; // Damage
    public float range = 20f; // Shooting Range
    public float fireRate = 1f; // 1 second interval between shots
    private float nextFireTime = 0f; // Time when the player can shoot again


    [Header("Sound Sphere Settings")]
    public GameObject soundSpherePrefab;
    public float soundSphereMaxRadius = 30f;
    public float soundSphereExpansionSpeed = 15f;

    [Header("UI Elements")]
    public Text ammoText;


    public float Ammo
    {
        get => GetAmmoFromInventory();
        set => Debug.LogWarning("Setting Ammo is not allowed directly.");
    }

    public void Use()
    {
        // 检查是否允许开枪
        if (Time.time < nextFireTime)
        {
            return; // 冷却时间未到
        }

        var inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogWarning("InventoryController not found!");
            return;
        }

        // 查找物品栏中的弹药槽位
        var ammoSlot = inventoryController.GetFirstAvailableAmmoSlot();
        if (ammoSlot == null || ammoSlot.GetAmmoCapacity()?.IsDepleted() == true)
        {
            Debug.Log("Out of ammo!");
            return; // 没有弹药，不能开枪
        }

        // 消耗弹药
        ammoSlot.ConsumeAmmo(1);
        var currentAmmo = ammoSlot.GetAmmoCapacity()?.currentAmmo ?? 0;
    Debug.Log($"Current Ammo After Firing: {currentAmmo}");

    nextFireTime = Time.time + fireRate;

    // 执行射击逻辑
    StartCoroutine(Shoot());

    }

    // 获取物品栏中的弹药数量
    private float GetAmmoFromInventory()
    {
        var inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogWarning("InventoryController not found!");
            return 0;
        }

        var ammoSlot = inventoryController.GetFirstAvailableAmmoSlot();
        return ammoSlot?.GetAmmoCapacity()?.currentAmmo ?? 0;
    }

    // 更新弹药 UI
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.enabled = true;
            ammoText.text = $"Ammo: {GetAmmoFromInventory()}";
        }
    }

    private IEnumerator Shoot()
    {
        PlayShootSound();
        Vector3 endPoint = hardpoint.position + hardpoint.forward * range;
        HandleRaycast(endPoint);
        yield return lineRendererHandler(endPoint);
        CreateSoundSphere();
    }



    void CreateSoundSphere()
    {

        GameObject soundSphereInstance = Instantiate(soundSpherePrefab, hardpoint.position, Quaternion.identity);

        SoundSphere soundSphereScript = soundSphereInstance.GetComponent<SoundSphere>();
        soundSphereScript.maxRadius = soundSphereMaxRadius; 
        soundSphereScript.expansionSpeed = soundSphereExpansionSpeed; 
    }


    // Line Renderer Handler
    private IEnumerator lineRendererHandler(Vector3 endPoint)
    {

        // Line Renderer Start and End Point
        lineRenderer.SetPosition(0, hardpoint.position);
        lineRenderer.SetPosition(1, endPoint);

        // Enable
        lineRenderer.enabled = true;

        // Wait 0.1s
        yield return new WaitForSeconds(0.1f);

        // Disable
        lineRenderer.enabled = false;
    }

    // Handle Raycast
    private void HandleRaycast(Vector3 endPoint)
    {
        // Raycast
        RaycastHit hit;
        if (Physics.Raycast(hardpoint.position, hardpoint.forward, out hit, range))
        {
            endPoint = hit.point;

            // Play Hit Effect
            playHitEffect(hit);

            // Target Taken Damage
            targetTakenDamage(hit);
        }
        else
        {
            Debug.Log("Not Hit");
        }
    }


    // Target Taken Damage
    private void targetTakenDamage(RaycastHit hit)
    {
        // Check target
        Damageable target = hit.transform.GetComponent<Damageable>(); // Get Component
        EnemyAI enemyAI = hit.transform.GetComponent<EnemyAI>();

        // Damageable Target
        if (target != null)
        {
            Debug.Log("Hit Target");
            target.TakeDamage(damage); // Target Taken Damage


        }

        // EnemyAI Target
        if (enemyAI != null)
        {
            Debug.Log("Hit Enemy");
            enemyAI.TakeDamage(damage);

            // Play Hit Sound
            PlayHitSound();

            // Stun
            enemyAI.Stun();
        }
    }

    // Play Hit Effect
    private void playHitEffect(RaycastHit hit)
    {
        // If has hitEffect setup
        if (hitEffect != null)
        {
            Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    // Play Shoot Sound
    private void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    // Play Hit Sound
    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}