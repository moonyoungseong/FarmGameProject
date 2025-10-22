using UnityEngine;

public class OtherCursorManager : MonoBehaviour
{
    public static OtherCursorManager Instance;

    [Header("기본 커서 설정")]
    public Texture2D defaultCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        // 싱글톤 중복 방지
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

    private void Start()
    {
        // 항상 기본 커서로 설정
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }
}
