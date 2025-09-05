using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // Item icon
    public Image borderImage; // Border
    public TextMeshProUGUI itemStatusTMP; // Status text (TextMeshPro)
    private InventoryItem currentItem;
    private BatteryCapacity batteryCapacity; // Store the BatteryCapacity for the item
    private AmmoCapacity ammoCapacity; // Store the AmmoCapacity for the item
    
    void Start()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        if (itemStatusTMP != null)
        {
            itemStatusTMP.text = ""; // Set text to empty
            itemStatusTMP.gameObject.SetActive(false); // Initially hidden
        }
        SetHighlight(false);
    }

    public bool IsEmpty() => currentItem == null;

    public void AddItem(InventoryItem item, BatteryCapacity battery = null, AmmoCapacity ammo = null)
    {
        currentItem = item;
        itemImage.sprite = item.itemIcon;
        itemImage.enabled = true;

        // 添加电池逻辑
        if (battery != null)
        {
            batteryCapacity = battery;
            UpdateStatusText($"{battery.currentPower:F2}%"); // 电池显示为百分比
            itemStatusTMP.gameObject.SetActive(true);

        }
        else if (ammo != null) // 添加弹药逻辑
        {
            ammoCapacity = ammo;
            UpdateStatusText($"{ammo.currentAmmo}/{ammo.maxAmmo}"); // 弹药显示为数量
            itemStatusTMP.gameObject.SetActive(true);

        }
        else
        {
            itemStatusTMP.text = "";
            itemStatusTMP.gameObject.SetActive(false);
        }
    }


    public (InventoryItem item, float? maxPower, float? currentPower) RemoveItem()
    {
        // 先保存当前状态
        InventoryItem item = currentItem;
        float? maxPower = null;
        float? currentPower = null;

        if (batteryCapacity != null)
        {
            maxPower = batteryCapacity.maxPower;
            currentPower = batteryCapacity.currentPower;
        }

        // 清除槽位数据
        currentItem = null;
        batteryCapacity = null;
        ammoCapacity = null;

        // 清理 UI
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemStatusTMP.text = "";
        itemStatusTMP.gameObject.SetActive(false);

        // 返回物品和电池状态
        return (item, maxPower, currentPower);
    }



    public InventoryItem GetItem() => currentItem;

    public void SetHighlight(bool isHighlighted)
    {
        borderImage.color = isHighlighted ? Color.yellow : Color.white;
    }

    public bool HasBattery()
    {
        return batteryCapacity != null && !batteryCapacity.IsDepleted();
    }
    public bool HasAmmo()
    {
        return ammoCapacity != null && !ammoCapacity.IsDepleted();
    }

    public AmmoCapacity GetAmmoCapacity()
    {
        return ammoCapacity;
    }

    public BatteryCapacity GetBatteryCapacity()
    {
        return batteryCapacity;
    }

    public void ConsumeBatteryPower(float amount)
    {
        if (batteryCapacity != null)
        {
            batteryCapacity.ConsumePower(amount);
            UpdateStatusText($"{batteryCapacity.currentPower:F2}%"); // Update UI

            if (batteryCapacity.IsDepleted())
            {
                RemoveItem();
            }
        }
    }

    public void ConsumeAmmo(float amount)
    {
        if (ammoCapacity != null)
        {
            ammoCapacity.ConsumeAmmo((int)amount);
            UpdateStatusText($"{ammoCapacity.currentAmmo}/{ammoCapacity.maxAmmo}");
            if (ammoCapacity.IsDepleted())
            {
                RemoveItem(); // 弹药耗尽时移除物品
            }
        }
    }

    private void UpdateStatusText(string status)
    {
        if (itemStatusTMP != null)
        {
            itemStatusTMP.text = status;
            itemStatusTMP.gameObject.SetActive(!string.IsNullOrEmpty(status));
        }
    }


}
