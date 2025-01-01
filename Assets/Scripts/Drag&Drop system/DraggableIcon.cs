using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Добавлен для работы с Image

public class DraggableIcon : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectPrefab;
    public Vector2Int objectSize = new Vector2Int(1, 1);
    // Удалено поле public Sprite icon, теперь будем использовать Image компонент

    private Image sourceImage;

    void Awake()
    {
        sourceImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        GameObject spawnedObject = Instantiate(objectPrefab, mouseWorldPos, Quaternion.identity);
        spawnedObject.transform.localScale = new Vector3(850f, 850f, 850f);
        spawnedObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
        
        Draggable3DObject draggableComponent = spawnedObject.AddComponent<Draggable3DObject>();
        draggableComponent.size = objectSize;
        draggableComponent.SetBaseObjectName(gameObject.name);
        draggableComponent.StartDragging();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        
        return Vector3.zero;
    }

    public Sprite GetIcon()
    {
        return sourceImage.sprite;
    }
}