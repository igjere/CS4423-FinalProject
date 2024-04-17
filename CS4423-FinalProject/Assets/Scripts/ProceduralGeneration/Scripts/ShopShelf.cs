using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this if you're using TextMeshPro
using System.Linq;

public class ShopShelf : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public int itemPrice = 4;
    private Item currentItem;

    // Use for displaying the price on the UI
    public GameObject priceTagPrefab;
    public GameObject priceTagInstance;

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
        if (currentItem.quality == 4){
            itemPrice = 8;
        }
        if (currentItem.quality == 3){
            itemPrice = 6;
        }
        // Display the price tag
        if (priceTagPrefab) {
            // priceTagInstance = Instantiate(priceTagPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            TextMeshPro priceText = priceTagInstance.GetComponentInChildren<TextMeshPro>();
            if (priceText) {
                priceText.text = $"¢{itemPrice}";
            }
        }
    }
    public Item GetItem() {
        return currentItem;
    }

    public void Interact(Creature player) {
        if (player.GetCoins() >= itemPrice) {
            currentItem.ApplyEffect(player);
            player.AddCoins(-itemPrice); // Subtract coins
            Destroy(gameObject); // Optionally destroy the shelf after purchase
        } else {
            Debug.Log("Not enough coins!");
        }
    }

    void OnDestroy() {
        if (priceTagInstance) {
            Destroy(priceTagInstance);
        }
    }
}
