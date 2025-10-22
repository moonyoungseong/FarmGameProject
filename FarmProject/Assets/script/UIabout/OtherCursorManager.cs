using UnityEngine;

public class OtherCursorManager : MonoBehaviour
{
    public static OtherCursorManager Instance;

    [Header("�⺻ Ŀ�� ����")]
    public Texture2D defaultCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        // �̱��� �ߺ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // �׻� �⺻ Ŀ���� ����
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }
}
