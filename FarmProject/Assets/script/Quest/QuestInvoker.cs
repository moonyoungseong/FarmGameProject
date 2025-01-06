using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInvoker : MonoBehaviour
{
    private IQuestCommand currentQuestCommand;

    // 퀘스트 명령을 설정하는 메서드
    public void SetQuestCommand(IQuestCommand questCommand)
    {
        currentQuestCommand = questCommand;
    }

    // 퀘스트 실행 메서드
    public void ExecuteQuest()
    {
        if (currentQuestCommand != null)
        {
            currentQuestCommand.Execute();  // 퀘스트 실행
        }
        else
        {
            Debug.LogError("퀘스트 명령이 설정되지 않았습니다!");
        }
    }
}
