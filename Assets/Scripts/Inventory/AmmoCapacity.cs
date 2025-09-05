using UnityEngine;

public class AmmoCapacity : MonoBehaviour
{
    public int maxAmmo = 6;       // 最大弹药容量
    public int currentAmmo = 6;  // 当前弹药数量

    // 消耗弹药
    public void ConsumeAmmo(int amount)
    {
        currentAmmo -= amount;
        if (currentAmmo < 0) currentAmmo = 0;
    }

    // 检查弹药是否耗尽
    public bool IsDepleted()
    {
        return currentAmmo <= 0;
    }

    // 增加弹药（拾取时用）
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
    }
}
