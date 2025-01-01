using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    public List<Item> items = new List<Item>();  // Список всех предметов в инвентаре
    public int space = 5;  // Ограничение по количеству предметов в инвентаре

    private InventoryUI inventoryUI;

    void Start()
    {
        items.Clear();  // Очищаем список предметов
        
        // Ищем компонент InventoryUI
        inventoryUI = FindObjectOfType<InventoryUI>();

        // Обновляем интерфейс инвентаря, если нашли компонент
        if (inventoryUI != null)
        {
            inventoryUI.UpdateUI();
        }
        else
        {
            Debug.LogError("InventoryUI не найден!");
        }
    }
    
    // Метод для добавления предмета в инвентарь
    public bool Add(Item item)
    {
        // Проверяем, есть ли место в инвентаре
        if (items.Count >= space)
        {
            Debug.Log("Недостаточно места в инвентаре.");
            return false;
        }

        items.Add(item);
        Debug.Log(item.itemName + " добавлен в инвентарь.");
        return true;
    }

    // Метод для удаления предмета из инвентаря
    public void Remove(Item item)
    {
        items.Remove(item);
        Debug.Log(item.itemName + " удалён из инвентаря.");
    }
}
