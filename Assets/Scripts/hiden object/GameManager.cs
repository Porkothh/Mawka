using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI endText;
    public GameObject endTextPanel;
    public GameObject To3x3;
    public GameObject TutorPanel;
    
    [System.Serializable]
    public class ItemToFind
    {
        public string itemName;
        public int amount;
    }
    
    [Header("UI References")]
    public TextMeshProUGUI objectivesText;
    public TextMeshProUGUI timerText;
    public Transform objectivesContainer; // Контейнер для целей (например, VerticalLayoutGroup)


    [Header("UI Settings")]
    public Vector2 objectiveContainerSize = new Vector2(200, 80); // Размер контейнера одной цели
    public Vector2 iconSize = new Vector2(80, 80); // Размер иконки
    public float objectiveSpacing = 0f; // Расстояние между элементами
    
    [Header("Game Settings")]
    public List<ItemToFind> itemsToFind = new List<ItemToFind>();
    public float gameDuration = 300f; // 5 минут по умолчанию
    
    private Dictionary<string, int> foundItems = new Dictionary<string, int>();
    private List<HiddenItem> hiddenItems = new List<HiddenItem>();
    private float currentTime;
    private bool isGameActive = true;
    
    public bool IsGameActive => isGameActive;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        hiddenItems.AddRange(FindObjectsOfType<HiddenItem>());
        InitializeLevel();
    }
    
    void Update()
    {
        if (isGameActive)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            
            if (currentTime <= 0)
            {
                GameOver(false);
            }
        }
    }
    
    void InitializeLevel()
    {
        foundItems.Clear();
        currentTime = gameDuration;
        isGameActive = true;
        
        // Очищаем контейнер целей
        foreach (Transform child in objectivesContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Создаем UI элементы для каждого типа предметов
        foreach (var item in itemsToFind)
        {
            foundItems[item.itemName] = 0;
            
            // Находим соответствующий HiddenItem для получения спрайта
            HiddenItem hiddenItem = hiddenItems.Find(x => x.itemName == item.itemName);
            if (hiddenItem != null)
            {
                CreateObjectiveUI(hiddenItem, item.amount);
                hiddenItem.requiredAmount = item.amount;
                hiddenItem.currentFound = 0;
            }
        }
        
        UpdateTimerDisplay();
    }
    
    void CreateObjectiveUI(HiddenItem item, int amount)
    {
        // Создаем контейнер для одной цели
        GameObject objectiveContainer = new GameObject("Objective_" + item.itemName, typeof(RectTransform));
        objectiveContainer.transform.SetParent(objectivesContainer, false);
        
        // Добавляем вертикальный лейаут
        VerticalLayoutGroup layout = objectiveContainer.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 10f;
        // layout.padding = new RectOffset(5, 5, 5, 5);
        
        // Устанавливаем размер контейнера
        RectTransform containerRect = objectiveContainer.GetComponent<RectTransform>();
        containerRect.sizeDelta = objectiveContainerSize;
        
        // Создаем изображение
        GameObject iconObj = new GameObject("Icon", typeof(RectTransform));
        iconObj.transform.SetParent(objectiveContainer.transform, false);
        Image icon = iconObj.AddComponent<Image>();
        icon.sprite = item.GetItemSprite();
        icon.preserveAspect = true;
        
        // Устанавливаем размер иконки
        RectTransform iconRect = iconObj.GetComponent<RectTransform>();
        iconRect.sizeDelta = iconSize;
        
        // Создаем текст
        GameObject textObj = new GameObject("Text", typeof(RectTransform));
        textObj.transform.SetParent(objectiveContainer.transform, false);
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.fontSize = 24;
        text.text = $"0/{amount}";
        
        // Настраиваем размер текстового поля
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(objectiveContainerSize.x - 10, iconSize.y);
        text.alignment = TextAlignmentOptions.Center; // Центрируем текст под иконкой

        // Добавляем в словарь для обновления
        text.gameObject.name = "Counter_" + item.itemName;
    }

    
    public void OnItemFound(HiddenItem item)
    {
        foundItems[item.itemName]++;
        UpdateObjectiveCounter(item.itemName);
        CheckWinCondition();
    }
    
    void UpdateObjectiveCounter(string itemName)
    {
        // Ищем объект с именем Counter_itemName
        Transform objectiveTransform = objectivesContainer.Find("Objective_" + itemName);
        if (objectiveTransform != null)
        {
            TextMeshProUGUI text = objectiveTransform.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                ItemToFind item = itemsToFind.Find(x => x.itemName == itemName);
                if (item != null)
                {
                    text.text = $"{foundItems[itemName]}/{item.amount}";
                }
            }
        }
    }
    
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    void CheckWinCondition()
    {
        bool allFound = true;
        foreach (var item in itemsToFind)
        {
            if (foundItems[item.itemName] < item.amount)
            {
                allFound = false;
                break;
            }
        }
        
        if (allFound)
        {
            GameOver(true);
        }
    }
    
    void GameOver(bool isWin)
    {
        isGameActive = false;
        if (isWin)
        {
            endText.text="Это было вовремя! \nВсего заработано: 75 \n Долг погашен!";
            endTextPanel.SetActive(true);
        }
        else
        {
            endText.text="Из-за опоздания клиенты отказались платить.. \n Весь прогресс утерян, но ничего, ты еще покоришь город!!";
            endTextPanel.SetActive(true);
            // To3x3.SetActive(false);
        }
        
    }

    public void CloseTutor()
    {
        TutorPanel.SetActive(false);
        InitializeLevel();
    }
}