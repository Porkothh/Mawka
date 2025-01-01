using UnityEngine;
using UnityEngine.UI;

public class HiddenItem : MonoBehaviour
{
    public string itemName;
    public int requiredAmount;
    public int currentFound = 0;
    
    private Button button;
    private Image buttonImage;
    
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(OnItemClick);
    }
    
    public Sprite GetItemSprite()
    {
        return buttonImage.sprite;
    }
    
    void OnItemClick()
    {
        if (currentFound < requiredAmount && GameManager.Instance.IsGameActive)
        {
            currentFound++;
            Debug.Log(currentFound);
            GameManager.Instance.OnItemFound(this);
            button.interactable = false;
            
            if (currentFound >= requiredAmount)
            {
                button.interactable = false;
            }
        }
    }
}