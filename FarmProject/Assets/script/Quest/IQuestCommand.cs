using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestCommand
{
    void Execute();  // ����Ʈ ���� �޼���
    void CompleteQuest();
    void Undo();  // �߰�
}
