using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GridSystem/Item")]
public class SO_Item : ScriptableObject
{
    public Sprite Sprite;
    public Vector2Int[] OccupiedSlots; // Origin is bottom left

}

