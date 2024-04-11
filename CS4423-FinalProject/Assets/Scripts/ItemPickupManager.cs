using UnityEngine;
using UnityEngine.UI; // Required for working with UI

public class ItemPickupManager : MonoBehaviour
{
    public Image itemDisplayUI; // Assign this in the Inspector

    // Call this method when the player picks up an item
    public void OnItemPickedUp(Sprite itemSprite)
    {
        if (itemDisplayUI != null)
        {
            itemDisplayUI.sprite = itemSprite; // Set the image to display the item's sprite
            itemDisplayUI.enabled = true; // Ensure the Image component is enabled
        }
    }
}