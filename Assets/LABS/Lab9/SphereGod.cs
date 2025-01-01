using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGod : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isGetCloseed = false;

    public void Interact()
    {
        if (!isGetCloseed)
        {
            GetClose();
        }
        else
        {
            Debug.Log("Вы уже повзоимодействовали.");
        }
    }

    public void OnFocused() {
        Debug.Log("Вы вошли в зону.");
    }

    public void OnDefocused() {
        Debug.Log("Вы вышли из зоны.");
    }

    public Transform GetTransform() {
        return transform;
    }

    void GetClose()
    {
        isGetCloseed = true;
        Debug.Log("Вы повзоимодействовали.");
        // Здесь можно добавить анимацию открытия или действия
    }

}
