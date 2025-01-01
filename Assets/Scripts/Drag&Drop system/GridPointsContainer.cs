using UnityEngine;

public class GridPointsContainer : MonoBehaviour
{
    private GridPoint[] points;

private void OnEnable()
{
    // Находим все точки на объекте
    points = GetComponentsInChildren<GridPoint>();
    
    // Регистрируем их в системе, но не помечаем как занятые
    foreach (var point in points)
    {
        GridManager.RegisterGridPoint(point);
        point.isOccupied = false; // Убедитесь, что точки не помечены как занятые
    }
}

    private void OnDisable()
    {
        // Удаляем точки из системы при деактивации объекта
        foreach (var point in points)
        {
            GridManager.UnregisterGridPoint(point);
        }
    }
} 