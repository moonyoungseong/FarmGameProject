using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OtherCursorManager.cs
///
/// - 게임 내에서 기본 커서만을 관리하는 간단한 커서 매니저.
/// - 씬이 전환되더라도 기본 커서 설정을 유지, StartScene에서 사용
/// </summary>
public class OtherCursorManager : MonoBehaviour
{
    public static OtherCursorManager Instance;

    [Header("기본 커서 설정")]
    public Texture2D defaultCursor;

    // 커서 클릭 위치 기준점
    public Vector2 hotspot = Vector2.zero;

    public CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 시작 시 무조건 기본 커서로 설정
    private void Start()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }
}
