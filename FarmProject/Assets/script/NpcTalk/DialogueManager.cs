using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public NPCDialogues npcDialogues;
    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private string currentSpeaker = "";
    private string currentText = "";
    private Dialogue[] currentDialogues;  // 현재 진행 중인 NPC의 대화 목록

    public TMP_Text dialogueText; // 대화 내용을 표시할 텍스트 UI 요소

    void Start()
    {
        TextAsset json = Resources.Load<TextAsset>("npc_dialogues");
        if (json != null)
        {
            npcDialogues = JsonUtility.FromJson<NPCDialogues>(json.ToString());
        }
        else
        {
            Debug.LogError("npc_dialogues.json 파일을 찾을 수 없습니다.");
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
                isDialogueActive = true;
                break;
            case "NPC2":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC2.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC3":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC3.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC4":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC4.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC5":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC5.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC6":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC6.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC7":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC7.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC8":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC8.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
            case "NPC9":
                dialogueIndex = 0;
                currentDialogues = npcDialogues.NPC9.dialogues;
                ShowNextDialogue();
                isDialogueActive = true;
                break;
        }
    }

    void ShowNextDialogue()
    {
        if (dialogueIndex < currentDialogues.Length)
        {
            currentSpeaker = currentDialogues[dialogueIndex].speaker;
            currentText = currentDialogues[dialogueIndex].text;

            dialogueText.text = $"{currentSpeaker}: {currentText}";  // 대화 UI에 표시
            dialogueIndex++;
        }
        else
        {
            isDialogueActive = false; // 대화 종료
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))  // E키로 대화 진행
        {
            ShowNextDialogue();  // 현재 대화 진행
        }
    }
}
