using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Pickup();
    }
    public void Pickup() 
    {
        Debug.Log("Предмет подобран: " + item.itemName);
        Destroy(gameObject);
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void OnFocused()
    {
        
    }
    public void OnDefocused()
    {

    }
}
