using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QuestInvoker.cs
///
/// - 커맨드 패턴에서 Invoker 역할을 수행하는 클래스
/// - 현재 설정된 IQuestCommand를 저장하고, Execute/Undo 실행을 담당
/// - 실제 퀘스트 시작, 완료 등의 로직은 Command 구현 클래스가 처리
/// </summary>
public class QuestInvoker : MonoBehaviour
{
    // 현재 실행할 퀘스트 커맨드
    private IQuestCommand currentCommand;

    // 외부에서 퀘스트 명령을 설정
    public void SetQuestCommand(IQuestCommand command) => currentCommand = command;

    // 설정된 퀘스트 명령 실행
    public void ExecuteQuest()
    {
        if (currentCommand != null)
            currentCommand.Execute();
        else
            Debug.LogWarning("실행할 퀘스트 명령이 없습니다!");
    }

    // 설정된 퀘스트 명령 되돌리기
    public void Undo()
    {
        if (currentCommand != null)
            currentCommand.Undo();
        else
            Debug.LogWarning("되돌릴 퀘스트 명령이 없습니다!");
    }
}
