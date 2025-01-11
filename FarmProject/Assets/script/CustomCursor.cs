using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D defaultCursor;   // 기본 커서 아이콘
    public Texture2D TomatoIcon1;    // 첫 번째 버튼의 커서 아이콘
    public Texture2D RiceIcon2;    // 두 번째 버튼의 커서 아이콘
    public Texture2D CornIcon3;    // 세 번째 버튼의 커서 아이콘
    public Vector2 hotspot = Vector2.zero; // 커서 핫스팟 위치
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        // 게임 시작 시 기본 커서로 설정
        ResetCursor();
    }

    // 기본 커서로 설정
    public void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }

    // 첫 번째 버튼 클릭 -> 아이콘 1로 변경
    public void SetCursorToIcon1()
    {
        SetCursor(TomatoIcon1);
    }

    // 두 번째 버튼 클릭 -> 아이콘 2로 변경
    public void SetCursorToIcon2()
    {
        SetCursor(RiceIcon2);
    }

    // 세 번째 버튼 클릭 -> 아이콘 3로 변경
    public void SetCursorToIcon3()
    {
        SetCursor(CornIcon3);
    }

    // 커서 변경 함수
    private void SetCursor(Texture2D cursorIcon)
    {
        if (cursorIcon != null)
        {
            Cursor.SetCursor(cursorIcon, hotspot, cursorMode);
        }
        else
        {
            Debug.LogWarning("Cursor icon is not assigned!");
        }
    }
}
