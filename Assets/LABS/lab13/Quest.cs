using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string title;
    public string description;
    public int experienceReward;
    public bool isActive;
    public int questId;

    public Item rewardItem;

    public QuestGoal goal; 
}
