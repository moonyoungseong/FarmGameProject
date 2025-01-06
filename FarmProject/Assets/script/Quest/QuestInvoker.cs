using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInvoker : MonoBehaviour
{
    private IQuestCommand currentQuestCommand;

    // ����Ʈ ����� �����ϴ� �޼���
    public void SetQuestCommand(IQuestCommand questCommand)
    {
        currentQuestCommand = questCommand;
    }

    // ����Ʈ ���� �޼���
    public void ExecuteQuest()
    {
        if (currentQuestCommand != null)
        {
            currentQuestCommand.Execute();  // ����Ʈ ����
        }
        else
        {
            Debug.LogError("����Ʈ ����� �������� �ʾҽ��ϴ�!");
        }
    }
}
