using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Подбираем предмет
            PickUp();
        }
    }

    private void PickUp()
    {
        // Здесь можно добавить эффекты подбора (звук, частицы и т.д.)
        
        // Уничтожаем объект
        FindObjectOfType<ItemListController>().CollectItem();
        Destroy(gameObject);
    }
}
