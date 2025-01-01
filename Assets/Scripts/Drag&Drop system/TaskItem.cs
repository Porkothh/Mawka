using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItem : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI counterText;
    
    public string objectName;
    public int currentCount = 0;
    public int requiredCount;



    public void Initialize(string name, int required, Sprite icon)
    {


        objectName = name;
        requiredCount = required;
        iconImage.sprite = icon;
        UpdateCounter();
    }

    public void IncrementCount()
    {
        currentCount++;
        UpdateCounter();
    }

    public void ResetCount()
    {
        currentCount = 0;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        counterText.text = $"{currentCount}/{requiredCount}";
    }

    public bool IsCompleted()
    {
        return currentCount >= requiredCount;
    }
}