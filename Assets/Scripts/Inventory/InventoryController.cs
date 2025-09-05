using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    public List<InventorySlot> slots;
    private Transform dropPoint;
    private int selectedIndex = 0;
    private GameObject currentItemInstance;

    public TextMeshProUGUI inventoryItemWord;
    private Coroutine fadeCoroutine;

    [SerializeField]
    public ItemCameraManager cameraManager;

    void OnEnable()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        FindDropPoint(); // Find drop point for the initial scene
        UpdateSlotHighlight();
        ShowSelectedItemName();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindDropPoint();
    }

    void FindDropPoint()
    {
        dropPoint = GameObject.Find("DropPoint")?.transform;

        if (dropPoint == null)
        {
            Debug.LogError($"Drop Point not found in the scene '{SceneManager.GetActiveScene().name}'! Ensure a GameObject named 'DropPoint' exists.");
        }
    }



    void Update()
    {
        HandlePickup();
        HandleDrop();
        HandleScrollSelection();
        HandleSlotSelectionByKey();
        HandleSelectedItemUse();
    }
     



    void HandleSlotSelectionByKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
    }




    // Handle scroll wheel item selection
    void HandleScrollSelection()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            int newIndex = (selectedIndex + (scroll > 0 ? -1 : 1)) % slots.Count;
            if (newIndex < 0) newIndex += slots.Count;
            SelectSlot(newIndex);
        }
    }

    void SelectSlot(int slotIndex)
    {
        selectedIndex = slotIndex;
        UpdateSelectedItemDisplay(); 
        UpdateSlotHighlight(); 
        ShowSelectedItemName();

        if (currentItemInstance != null)
        {
            InventoryItem selectedItem = slots[selectedIndex].GetItem();
            if (selectedItem != null && selectedItem.itemPrefab.GetComponent<ItemCamera>() != null)
            {
                cameraManager.ShowCameraScreen();
            }
            else
            {
                cameraManager.HideCameraScreen();
            }
        }
        else
        {
            cameraManager.HideCameraScreen();
        }
    }



    
    // Update the display of the currently selected item
    void UpdateSelectedItemDisplay()
    {
        InventorySlot selectedSlot = slots[selectedIndex];

        if (currentItemInstance != null)
        {
            cameraManager.HideCameraScreen();

            Destroy(currentItemInstance);
            currentItemInstance = null;
        }

        if (!selectedSlot.IsEmpty())
        {
            HandleNonEmptySlot(selectedSlot);
        }
    }


    // Update the highlight around selected slot
    void UpdateSlotHighlight()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetHighlight(i == selectedIndex);
        }
    }
    

    // Show Selected Item Name
    private void ShowSelectedItemName()
    {
        InventorySlot selectedSlot = slots[selectedIndex];

        if (!selectedSlot.IsEmpty())
        {
            string itemName = selectedSlot.GetItem().itemName;
            Debug.Log($"Selected item name: {itemName}");
            UpdateInventoryItemWord(itemName ?? "Unnamed Item"); // Update UI
        }
        else
        {
            Debug.Log("Selected slot is empty.");
            UpdateInventoryItemWord("Empty Slot");
        }
    }




    // Handle item pickup
    void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("Main camera not found.");
                return;
            }

            // Perform raycast for item pickup
            Vector3 rayOrigin = mainCamera.transform.position;
            Vector3 rayDirection = mainCamera.transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, 2f))
            {
                InventoryItemPickedUp pickupItem = hit.collider.GetComponent<InventoryItemPickedUp>();
                if (pickupItem != null)
                {
                    BatteryCapacity attachedCapacity = pickupItem.GetComponent<BatteryCapacity>();
                    AddItemToInventory(pickupItem);
                }
            }
        }
    }



    // Add item to inventory
    void AddItemToInventory(InventoryItemPickedUp item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                // Battery
                BatteryCapacity batteryCapacity = item.GetBatteryCapacity();
                AmmoCapacity ammoCapacity = item.GetAmmoCapacity();

                // Add Items
                if (batteryCapacity != null)
                {
                    slots[i].AddItem(item.inventoryItem, batteryCapacity, null); // batteryCapacity
                }
                else if (ammoCapacity != null)
                {
                    slots[i].AddItem(item.inventoryItem, null, ammoCapacity); // ammoCapacity
                }
                else
                {
                    slots[i].AddItem(item.inventoryItem);
                }

                item.gameObject.SetActive(false); // Hide OBJ

                // Select OBJ
                SelectSlot(i);

                UpdateInventoryItemWord($"Picked up: {item.inventoryItem.itemName}");
                return;
            }
        }
    }









    // Set the camera's view
    private void SetViewCam()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            currentItemInstance.transform.SetParent(mainCamera.transform, false);
            currentItemInstance.transform.localPosition = new Vector3(0.2f, -0.3f, 0.2f);
            currentItemInstance.transform.localRotation = Quaternion.identity;
        }
    }

    // Handle logic for a non-empty slot
    private void HandleNonEmptySlot(InventorySlot selectedSlot)
    {
        InventoryItem selectedItem = selectedSlot.GetItem();
        if (selectedItem.itemPrefab != null)
        {
            currentItemInstance = Instantiate(selectedItem.itemPrefab);
            currentItemInstance.name = selectedItem.itemPrefab.name;
            SetViewCam();
        }
    }


void HandleDrop()
{
    if (Input.GetKeyDown(KeyCode.Q))
    {
        InventorySlot slot = slots[selectedIndex];
        if (!slot.IsEmpty())
        {

            cameraManager.HideCameraScreen();


            // 检查并打印槽位 AmmoCapacity 的当前状态
            AmmoCapacity slotAmmoCapacity = slot.GetAmmoCapacity();
   

            if (currentItemInstance != null)
            {
                Destroy(currentItemInstance);
                currentItemInstance = null; // 清空当前物品实例
            }

            // 获取物品信息
            var (itemData, maxPower, currentPower) = slot.RemoveItem();

            // 创建掉落的克隆物体
            if (itemData?.itemPrefab != null)
            {
                GameObject dropObject = Instantiate(itemData.itemPrefab, dropPoint.position, Quaternion.identity);

                // 更新克隆对象上的 AmmoCapacity 信息
                AmmoCapacity dropAmmo = dropObject.GetComponent<AmmoCapacity>();
                if (dropAmmo != null && slotAmmoCapacity != null)
                {
                    dropAmmo.maxAmmo = slotAmmoCapacity.maxAmmo;
                    dropAmmo.currentAmmo = slotAmmoCapacity.currentAmmo; // 传递当前弹药
                    Debug.Log($"[After Drop] Drop Object Ammo: {dropAmmo.currentAmmo}/{dropAmmo.maxAmmo}");
                }
                else if (slotAmmoCapacity != null)
                {
                    dropAmmo = dropObject.AddComponent<AmmoCapacity>();
                    dropAmmo.maxAmmo = slotAmmoCapacity.maxAmmo;
                    dropAmmo.currentAmmo = slotAmmoCapacity.currentAmmo; // 确保覆盖当前状态
                    Debug.Log($"[After Drop] Drop Object Ammo After Adding Component: {dropAmmo.currentAmmo}/{dropAmmo.maxAmmo}");
                }

                // 更新电池容量信息（如果存在）
                if (maxPower.HasValue && currentPower.HasValue)
                {
                    BatteryCapacity dropBattery = dropObject.GetComponent<BatteryCapacity>() ?? dropObject.AddComponent<BatteryCapacity>();
                    dropBattery.maxPower = maxPower.Value;
                    dropBattery.currentPower = currentPower.Value;
                    Debug.Log($"[After Drop] Drop Object Battery: {dropBattery.currentPower}/{dropBattery.maxPower}");
                }
            }
        }
    }
}




    // Remove a specific item from inventory
    public void RemoveItem(string itemName)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem().itemName == itemName)
            {
                slot.RemoveItem(); 
                return;
            }
        }
    }

    // Handle the use of the selected item
    void HandleSelectedItemUse()
    {
        InventorySlot selectedSlot = slots[selectedIndex];
        if (Input.GetButtonDown("Fire1") && !selectedSlot.IsEmpty())
        {
            InventoryItem selectedItem = selectedSlot.GetItem();
            if (selectedItem.itemPrefab != null && currentItemInstance != null)
            {
                IUsable usableItem = currentItemInstance.GetComponent<IUsable>();

                if (usableItem != null)
                {
                    usableItem.Use();
                }
            }
        }
    }

    // Get the first valid battery slot
    public InventorySlot GetFirstAvailableBatterySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.HasBattery())
            {
                return slot;
            }
        }
        return null;
    }

    public InventorySlot GetFirstAvailableAmmoSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.HasAmmo()) // 检查槽位是否有弹药
            {
                return slot;
            }
        }
        return null;
    }


    // Consume battery power
    public void ConsumeBatteryPower(float amount)
    {
        InventorySlot batterySlot = GetFirstAvailableBatterySlot();
        if (batterySlot != null)
        {
            batterySlot.ConsumeBatteryPower(amount);
        }
    }

    public bool HasBattery()
    {
        foreach (var slot in slots)
        {
            if (slot.HasBattery())
            {
                return true;
            }
        }
        return false;
    }

 

    // 更新 UI 上的文字
    private void UpdateInventoryItemWord(string text)
    {
        if (inventoryItemWord == null) return; // 如果未绑定 UI，跳过
        inventoryItemWord.text = text;
        inventoryItemWord.color = new Color(inventoryItemWord.color.r, inventoryItemWord.color.g, inventoryItemWord.color.b, 1f); // 重置透明度

        // 如果已有渐隐效果正在运行，停止它
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // 启动渐隐效果
        fadeCoroutine = StartCoroutine(FadeOutInventoryItemWord());
    }

    // 渐隐效果
    private IEnumerator FadeOutInventoryItemWord()
    {
        yield return new WaitForSeconds(3f); // 等待 3 秒
        float fadeDuration = 1f; // 渐隐时长
        Color startColor = inventoryItemWord.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            inventoryItemWord.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        inventoryItemWord.color = endColor; // 确保完全透明
    }
}
