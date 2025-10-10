using UnityEngine;

public class QuestInvoker : MonoBehaviour
{
    private IQuestCommand currentCommand;

    public void SetQuestCommand(IQuestCommand command) => currentCommand = command;

    public void ExecuteQuest()
    {
        if (currentCommand != null)
            currentCommand.Execute();
        else
            Debug.LogWarning("������ ����Ʈ ����� �����ϴ�!");
    }

    public void Undo()
    {
        if (currentCommand != null)
            currentCommand.Undo();
        else
            Debug.LogWarning("�ǵ��� ����Ʈ ����� �����ϴ�!");
    }
}

