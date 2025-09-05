using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public void OnMouseEnter()
    {
        // 更改为系统的手型光标
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        // 恢复默认光标
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
