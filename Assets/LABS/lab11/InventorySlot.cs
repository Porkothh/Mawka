using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;  // Иконка предмета
    public TextMeshProUGUI itemNameText;  // Ссылка на TextMeshPro элемент для отображения названия

    Item item;  // Предмет, который находится в слоте

    // Добавить предмет в слот
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        
        itemNameText.text = item.itemName;  // Отобразить название предмета
        itemNameText.enabled = true;
    }

    // Очистить слот
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        itemNameText.text = "";
        itemNameText.enabled = false;
    }
}
