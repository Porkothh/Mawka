using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;  // Родительский объект для слотов
    public Inventory inventory;    // Ссылка на инвентарь

    InventorySlot[] slots;  // Массив слотов инвентаря

    void Start()
    {
        
        inventory = FindObjectOfType<Inventory>();
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();  // Получаем все слоты инвентаря
        itemsParent.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateUI();  // Обновляем интерфейс каждую проверку кадра
        if (Input.GetKeyDown(KeyCode.I))
        {
            itemsParent.gameObject.SetActive(!itemsParent.gameObject.activeSelf);
        }

        
        

    }

    // Обновление интерфейса инвентаря
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);  // Заполняем слот предметом
            }
            else
            {
                slots[i].ClearSlot();  // Очищаем слот, если предмета нет
            }
        }
    }
}
