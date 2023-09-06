using UnityEngine;

public class SC_Item : MonoBehaviour
{
    public SO_Item gridItemData;
    private Transform spriteTransform;
    [SerializeField] private Vector2Int[] occupiedSlots;

    private void Awake()
    {
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
    }

    public void SetItem(SO_Item itemData, ItemRotation rotation = ItemRotation.None)
    {
        // make a copy of the occupied slots
        occupiedSlots = new Vector2Int[itemData.OccupiedSlots.Length];
        for (int i = 0; i < itemData.OccupiedSlots.Length; i++)
        {
            occupiedSlots[i] = itemData.OccupiedSlots[i];
        }

        gridItemData = itemData;
        Rotate(rotation); 
        GetComponentInChildren<SpriteRenderer>().sprite = gridItemData.Sprite;

        // Calculate the new center
        Vector2 center = CalculateCenter(occupiedSlots);
        spriteTransform.localPosition = new Vector3(center.x, center.y, 0);
    }

    private Vector2 CalculateCenter(Vector2Int[] occupiedSlots)
    {
        // approach: find the bounding box of the occupied slots, then find the center of that bounding box

        if (occupiedSlots == null || occupiedSlots.Length == 0)
            return Vector2.zero;

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;

        foreach (Vector2Int slot in occupiedSlots)
        {
            if (slot.x < minX) minX = slot.x;
            if (slot.x > maxX) maxX = slot.x;
            if (slot.y < minY) minY = slot.y;
            if (slot.y > maxY) maxY = slot.y;
        }

        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;

        return new Vector2(centerX, centerY);
    }

    public void Rotate(ItemRotation rotation)
    {
        switch (rotation)
        {
            case ItemRotation.None:  // default
                break;  
            case ItemRotation.CW90:
                Rotate90();
                break;
            case ItemRotation.CW180:
                Rotate90();
                Rotate90();
                break;
            case ItemRotation.CW270:
                Rotate90();
                Rotate90();
                Rotate90();
                break;
        }
    }

    private void Rotate90()
    {
        // rotate the occupied slots
        for (int i = 0; i < occupiedSlots.Length; i++)
        {
            occupiedSlots[i] = new Vector2Int(occupiedSlots[i].y, -occupiedSlots[i].x);
        }

        // rotate the sprite
        spriteTransform.Rotate(0, 0, -90);
    }
}
