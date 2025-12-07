using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DefaultCursorManager.cs
/// 
/// 게임 전체에서 사용할 "기본 커서"를 관리하는 싱글톤 매니저.
/// 다른 스크립트에서 기본 커서로 되돌리고 싶을 때
/// </summary>
public class DefaultCursorManager : MonoBehaviour
{
    public static DefaultCursorManager instance;

    // 기본 커서
    public Texture2D defaultCursor;

    // 커서 클릭 지점의 기준 위치
    public Vector2 hotspot = Vector2.zero;

    // 커서 모드 Auto
    public CursorMode cursorMode = CursorMode.Auto;

    // 현재 적용된 커서 텍스처
    private Texture2D currentCursor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    // 게임 시작 시 기본 커서를 자동 적용
    private void Start()
    {
        ResetCursor();
    }

    // 기본 커서로 되돌림
    public void ResetCursor()
    {
        SetCursor(defaultCursor);
    }

    /// <summary>
    /// 전달된 커서 아이콘으로 커서를 변경
    /// null이 전달되면 defaultCursor로 대체
    /// </summary>
    public void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;
        Cursor.SetCursor(cursorIcon ?? defaultCursor, hotspot, cursorMode);
    }

    // 현재 적용 중인 커서 텍스처 반환
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }
}
