using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IQuestCommand.cs
///
/// 퀘스트 명령 인터페이스 (커맨드)
/// 모든 퀘스트 타입(수집, 대화, 건설, 이동, 전달 등)이 공통적으로
/// 실행/완료/되돌리기 기능을 가지도록 강제
/// </summary>
public interface IQuestCommand
{
    // 퀘스트를 시작하거나 실행할 때 호출되는 메서드
    void Execute();

    // 퀘스트 조건을 충족했을 때 완료 처리하는 메서드
    void CompleteQuest();

    // 퀘스트 실행을 되돌려 NotStarted 상태로 복귀시키는 메서드
    void Undo();
}
