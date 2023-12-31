using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GridCell : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool isOccupied = false;

    public Color DefaultColor = Color.white;
    public Color SelectedColor = Color.red;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(x, y);
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    public void SetDefaultColor()
    {
        spriteRenderer.color = DefaultColor;
    }

    public void SetSelectedColor()
    {
        spriteRenderer.color = SelectedColor;
    }

    private void OnMouseDown()
    {
        SC_GridSystem.Singleton.PlaceItem(x,y);
    }

    private void OnMouseOver()
    {
        SC_GridSystem.Singleton.SetCurrentGrid(this);
        SC_GridSystem.Singleton.PreviewItem(x, y);
        spriteRenderer.sortingOrder = 10;
    }

    private void OnMouseExit()
    {
        if (SC_GridSystem.Singleton.PreviewedItemInstance)
        {
            Destroy(SC_GridSystem.Singleton.PreviewedItemInstance);
        }

        spriteRenderer.sortingOrder = 0;
    }

}
