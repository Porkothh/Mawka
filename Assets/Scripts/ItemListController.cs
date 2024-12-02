using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemListController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI itemCountText;
    [SerializeField] private int targetItemCount = 10;

    private int currentItemCount = 0;

    private void Start()
    {
        UpdateItemCountDisplay();
    }

    public void CollectItem()
    {
        currentItemCount++;
        UpdateItemCountDisplay();
    }

    private void UpdateItemCountDisplay()
    {
        itemCountText.text = $"{currentItemCount}/{targetItemCount}";
    }

    public bool IsCollectionComplete()
    {
        return currentItemCount >= targetItemCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
