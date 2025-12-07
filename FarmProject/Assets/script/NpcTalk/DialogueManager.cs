using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// DialogueManager.cs
/// NPC 대화를 관리하는 매니저 스크립트.
/// 
/// - npc_dialogues.json 파일을 로드하여 NPC별 대화 데이터를 가져옴
/// - StartDialogue(npcName) 호출 시 해당 NPC의 대화를 시작함
/// - ShowNextDialogue()는 대사를 순서대로 UI에 출력
/// - E 키를 누르면 다음 대사로 넘어감
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public NPCDialogues npcDialogues;
    private int dialogueIndex = 0;
    private bool isDialogueActive = false;
    private string currentSpeaker = "";
    private string currentText = "";
    private Dialogue[] currentDialogues;  // 현재 선택된 NPC의 대사 목록

    public TMP_Text dialogueText; // UI 텍스트에 대사를 출력

    void Start()
    {
        // JSON 파일 로드
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

    /// <summary>
    /// 특정 NPC의 이름을 받아 해당 NPC의 대화를 시작한다.
    /// </summary>
    /// <param name="npcName">대화를 시작할 NPC 이름</param>
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

    // 다음 대사를 UI에 출력
    void ShowNextDialogue()
    {
        if (dialogueIndex < currentDialogues.Length)
        {
            currentSpeaker = currentDialogues[dialogueIndex].speaker;
            currentText = currentDialogues[dialogueIndex].text;

            dialogueText.text = $"{currentSpeaker}: {currentText}";
            dialogueIndex++;
        }
        else
        {
            // 모든 대사를 출력했다면 대화 종료
            isDialogueActive = false;
        }
    }

    void Update()
    {
        // 대화 중일 때 E 키를 누르면 다음 대사 진행
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextDialogue();
        }
    }
}
