using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;        
    public Sprite itemIcon;        
    public GameObject itemPrefab;  
}
