using UnityEngine;

public class DialogTriggerZoneInf : MonoBehaviour
{
    [Header("Dialog Settings")]
    public string[] dialogLines; // 对话内容
    public DialogManager dialogManager; // 引用DialogManager

    [Header("Trigger Zone Settings")]
    public Vector3 center = Vector3.zero; // 中心点
    public Vector3 size = new Vector3(5, 5, 5); // 范围尺寸

    private bool isPlayerInside = false; // 标记玩家是否在区域内

    private void Update()
    {
        // 获取玩家的位置
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            // 检查玩家是否在区域内
            if (IsPlayerInZone(playerPosition))
            {
                if (!isPlayerInside)
                {
                    TriggerDialog();
                    isPlayerInside = true; // 标记玩家已在区域内
                }
            }
            else
            {
                isPlayerInside = false; // 玩家离开区域后重置标记
            }
        }
    }

    private bool IsPlayerInZone(Vector3 playerPosition)
    {
        // 计算范围的最小和最大边界
        Vector3 min = transform.position + center - size / 2;
        Vector3 max = transform.position + center + size / 2;

        // 判断玩家是否在边界范围内
        return playerPosition.x >= min.x && playerPosition.x <= max.x &&
               playerPosition.y >= min.y && playerPosition.y <= max.y &&
               playerPosition.z >= min.z && playerPosition.z <= max.z;
    }

    private void TriggerDialog()
    {
        if (dialogManager != null && dialogLines.Length > 0)
        {
            dialogManager.StartDialog(dialogLines);
        }
        else
        {
            Debug.LogWarning("DialogManager is not assigned or dialogLines are empty.");
        }
    }

    // 在Scene视图中显示触发区域的可视化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // 半透明绿色
        Gizmos.DrawCube(transform.position + center, size); // 绘制区域范围
    }
}
