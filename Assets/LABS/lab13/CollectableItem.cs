using UnityEngine;
using static QuestManager;

public class CollectibleItem : MonoBehaviour, IInteractable
{
    public QuestManager QuestManager;
    public void Interact()
    {
        QuestManager.OnItemCollected();
        Destroy(gameObject);
    }

    public void OnFocused() { }
    public void OnDefocused() { }
    public Transform GetTransform() { return transform; }
}
