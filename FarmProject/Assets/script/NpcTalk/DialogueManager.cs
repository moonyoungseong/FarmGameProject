using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public NPCDialogues npcDialogues;
    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private string currentSpeaker = "";
    private string currentText = "";
    private Dialogue[] currentDialogues;  // ���� ���� ���� NPC�� ��ȭ ���

    public Text dialogueText; // ��ȭ ������ ǥ���� �ؽ�Ʈ UI ���

    void Start()
    {
        TextAsset json = Resources.Load<TextAsset>("npc_dialogues");
        if (json != null)
        {
            npcDialogues = JsonUtility.FromJson<NPCDialogues>(json.ToString());
        }
        else
        {
            Debug.LogError("npc_dialogues.json ������ ã�� �� �����ϴ�.");
        }
    }

    public void StartDialogue(string npcName)
    {
        switch (npcName)
        {
            case "NPC1":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC1.dialogues;
                ShowNextDialogue();
                break;
            case "NPC2":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC2.dialogues;
                ShowNextDialogue();
                break;
            case "NPC3":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC3.dialogues;
                ShowNextDialogue();
                break;
            case "NPC4":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC4.dialogues;
                ShowNextDialogue();
                break;
            case "NPC5":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC5.dialogues;
                ShowNextDialogue();
                break;
            case "NPC6":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC6.dialogues;
                ShowNextDialogue();
                break;
            case "NPC7":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC7.dialogues;
                ShowNextDialogue();
                break;
            case "NPC8":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC8.dialogues;
                ShowNextDialogue();
                break;
            case "NPC9":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC9.dialogues;
                ShowNextDialogue();
                break;

                isDialogueActive = true;
    }

    void ShowNextDialogue()
    {
        if (dialogueIndex < currentDialogues.Length)
        {
            currentSpeaker = currentDialogues[dialogueIndex].speaker;
            currentText = currentDialogues[dialogueIndex].text;

            dialogueText.text = $"{currentSpeaker}: {currentText}";  // ��ȭ UI�� ǥ��
            dialogueIndex++;
        }
        else
        {
            isDialogueActive = false; // ��ȭ ����
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))  // EŰ�� ��ȭ ����
        {
            ShowNextDialogue();  // ���� ��ȭ ����
        }
    }
}
