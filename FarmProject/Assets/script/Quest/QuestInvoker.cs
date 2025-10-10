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
            Debug.LogWarning("실행할 퀘스트 명령이 없습니다!");
    }

    public void Undo()
    {
        if (currentCommand != null)
            currentCommand.Undo();
        else
            Debug.LogWarning("되돌릴 퀘스트 명령이 없습니다!");
    }
}

