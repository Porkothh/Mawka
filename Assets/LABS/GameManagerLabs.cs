using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLabs : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    public static GameManagerLabs Instance { get; private set; }
    public DialogueManager dialogueManager;
    public QuestManager questManager;
    public Inventory inventoryManager;

    private Dialogue currentDialogue;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize managers
        dialogueManager = FindObjectOfType<DialogueManager>();
        questManager = FindObjectOfType<QuestManager>();
        inventoryManager = FindObjectOfType<Inventory>();

        if (dialogueManager == null || questManager == null || inventoryManager == null)
        {
            Debug.LogError("Не все менеджеры найдены в сцене!");
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        dialogueManager.StartDialogue(dialogue);
    }

    public void OnDialogueComplete()
    {
        if (currentDialogue != null && currentDialogue.startsQuest)
        {
            questManager.StartQuest(currentDialogue.questId);
        }
    }

    public void OnQuestComplete(Quest quest)
    {
        // Add reward to inventory
        if (quest.rewardItem != null)
        {
            inventoryManager.Add(quest.rewardItem);
        }
    }


    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}