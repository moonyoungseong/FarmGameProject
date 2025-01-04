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
public class NPCDialogues // NPC°´Ã¼ ¼ø¼­´ë·Î
{
    public NPCDialogue NPC1; // µ¿¹°»çÀ°»ç
    public NPCDialogue NPC2; // ¾çºÀ¾÷ÀÚ
    public NPCDialogue NPC3; // »óÁ¡
    public NPCDialogue NPC4; // ½Ò³óºÎ
    public NPCDialogue NPC5; // Åä¸¶Åä³óºÎ
    public NPCDialogue NPC6; // ¿Á¼ö¼ö³óºÎ
    public NPCDialogue NPC7; // Åä³¢ÁÖ¹Î
    public NPCDialogue NPC8; // ¸¶À»ÀÌÀå
    public NPCDialogue NPC9; // ÆÇ´Ù³óºÎ
}
