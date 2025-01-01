using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public GameObject taskItemPrefab;
    public Transform taskPanel;
    
    private Dictionary<string, TaskItem> tasks = new Dictionary<string, TaskItem>();
    public UnityEvent onAllTasksCompleted = new UnityEvent();

    public TextMeshProUGUI endText;
    public GameObject endTextPanel;
    public TextMeshProUGUI CurrentLevelText;
    public GameObject NextLevel;
    public GameObject ToMenuButton;
    public GameObject MiniGame;
    public GameObject TutorPanel;

     private int[,] victoryConditions = new int[,]
    {
        { 1, 1, 0, 0 }, // Уровень 0
        { 1, 0, 2, 0 }, // Уровень 1
        { 1, 1, 1, 0 }, // Уровень 2
        { 1, 1, 1, 1 }, // Уровень 0
        { 1, 0, 2, 1 }, // Уровень 1
        { 1, 1, 1, 1 }, // Уровень 2
    };

    private int currentLevel = 0; // Текущий уровень   

    private void Start()
    {
    
        int levelsCount = victoryConditions.GetLength(0); // Получаем количество уровней
        // Debug.Log($"Уровень {currentLevel}"); // Выводим номер уровня в консоль

        // CurrentLevelText.text = $"Уровень {currentLevel}";

        // Убедимся, что на панели есть Vertical Layout Group
        if (taskPanel.GetComponent<VerticalLayoutGroup>() == null)
        {
            var verticalLayout = taskPanel.gameObject.AddComponent<VerticalLayoutGroup>();
            verticalLayout.spacing = 10; // Расстояние между тасками
            verticalLayout.padding = new RectOffset(10, 10, 50, 10); // Отступы от краев
            verticalLayout.childAlignment = TextAnchor.UpperCenter; // Выравнивание по верхнему краю
            verticalLayout.childControlHeight = false;
            verticalLayout.childControlWidth = false;
        }

        // Находим все DraggableIcon в сцен
        DraggableIcon[] draggableIcons = FindObjectsOfType<DraggableIcon>();
        // Debug.Log($"Found {draggableIcons.Length} DraggableIcon components");
                    
        // Пример создания задач для уровня 0
        for (int i = 0; i < victoryConditions.GetLength(1); i++)
        {
            if (victoryConditions[currentLevel, i] > 0) // Проверяем, нужно ли создавать задачи для этого типа
            {
                // Debug.Log($"Creating task for object type {i} with required count: {victoryConditions[0, i]}");
                Debug.Log(draggableIcons[i].gameObject.name);
                CreateTask(draggableIcons[i].gameObject.name, victoryConditions[0, i], draggableIcons[i].GetIcon());
            }else
            {
                // Debug.Log($"levelsCount {levelsCount}");
            }
        
        }
    }



public void CreateTask(string objectName, int required, Sprite icon)
{
    GameObject taskItemObj = Instantiate(taskItemPrefab, taskPanel);
    
    // Настраиваем размеры для каждого таска
    RectTransform rectTransform = taskItemObj.GetComponent<RectTransform>();
    
    // Устанавливаем якоря в центр
    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);    
    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    
    // Задаем фиксированные размеры
    rectTransform.sizeDelta = new Vector2(300, 20); // Ширина 200, высота 25
    
    // Сбрасываем позицию относительно точки привязки
    rectTransform.anchoredPosition = Vector2.zero;
    
    TaskItem taskItem = taskItemObj.GetComponent<TaskItem>();
    taskItem.Initialize(objectName, required, icon);
    tasks.Add(objectName, taskItem);
}

    public void IncrementTaskCount(GameObject placedObject)
    {
        var draggable = placedObject.GetComponent<Draggable3DObject>();
        string objectName = draggable.baseObjectName;
        if (tasks.ContainsKey(objectName))
        {
            Debug.Log("yes!");
            tasks[objectName].IncrementCount();
        }
        else
        {
            foreach (var task in tasks.Keys)
            {
                Debug.Log($"{task}");
            }
        }

        CheckGameStatus();
    }

    public void CheckGameStatus()
    {
            
        // Сначала проверяем условие поражения
        if (CheckLoseCondition())
        {
            ShowDefeatMessage();
            return; // Выходим из метода, так как игра уже проиграна
        }

        // Затем проверяем условие победы
        bool allTasksCompleted = true;
        foreach (var task in tasks.Values)
        {
            
            if (!task.IsCompleted())
            {
                allTasksCompleted = false;
                break;
            }
        }

        if (allTasksCompleted)
        {
            ShowVictoryMessage();
        }
    }

    private bool CheckLoseCondition()
    {
        // Сначала проверяем, все ли точки заняты
        bool allPointsOccupied = true;

        foreach (var point in GridManager.allGridPoints)
        {
            if (point != null && !point.isOccupied)
            {
                allPointsOccupied = false;
                break;
            }
        }
        Debug.Log($"allPointsOccupied- {allPointsOccupied}");
        // Если все точки заняты и не все задания выполнены, это поражение
        if (allPointsOccupied)
        {
            foreach (var task in tasks.Values)
            {
                Debug.Log(task.IsCompleted());
                if (!task.IsCompleted())
                {
                    return true; // Это поражение
                }
            }
        }

        return false;
    }

    public void ToNextLevel()
    {
        // Удаляем задания текущего уровня с панели
        RemoveTasksFromPanel();

        currentLevel++;

        CreateTasksForCurrentLevel(); // Обновляем список заданий для нового уровня
        onAllTasksCompleted.Invoke();
        endTextPanel.SetActive(false);
    }

    public void ReplayLevel()
    {
        // Удаляем задания текущего уровня с панели
        RemoveTasksFromPanel();
        CreateTasksForCurrentLevel(); // Обновляем список заданий для уровня
        onAllTasksCompleted.Invoke();
        endTextPanel.SetActive(false);
    }

    // Метод для удаления заданий с панели
    private void RemoveTasksFromPanel()
    {
        foreach (var task in tasks.Values)
        {
            Destroy(task.gameObject); // Удаляем объект задания из сцены
        }
        tasks.Clear(); // Очищаем словарь задач
    }

    public void CreateTasksForCurrentLevel()
    {
        CurrentLevelText.text = $"Уровень {currentLevel}";
        tasks.Clear();
        
        DraggableIcon[] draggableIcons = FindObjectsOfType<DraggableIcon>();
        
        for (int i = 0; i < victoryConditions.GetLength(1); i++)
        {
            if (victoryConditions[currentLevel, i] > 0) // Проверяем, нужно ли создавать задачи для этого типа
            {
                Debug.Log($"Creating task for object type {i} with required count: {victoryConditions[currentLevel, i]}");
                CreateTask(draggableIcons[i].gameObject.name, victoryConditions[currentLevel, i], draggableIcons[i].GetIcon());
            }
        }
    }




    private void ShowVictoryMessage()
    {
        endText.text=$"Замурчательно!!\nСумма заказов: {(currentLevel+1)*25}";
        endTextPanel.SetActive(true);
        NextLevel.SetActive(true);
        // ToMenuButton.SetActive(false);
        MiniGame.SetActive(false);
        

        // Проверяем, достигли ли мы последнего уровня
        if (currentLevel >= 2)
        {
            endText.text=$"Все заказы упакованы!\nСумма заказов: {(currentLevel+1)*25}";
            // Скрываем кнопку для перехода на следующий уровень
            NextLevel.SetActive(false); // Скрываем панель, если это необходимо
            ToMenuButton.SetActive(true);
            MiniGame.SetActive(true);
            return; // Выходим из метода, так как больше уровней нет
        }
            
    }

    private void ShowDefeatMessage()
    {
        endText.text="Не каждая из 9 жизней удачна.. В следующий раз получился!";
        endTextPanel.SetActive(true);
        NextLevel.SetActive(false);
        MiniGame.SetActive(false);
    }

    public void ResetTasks()
    {
        foreach (var task in tasks.Values)
        {
            task.ResetCount();
        }
    }

     public void CloseTutor()
    {
        TutorPanel.SetActive(false);
    }
}