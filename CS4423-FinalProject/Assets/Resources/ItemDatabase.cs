using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "MyItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject {
    private static ItemDatabase _instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<ItemDatabase>().FirstOrDefault();
            return _instance;
        }
    }
    public List<Item> items = new List<Item>();
    [HideInInspector]
    public List<int> availableItemIDs = new List<int>();
    public void ResetAvailableItems()
    {
        availableItemIDs = items.Select(item => item.itemID).ToList();
    }

    void OnEnable() {
        // Initialize the list with all item IDs when the game starts
        ResetAvailableItems(); 
    }

    public void RemoveItemByID(int itemID) {
        availableItemIDs.Remove(itemID);
    }
}