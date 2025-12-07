using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CursorIcons.cs
///
/// 농사 게임에서 작물 아이콘, 도구 아이콘 등 다양한 커서 이미지를 관리하는 스크립트
/// 각 메서드를 통해 특정 커서 이미지로 즉시 변경
/// DefaultCursorManager를 통해 기본 커서로 되돌림
/// </summary>
public class CursorIcons : MonoBehaviour
{
    // 토마토 아이콘 커서
    public Texture2D TomatoIcon1;
    // 쌀 아이콘 커서
    public Texture2D RiceIcon2;
    // 옥수수 아이콘 커서
    public Texture2D CornIcon3;
    // 괭이 커서 아이콘
    public Texture2D FarmTool1;
    // 삽 커서 아이콘
    public Texture2D shovel;

    // 현재 적용 중인 커서 텍스처>
    private Texture2D currentCursor;

    // 토마토 커서로 변경
    public void SetCursorToIcon1()
    {
        SetCursor(TomatoIcon1);
    }

    // 쌀 커서로 변경
    public void SetCursorToIcon2()
    {
        SetCursor(RiceIcon2);
    }

    // 옥수수 커서로 변경
    public void SetCursorToIcon3()
    {
        SetCursor(CornIcon3);
    }

    // 괭이 커서로 변경
    public void SetCursorToIcon4()
    {
        SetCursor(FarmTool1);
    }

    // 삽 커서로 변경
    public void SetCursorToIcon5()
    {
        SetCursor(shovel);
    }

    // 실제로 커서를 변경하는 내부 메서드
    private void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;
        Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
    }

    // 현재 커서 텍스처를 반환
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }

    // DefaultCursorManager를 호출하여 기본 커서로 되돌림
    public void ResetToDefaultCursor()
    {
        if (DefaultCursorManager.instance != null)
        {
            DefaultCursorManager.instance.ResetCursor();
        }
        else
        {
            Debug.LogError("DefaultCursorManager 인스턴스를 찾을 수 없습니다.");
        }
    }
}
