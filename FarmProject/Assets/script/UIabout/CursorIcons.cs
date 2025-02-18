using UnityEngine;

public class CursorIcons : MonoBehaviour
{
    public Texture2D TomatoIcon1;
    public Texture2D RiceIcon2;
    public Texture2D CornIcon3;
    public Texture2D FarmTool1;     // ����
    public Texture2D shovel;        // ��
    private Texture2D currentCursor;

    // Ŀ�� ������ ���� �޼����
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

    // Ŀ�� ����
    private void SetCursor(Texture2D cursorIcon)
    {
        currentCursor = cursorIcon;  // currentCursor�� Ŀ�� ����
        Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
    }

    //// ���� Ŀ�� ��������
    //public Texture2D GetCursor(string cursorType)
    //{
    //    // cursorType�� �´� Ŀ���� ��ȯ
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
    //            return null; // �⺻���� null ó��
    //    }
    //}

    // ���� Ŀ�� ��������
    public Texture2D GetCurrentCursor()
    {
        return currentCursor;
    }

    // �⺻ Ŀ���� ���ư���
    public void ResetToDefaultCursor()
    {
        // DefaultCursorManager �ν��Ͻ��� �����Ͽ� �⺻ Ŀ���� ����
        if (DefaultCursorManager.instance != null)
        {
            DefaultCursorManager.instance.ResetCursor();  // �⺻ Ŀ���� ����
        }
        else
        {
            Debug.LogError("DefaultCursorManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }
}
