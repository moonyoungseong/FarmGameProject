using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string objective;
}

[System.Serializable]
public class Quest
{
    public int questID;
    public string questName;
    public string description;
    public string giverNPC;
    public string receiverNPC;
    public List<QuestObjective> objectives;
    public List<Reward> reward;
    public int levelRequirement; // optional
}

[System.Serializable]
public class QuestCategory
{
    public List<Quest> Collection;
    public List<Quest> Dialogue;
    public List<Quest> Construction;
    public List<Quest> Delivery;
    public List<Quest> Movement;
}

[System.Serializable]
public class Reward
{
    public int itemID;
    public string icon;
}

[System.Serializable]
public class QuestData
{
    public QuestCategory quests;
}

public class QuestManager : MonoBehaviour
{
    public TextAsset questDataFile;
    public QuestData questData;

    void Start()
    {
        LoadQuestData();
    }

    void LoadQuestData()
    {
        if (questDataFile != null)
        {
            questData = JsonUtility.FromJson<QuestData>(questDataFile.text);
            Debug.Log("퀘스트 데이터 로드 완료!");
        }
        else
        {
            Debug.LogError("퀘스트 JSON 파일이 연결되지 않았습니다!");
        }
    }
}
