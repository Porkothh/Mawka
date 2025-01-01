using UnityEngine;
using UnityEngine.UI;

public class Draggable3DObject : MonoBehaviour
{
    private bool isDragging = false;
    public Vector2Int size = new Vector2Int(1, 1);
    private GridManager gridManager;
    private Camera mainCamera;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;

     [SerializeField] public string baseObjectName;

    // Добавляем материалы для разных состояний
    private static readonly Color validColor = new Color(1f, 1f, 1f); // Яркий
    private static readonly Color invalidColor = new Color(5f, 0f, 0f); // Красный
    private static readonly Color neutralColor = new Color(0f, 0f, 0f); // Тусклый

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        mainCamera = Camera.main;
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    public void SetBaseObjectName(string name)
    {
        baseObjectName = name;
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition();
            UpdateVisualState();
            
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = false;
                ResetMaterial();
                TrySnapToGrid();
            }
        }
    }

    private void UpdateVisualState()
    {
        if (IsOverlappingAnyPoint())
        {
            
        }
        else
        {
           
        }
    }

    private bool IsOverlappingAnyPoint()
    {
  
        return false;
    }

    private void SetColor(Color color)
    {
        Material material = meshRenderer.material;
        material.color = color;
    }

    private void ResetMaterial()
    {
        meshRenderer.material = originalMaterial;
    }
    
private void TrySnapToGrid()
{
    if (size.x == 1 && size.y == 1)
    {
        // Для объектов 1x1 используем простой поиск ближайшей точки
        GridPoint closestPoint = GridManager.GetClosestAvailablePoint(transform.position);
        
        if (closestPoint != null)
        {
            transform.position = closestPoint.transform.position;
            closestPoint.isOccupied = true;
            
            // var container = gameObject.AddComponent<GridPointsContainer>();
            // foreach (var point in container.GetComponentsInChildren<GridPoint>())
            // {
            //     point.isOccupied = true;
            // }

            
            FindObjectOfType<TaskManager>().IncrementTaskCount(gameObject);
            // FindObjectOfType<TaskManager>().CheckGameStatus();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    else
    {
        // Для объектов большего размера
        float minDistance = float.MaxValue;
        Vector2Int bestGridPosition = new Vector2Int();

        for (int x = 0; x <= 3 - size.x; x++)
        {
            for (int y = 0; y <= 3 - size.y; y++)
            {
                Vector2Int currentPosition = new Vector2Int(x, y);
                if (gridManager.CanPlaceObject(currentPosition, size))
                {
                    float distance = Vector3.Distance(transform.position, 
                        GetCenterPosition(currentPosition));
                    
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        bestGridPosition = currentPosition;
                    }
                }
            }
        }

        if (minDistance < 150f)
        {
            transform.position = GetCenterPosition(bestGridPosition);
            gridManager.OccupyPoints(bestGridPosition, size);
            
            // Помечаем все затронутые GridPoint как занятые
            for (int x = bestGridPosition.x; x < bestGridPosition.x + size.x; x++)
            {
                for (int y = bestGridPosition.y; y < bestGridPosition.y + size.y; y++)
                {
                    GridPoint point = gridManager.gridPoints[x, y].GetComponent<GridPoint>();
                    if (point != null)
                    {
                        point.isOccupied = true;
                    }
                }
            }
            
            // var container = gameObject.AddComponent<GridPointsContainer>();
            // foreach (var point in container.GetComponentsInChildren<GridPoint>())
            // {
            //     point.isOccupied = true;
            // }

            FindObjectOfType<TaskManager>().IncrementTaskCount(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

    private Vector3 GetCenterPosition(Vector2Int gridPosition)
    {
        Vector3 startPos = gridManager.GetWorldPosition(gridPosition);
        Vector3 endPos = gridManager.GetWorldPosition(new Vector2Int(
            gridPosition.x + size.x - 1,
            gridPosition.y + size.y - 1
        ));
        
        return (startPos + endPos) * 0.5f;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("GameArea"))
        {
            return hit.point;
        }
        
        return transform.position;
    }

    public void StartDragging()
    {
        isDragging = true;
    }
}

