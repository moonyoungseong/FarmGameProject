using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string speaker;
    public string text;
}

[System.Serializable]
public class NPCDialogue
{
    public Dialogue[] dialogues;
}

[System.Serializable]
public class NPCDialogues // NPC��ü �������
{
    public NPCDialogue NPC1; // ����������
    public NPCDialogue NPC2; // �������
    public NPCDialogue NPC3; // ����
    public NPCDialogue NPC4; // �ҳ��
    public NPCDialogue NPC5; // �丶����
    public NPCDialogue NPC6; // ���������
    public NPCDialogue NPC7; // �䳢�ֹ�
    public NPCDialogue NPC8; // ��������
    public NPCDialogue NPC9; // �Ǵٳ��
}
