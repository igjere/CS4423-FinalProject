using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryDisplay : MonoBehaviour
{
    public GameObject itemIconPrefab; // Assign in Inspector
    public Transform itemsPanel; // Assign in Inspector
    public Image itemDisplayUI;

    private List<GameObject> displayedItems = new List<GameObject>();

    public void OnItemPickedUp(Sprite itemSprite)
    {
        if (itemDisplayUI != null)
        {
            itemDisplayUI.sprite = itemSprite; // Set the image to display the item's sprite
            Color itemDisplayColor = itemDisplayUI.color;
            itemDisplayColor.a = 0.5f;
            itemDisplayUI.color = itemDisplayColor;
            itemDisplayUI.enabled = true; // Ensure the Image component is enabled
        }
    }
    // Call this method when the player acquires a new item
    public void AddItem(Sprite itemSprite)
    {
            // Instantiate a new item icon GameObject as a child of the items panel
            GameObject itemIcon = Instantiate(itemIconPrefab, itemsPanel);

            // Get the Image component of the new item icon
            Image itemImage = itemIcon.GetComponent<Image>();

            // Set the sprite of the new item icon to the sprite of the acquired item
            itemImage.sprite = itemSprite;

            // Set the transparency of the item icon
            Color imageColor = itemImage.color;
            imageColor.a = 0.5f; // Adjust this value between 0 (fully transparent) and 1 (fully opaque) as desired
            itemImage.color = imageColor;
        

            // Add the new item icon to the list of displayed items to keep track
            displayedItems.Add(itemIcon);
        }

    // Optional: Call this method to remove an item from the display
    public void RemoveItem(GameObject item)
    {
        if (displayedItems.Contains(item))
        {
            displayedItems.Remove(item);
            Destroy(item);
        }
    }
}
