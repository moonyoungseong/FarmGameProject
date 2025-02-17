using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public static CustomCursor instance;

    public Texture2D defaultCursor;
    public Texture2D TomatoIcon1;
    public Texture2D RiceIcon2;
    public Texture2D CornIcon3;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

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

    private void Start()
    {
        ResetCursor();
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }

    public void SetCursorToIcon1() => SetCursor(TomatoIcon1);
    public void SetCursorToIcon2() => SetCursor(RiceIcon2);
    public void SetCursorToIcon3() => SetCursor(CornIcon3);

    private void SetCursor(Texture2D cursorIcon)
    {
        Cursor.SetCursor(cursorIcon ?? defaultCursor, hotspot, cursorMode);
    }
}
