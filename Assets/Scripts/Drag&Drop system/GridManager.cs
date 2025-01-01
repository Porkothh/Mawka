using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public Transform[,] gridPoints = new Transform[3,3];
    public List<Vector2Int> occupiedPoints = new List<Vector2Int>();
    public static List<GridPoint> allGridPoints = new List<GridPoint>();
    public TaskManager TaskManager;


    private void Start()
    {
        // Находим и сохраняем все точки базового грида
        Transform gridPointsParent = transform.Find("GridPoints");
        int pointIndex = 0;
        
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                gridPoints[x,y] = gridPointsParent.GetChild(pointIndex);
                var gridPoint = gridPoints[x,y].gameObject.AddComponent<GridPoint>();
                allGridPoints.Add(gridPoint);

                  // Проверяем имя точки и помечаем как занятую если это TablePoint
                string pointName = gridPoints[x,y].name;
                if (pointName == "TablePoint" || 
                    pointName == "TablePoint (1)" || 
                    pointName == "TablePoint (2)")
                {
                    gridPoint.isOccupied = true;
                    occupiedPoints.Add(new Vector2Int(x, y));
                }

                pointIndex++;
            }
        }
    }

    public static void RegisterGridPoint(GridPoint point)
    {
        if (!allGridPoints.Contains(point))
        {
            allGridPoints.Add(point);
        }
    }

    public static void UnregisterGridPoint(GridPoint point)
    {
        allGridPoints.Remove(point);
    }

    public static GridPoint GetClosestAvailablePoint(Vector3 position)
    {
        GridPoint closest = null;
        float minDistance = 200f;

        foreach (var point in allGridPoints)
        {
            if (!point.isOccupied)
            {
                float distance = Vector3.Distance(position, point.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = point;
                }
            }
        }

        return closest;
    }

    public bool CanPlaceObject(Vector2Int position, Vector2Int size)
    {
        // Проверяем, можно ли разместить объект заданного размера в указанной позиции
        for (int x = position.x; x < position.x + size.x; x++)
        {
            for (int y = position.y; y < position.y + size.y; y++)
            {
                if (x >= 3 || y >= 3 || IsPointOccupied(new Vector2Int(x, y)))
                    return false;
            }
        }
        return true;
    }

public void OccupyPoints(Vector2Int position, Vector2Int size)
{
    for (int x = position.x; x < position.x + size.x; x++)
    {
        for (int y = position.y; y < position.y + size.y; y++)
        {
            occupiedPoints.Add(new Vector2Int(x, y));
        }
    }
    // Убедитесь, что вложенные точки не помечаются как занятые
    var container = GetComponent<GridPointsContainer>();
    if (container != null)
    {
        foreach (var point in container.GetComponentsInChildren<GridPoint>())
        {
            point.isOccupied = false; // Сбрасываем состояние вложенных точек
        }
    }
}

    private bool IsPointOccupied(Vector2Int point)
    {
        return occupiedPoints.Contains(point);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return gridPoints[gridPosition.x, gridPosition.y].position;
    }

public void ClearTable()
{
    // Находим все объекты с компонентом Draggable3DObject и удаляем их
    var placedObjects = FindObjectsOfType<Draggable3DObject>();
    foreach (var obj in placedObjects)
    {
        Destroy(obj.gameObject);
    }

    // Очищаем список занятых точек
    occupiedPoints.Clear();

    // Сбрасываем состояние всех точек
    foreach (var point in allGridPoints)
    {
        if (point != null)
        {
            point.isOccupied = false;
        }
    }

    // Заново помечаем TablePoints как занятые
    for (int x = 0; x < 3; x++)
    {
        for (int y = 0; y < 3; y++)
        {
            string pointName = gridPoints[x,y].name;
            if (pointName == "TablePoint" || 
                pointName == "TablePoint (1)" || 
                pointName == "TablePoint (2)")
            {
                GridPoint gridPoint = gridPoints[x,y].GetComponent<GridPoint>();
                if (gridPoint != null)
                {
                    gridPoint.isOccupied = true;
                    occupiedPoints.Add(new Vector2Int(x, y));
                }
            }
        }
    }
}

    private void OnDestroy()
    {
        // Очищаем статический список при уничтожении объекта
        allGridPoints.Clear();
    }
}