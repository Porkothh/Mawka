using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Transform focus;  // Цель фокуса
    private IInteractable currentFocus;  // Текущий объект фокуса

    private Animator animator;  // Ссылка на компонент Animator
    private Transform headBone; // Ссылка на трансформ головы

    private Transform rightHandBone; // Ссылка на трансформ правой руки
    public KeyCode interactKey = KeyCode.E; // Клавиша для взаимодействия
    public float handReachDuration = 1.0f; // Время для движения руки к объекту
    private bool isHandMoving = false;  // Флаг для проверки, тянется ли рука

    private Inventory inventory;




    void SetFocus(IInteractable newFocus)
    {
        if (newFocus != currentFocus)  // Если новый фокус отличается от текущего
        {
            if (currentFocus != null)
            {
                currentFocus.OnDefocused();  // Если был предыдущий фокус, вызываем его завершение
            }

            currentFocus = newFocus;
            focus = currentFocus.GetTransform();  // Получаем объект, на котором будет фокус
        }
    }


// Убираем фокус с объекта
    void RemoveFocus()
    {
    if (currentFocus != null)
    {
        currentFocus.OnDefocused();  // Сообщаем объекту о снятии фокуса
    }

    focus = null;
    currentFocus = null;
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (currentFocus != null && focus != null)
        {
            // Персонаж смотрит на объект
            animator.SetLookAtWeight(1f, 0f, 1f, 0.5f, 0.5f);
            animator.SetLookAtPosition(focus.position);

            // Если рука движется к объекту, активируем IK для руки
            if (isHandMoving)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                
                animator.SetIKPosition(AvatarIKGoal.RightHand, focus.position);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            }
        }
        else
        {
            // Отключаем IK, если нет фокуса
            animator.SetLookAtWeight(0f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        }
    }







    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        // rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
        // // Получаем трансформ головы с помощью GetBoneTransform
        // headBone = animator.GetBoneTransform(HumanBodyBones.Head);

    }

    // Update is called once per frame
    void Update()
    {
        if (focus == null && currentFocus == null)
        {
            RemoveFocus();
        }

        if (Input.GetKeyDown(interactKey) && currentFocus != null)
        {
            StartCoroutine(RemoveObject());  // Поднять руку и удалить объект
        }
        if (Input.GetKeyDown(KeyCode.R) && inventory.items.Count > 0)
        {
            inventory.Remove(inventory.items[inventory.items.Count - 1]);
        }
        
    
    }
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            SetFocus(interactable);
        }
    }

    // Обработка выхода из триггера 
    private void OnTriggerExit(Collider other) 
    { 
        // Проверяем, реализует ли объект интерфейс IInteractable
        IInteractable interactable = other.GetComponent<IInteractable>(); 
        if (interactable != null) 
        { 
            RemoveFocus();
        } 
    }

    IEnumerator RemoveObject()
    {
    isHandMoving = true;  // Активируем IK для руки

    yield return new WaitForSeconds(0.0f);  // Ждем, пока рука достигнет объекта

    if (currentFocus != null)
    {
        currentFocus.Interact(); // Выполняем взаимодействие с объектом
        ItemPickUp itemPickUp = currentFocus as ItemPickUp;
        if (itemPickUp != null)
        {
            inventory.Add(itemPickUp.item);
        }
        RemoveFocus(); // Убираем фокус после взаимодействия
    }

    isHandMoving = false;  // Отключаем IK для руки
    }



}
