using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal : MonoBehaviour
{
    public enum GoalType {
        Kill,
        Gather
    }

    public GoalType goalType; 
    public int requiredAmount; 
    public int currentAmount; 

    public bool IsReached() {
        return currentAmount >= requiredAmount;
    }
}
