using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D defaultCursor;   // �⺻ Ŀ�� ������
    public Texture2D TomatoIcon1;    // ù ��° ��ư�� Ŀ�� ������
    public Texture2D RiceIcon2;    // �� ��° ��ư�� Ŀ�� ������
    public Texture2D CornIcon3;    // �� ��° ��ư�� Ŀ�� ������
    public Vector2 hotspot = Vector2.zero; // Ŀ�� �ֽ��� ��ġ
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        // ���� ���� �� �⺻ Ŀ���� ����
        ResetCursor();
    }

    // �⺻ Ŀ���� ����
    public void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }

    // ù ��° ��ư Ŭ�� -> ������ 1�� ����
    public void SetCursorToIcon1()
    {
        SetCursor(TomatoIcon1);
    }

    // �� ��° ��ư Ŭ�� -> ������ 2�� ����
    public void SetCursorToIcon2()
    {
        SetCursor(RiceIcon2);
    }

    // �� ��° ��ư Ŭ�� -> ������ 3�� ����
    public void SetCursorToIcon3()
    {
        SetCursor(CornIcon3);
    }

    // Ŀ�� ���� �Լ�
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
