using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questProgressText;
    public static QuestManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip questCompleteSound;


    // public Timer timerScript;

     public void StartQuest(int questId)
    {
        Quest quest = quests.Find(q => q.questId == questId);
        if (quest != null)
        {
            quest.isActive = true;
            UpdateQuestUI();
        }
    }
    
    public void CompleteQuest(Quest quest)
    {
        quest.isActive = false;
        audioSource.PlayOneShot(questCompleteSound);
        GameManagerLabs.Instance.OnQuestComplete(quest);
        UpdateQuestUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.goal.currentAmount = 0; 
        }
        UpdateQuestUI(); // Обновляем UI после обнуления

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddQuest(Quest quest) 
    {
        quests.Add(quest);
        UpdateQuestUI(); // Обновляем UI после добавления квеста
    }

     public void OnItemCollected()
    {
        Quest activeQuest = quests.Find(q => q.isActive && q.goal.goalType == QuestGoal.GoalType.Gather);
        if (activeQuest != null)
        {
            activeQuest.goal.currentAmount++;
            UpdateQuestUI();

            if (activeQuest.goal.IsReached())
            {
                CompleteQuest(activeQuest);
            }
        }
    }

     public void UpdateQuestUI()
    {
        Quest activeQuest = quests.Find(q => q.isActive && q.goal.goalType == QuestGoal.GoalType.Gather);
        if (activeQuest != null)
        {
            questTitleText.text = activeQuest.title;
            questDescriptionText.text = activeQuest.description;
            questProgressText.text = "Собрано предметов: " + activeQuest.goal.currentAmount + "/" + activeQuest.goal.requiredAmount;
        }
        else
        {
            // Скрыть UI квеста, если нет активного квеста
            questTitleText.text = "";
            questDescriptionText.text = "";
            questProgressText.text = ""; 
        }
    }
    }


   


