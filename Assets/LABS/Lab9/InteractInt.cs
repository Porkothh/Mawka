using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();  // Метод для взаимодействия
    void OnFocused();  // Метод, который вызывается, когда объект находится в фокусе
    void OnDefocused();  // Метод, который вызывается, когда объект выходит из фокуса
    Transform GetTransform();  // Метод для получения позиции объекта
}


