using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject {
    public List<Item> items = new List<Item>();
    [HideInInspector]
    public List<int> availableItemIDs = new List<int>();

    void OnEnable() {
        // Initialize the list with all item IDs when the game starts
        availableItemIDs.Clear();
        for (int i = 0; i < items.Count; i++) {
            availableItemIDs.Add(items[i].itemID); // Assuming itemID is correctly set from 0 to items.Count-1
        }
    }

    public void RemoveItemByID(int itemID) {
        availableItemIDs.Remove(itemID);
    }
}