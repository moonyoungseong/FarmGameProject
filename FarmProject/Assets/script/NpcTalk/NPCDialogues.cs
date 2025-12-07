using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCDialogues.cs  
/// 
/// - NPC 대화 데이터 구조 정의
/// - JSON 파싱을 위한 Serializable 클래스들
/// - Dialogue → NPCDialogue → NPCDialogues 구조  
/// - 각 NPCDialogues의 필드는 게임 내 실제 NPC 순서에 맞춰 배치됨
/// </summary>

[System.Serializable]
public class Dialogue
{
    public string speaker;
    public string text;
}

[System.Serializable]
public class NPCDialogue
{
    // 해당 NPC가 가지고 있는 모든 대사 목록
    public Dialogue[] dialogues;
}

[System.Serializable]
public class NPCDialogues // NPC 객체 순서대로
{
    // 동물사육사
    public NPCDialogue NPC1;

    // 양봉업자 
    public NPCDialogue NPC2;

    // 상점 
    public NPCDialogue NPC3;

    // 쌀농부 
    public NPCDialogue NPC4;

    // 토마토농부
    public NPCDialogue NPC5;

    // 옥수수농부 
    public NPCDialogue NPC6;

    // 토끼주민
    public NPCDialogue NPC7;

    // 마을이장 
    public NPCDialogue NPC8;

    // 판다농부
    public NPCDialogue NPC9;
}
