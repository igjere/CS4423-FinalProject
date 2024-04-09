using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* public class BookShelf : MonoBehaviour {
    public ItemDatabase itemDatabase;
    private Item currentItem;

    void Start() {
        if (itemDatabase.availableItemIDs.Count > 0) {
            int randomIndex = Random.Range(0, itemDatabase.availableItemIDs.Count);
            int itemID = itemDatabase.availableItemIDs[randomIndex];
            currentItem = itemDatabase.items.Find(item => item.itemID == itemID);
            
            itemDatabase.RemoveItemByID(itemID); // Remove the picked item's ID

            Transform itemSpriteTransform = transform.Find("ItemSprite");
            if (itemSpriteTransform != null) {
                SpriteRenderer itemSpriteRenderer = itemSpriteTransform.GetComponent<SpriteRenderer>();
                if (itemSpriteRenderer != null) {
                    itemSpriteRenderer.sprite = currentItem.itemSprite;
                }
            }
        }
    }

    public Item GetItem() {
        return currentItem;
    }
} */
public class BookShelf : MonoBehaviour {
    public ItemDatabase itemDatabase;
    private Item currentItem;

    void Start() {
        // Calculate the total weight
        float totalWeight = itemDatabase.items.Where(item => itemDatabase.availableItemIDs.Contains(item.itemID))
                                               .Sum(item => item.weight);

        // Randomly select a weight threshold
        float randomWeightThreshold = Random.Range(0, totalWeight);
        float runningWeight = 0f;

        // Determine which item corresponds to the random threshold
        foreach (var itemId in itemDatabase.availableItemIDs) {
            Item item = itemDatabase.items.Find(i => i.itemID == itemId);
            runningWeight += item.weight;
            if (randomWeightThreshold <= runningWeight) {
                currentItem = item;
                break; // Found the selected item
            }
        }

        // Remove the selected item's ID from availableItemIDs
        itemDatabase.availableItemIDs.Remove(currentItem.itemID);

        // Apply the item sprite to the child GameObject's SpriteRenderer
        Transform itemSpriteTransform = transform.Find("ItemSprite");
        if (itemSpriteTransform != null) {
            SpriteRenderer itemSpriteRenderer = itemSpriteTransform.GetComponent<SpriteRenderer>();
            if (itemSpriteRenderer != null) {
                itemSpriteRenderer.sprite = currentItem.itemSprite;
            }
        }
    }
    public Item GetItem() {
        return currentItem;
    }
}
