using UnityEngine;

public class CursorIcons : MonoBehaviour
{
    public Texture2D TomatoIcon1;
    public Texture2D RiceIcon2;
    public Texture2D CornIcon3;
    public Texture2D FarmTool1;     // 괭이
    public Texture2D shovel;        // 삽
    private Texture2D currentCursor;

    // 커서 아이콘 설정 메서드들
    public void SetCursorToIcon1()
    {
        SetCursor(TomatoIcon1);
    }

    public void SetCursorToIcon2()
    {
        SetCursor(RiceIcon2);
    }

    public void SetCursorToIcon3()
    {
        SetCursor(CornIcon3);
    }

    public void SetCursorToIcon4()
    {
        SetCursor(FarmTool1);
    }

    public void SetCursorToIcon5()
    {
        SetCursor(shovel);
    }

    // 커서 변경
    private void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;  // currentCursor에 커서 설정
        Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
    }

    //// 현재 커서 가져오기
    //public Texture2D GetCursor(string cursorType)
    //{
    //    // cursorType에 맞는 커서를 반환
    //    switch (cursorType)
    //    {
    //        case "farmTool1":
    //            return FarmTool1;
    //        case "shovel":
    //            return shovel;
    //        case "tomato":
    //            return TomatoIcon1;
    //        case "rice":
    //            return RiceIcon2;
    //        case "corn":
    //            return CornIcon3;
    //        default:
    //            return null; // 기본값은 null 처리
    //    }
    //}

    // 현재 커서 가져오기
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }

    // 기본 커서로 돌아가기
    public void ResetToDefaultCursor()
    {
        // DefaultCursorManager 인스턴스를 참조하여 기본 커서를 리셋
        if (DefaultCursorManager.instance != null)
        {
            DefaultCursorManager.instance.ResetCursor();  // 기본 커서로 리셋
        }
        else
        {
            Debug.LogError("DefaultCursorManager 인스턴스를 찾을 수 없습니다.");
        }
    }
}
