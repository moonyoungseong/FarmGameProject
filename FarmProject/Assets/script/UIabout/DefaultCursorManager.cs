using UnityEngine;

public class DefaultCursorManager : MonoBehaviour
{
    public static DefaultCursorManager instance;

    public Texture2D defaultCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private Texture2D currentCursor; // 현재 적용된 커서 추적

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 이 객체 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 객체 파괴
        }
    }

    private void Start()
    {
        ResetCursor();  // 시작할 때 기본 커서로 설정
    }

    // 기본 커서로 리셋
    public void ResetCursor()
    {
        SetCursor(defaultCursor);
    }

    // 커서를 변경하는 메서드 (public으로 접근 가능하게 변경)
    public void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;
        Cursor.SetCursor(cursorIcon ?? defaultCursor, hotspot, cursorMode);
    }

    // 현재 커서 반환
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }
}
