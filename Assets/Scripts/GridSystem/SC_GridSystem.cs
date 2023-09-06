using UnityEngine;

public class SC_GridSystem : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject gridCellPrefab;
    private SC_GridCell[] grid;
    private Transform itemParent;
    private SC_GridCell currentlySelectedCell;
    public GameObject PreviewedItemInstance { get; private set; }

    [Header("Debug Use")]
    public SO_Item itemToPlace;
    public GameObject itemTemplate;
    public Vector2Int itemPosition;
    public ItemRotation currentRotation = ItemRotation.None;

    #region Singleton
    public static SC_GridSystem Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    private void Start()
    {
        SetUpGrid();
    }

    private void Update()
    {

    }

    #region Grids
    private void SetUpGrid()
    {
        itemParent = GameObject.Find("Items").transform;

        grid = new SC_GridCell[width * height];
        float xOffset = width / 2f - 0.5f;
        float yOffset = height / 2f - 0.5f;

        for (int y = 0; y < height; y++)
        {
            GameObject rowObject = new GameObject($"Row{y}");
            rowObject.transform.SetParent(this.transform);

            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(gridCellPrefab, new Vector3(x - xOffset, y - yOffset, 0),
                                              Quaternion.identity, rowObject.transform);
                grid[GetIndex(x, y)] = cell.GetComponent<SC_GridCell>();
                grid[GetIndex(x, y)].SetPosition(x, y);
                cell.name = $"GridCell({x}, {y})";
            }
        }
    }

    public void SetCurrentGrid(SC_GridCell selectedCell)
    {
        if (currentlySelectedCell != null)
        {
            currentlySelectedCell.SetDefaultColor();
        }
        currentlySelectedCell = selectedCell;
        currentlySelectedCell.SetSelectedColor();
        itemPosition = selectedCell.GetPosition();
    }
    #endregion

    #region Items
    public bool CanPlaceItem(SO_Item item, int startX, int startY, ItemRotation rotation = ItemRotation.None)
    {
        Vector2Int[] rotatedSlots = GetRotatedSlots(item.OccupiedSlots, rotation);
        foreach (var slot in rotatedSlots)
        {
            int x = startX + slot.x;
            int y = startY + slot.y;

            if (x < 0 || x >= width || y < 0 || y >= height || grid[GetIndex(x, y)].IsOccupied)
            {
                return false;
            }
        }
        return true;
    }

    public void PlaceItem(int startX, int startY)
    {
        SO_Item itemSO = itemToPlace;
        ItemRotation rotation = currentRotation;
        if (!CanPlaceItem(itemSO, startX, startY, rotation))
        {
            Debug.LogWarning("Cannot place the item here.");
            return;
        }

        float xOffset = width / 2f - 0.5f;
        float yOffset = height / 2f - 0.5f;

        Vector3 itemWorldPosition = new Vector3(startX - xOffset, startY - yOffset, 0);
        GameObject newItemInstance = Instantiate(itemTemplate, itemWorldPosition, Quaternion.identity);
        newItemInstance.transform.SetParent(itemParent);
        SC_Item itemScript = newItemInstance.GetComponent<SC_Item>();
        if (itemScript != null)
        {
            itemScript.gridItemData = itemSO;
            itemScript.SetItem(itemSO, rotation);
        }

        Vector2Int[] rotatedSlots = GetRotatedSlots(itemSO.OccupiedSlots, rotation);
        foreach (var slot in rotatedSlots)
        {
            int x = startX + slot.x;
            int y = startY + slot.y;
            grid[GetIndex(x, y)].IsOccupied = true;
        }
    }

    public void PreviewItem(int startX, int startY)
    {
        if (PreviewedItemInstance)
        {
            Destroy(PreviewedItemInstance);
        }

        if (!CanPlaceItem(itemToPlace, startX, startY, currentRotation))
        {
            return;  
        }

        float xOffset = width / 2f - 0.5f;
        float yOffset = height / 2f - 0.5f;

        Vector3 itemWorldPosition = new Vector3(startX - xOffset, startY - yOffset, 0);
        PreviewedItemInstance = Instantiate(itemTemplate, itemWorldPosition, Quaternion.identity);
        PreviewedItemInstance.transform.SetParent(itemParent);
    
        SC_Item itemScript = PreviewedItemInstance.GetComponent<SC_Item>();
        if (itemScript != null)
        {
            itemScript.gridItemData = itemToPlace;
            itemScript.SetItem(itemToPlace, currentRotation);
        }
    }


    public void SetCurrentItem(SO_Item selectedItem)
    {
        itemToPlace = selectedItem;
    }

    private Vector2Int[] GetRotatedSlots(Vector2Int[] originalSlots, ItemRotation rotation)
    {
        Vector2Int[] rotatedSlots = (Vector2Int[])originalSlots.Clone();
        switch (rotation)
        {
            case ItemRotation.None:
                break;
            case ItemRotation.CW90:
                RotateSlots90(rotatedSlots);
                break;
            case ItemRotation.CW180:
                RotateSlots90(rotatedSlots);
                RotateSlots90(rotatedSlots);
                break;
            case ItemRotation.CW270:
                RotateSlots90(rotatedSlots);
                RotateSlots90(rotatedSlots);
                RotateSlots90(rotatedSlots);
                break;
        }
        return rotatedSlots;
    }

    private void RotateSlots90(Vector2Int[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Vector2Int(slots[i].y, -slots[i].x);
        }
    }

    public void NextRotation()
    {
        currentRotation = (ItemRotation)(((int)currentRotation + 1) % 4);
    }
    #endregion 

    #region Utilities
    private int GetIndex(int x, int y)
    {
        return y * width + x;
    }

    public SO_Item[] GetAllItemsFromResources()
    {
        return Resources.LoadAll<SO_Item>("Items");
    }
    #endregion

}

public enum ItemRotation
{
    None,
    CW90,
    CW180,
    CW270
}