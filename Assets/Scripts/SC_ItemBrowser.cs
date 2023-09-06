using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TextMeshPro namespace

public class SC_ItemBrowser : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform itemsContainer; 

    private SC_GridSystem gridSystem;
    private SO_Item items;

    private void Start()
    {
        gridSystem = SC_GridSystem.Singleton;

        // Load all items and display them
        var items = gridSystem.GetAllItemsFromResources();
        foreach (SO_Item item in items)
        {
            var btn = Instantiate(buttonPrefab, itemsContainer).GetComponent<Button>();
            btn.gameObject.name = item.name;
            btn.onClick.AddListener(() => gridSystem.SetCurrentItem(item));
            btn.GetComponent<Image>().sprite = item.Sprite;
        }
    }

    public void RotateButtonsBy90Degrees()
    {
        foreach (Transform child in itemsContainer)
        {
            child.Rotate(0, 0, -90);
        }
    }
}
