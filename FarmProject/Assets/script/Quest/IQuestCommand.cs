using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestCommand
{
    void Execute();  // 퀘스트 실행 메서드
    void CompleteQuest();
    void Undo();  // 추가
}
