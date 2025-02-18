using UnityEngine;

public class DefaultCursorManager : MonoBehaviour
{
    public static DefaultCursorManager instance;

    public Texture2D defaultCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private Texture2D currentCursor; // ���� ����� Ŀ�� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� �� ��ü ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ߺ� ��ü �ı�
        }
    }

    private void Start()
    {
        ResetCursor();  // ������ �� �⺻ Ŀ���� ����
    }

    // �⺻ Ŀ���� ����
    public void ResetCursor()
    {
        SetCursor(defaultCursor);
    }

    // Ŀ���� �����ϴ� �޼��� (public���� ���� �����ϰ� ����)
    public void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;
        Cursor.SetCursor(cursorIcon ?? defaultCursor, hotspot, cursorMode);
    }

    // ���� Ŀ�� ��ȯ
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }
}
