using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_RotateButton : MonoBehaviour
{
    private ItemRotation currentRotation = ItemRotation.None;
    private TMP_Text text;
    private Dictionary<ItemRotation, string> rotationText = new Dictionary<ItemRotation, string>()
    {
        { ItemRotation.None, "BottonLeft" },
        { ItemRotation.CW90, "TopLeft" },
        { ItemRotation.CW180, "TopRight" },
        { ItemRotation.CW270, "BottomRight" }
    };  

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        SetText(currentRotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
    }

    private void SetText(ItemRotation rotation)
    {
        text.text = rotationText[rotation];
    }

    public void Rotate()
    {
        currentRotation = (ItemRotation)(((int)currentRotation + 1) % 4);
        SetText(currentRotation);
        SC_GridSystem.Singleton.NextRotation();
        SC_ItemBrowser.Singleton.RotateButtonsBy90Degrees();
    }

}
