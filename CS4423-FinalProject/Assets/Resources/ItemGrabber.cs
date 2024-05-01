using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrabber : MonoBehaviour
{
    public UnityEvent onItemPickup;
    public Creature player;
    public ItemDisplay itemDisplay;
    public InventoryDisplay inventoryDisplay;
    public bool firstItem = true;

    void Start(){
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>();
        }
        onItemPickup.AddListener(PrintPickup);

    }

    void PrintPickup(){
        //player.health++;  //to test if items can affect the player (it works)
        Debug.Log("Picked up item!");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<BookShelf>() != null) {
            Item item = other.GetComponent<BookShelf>().GetItem();

            // Apply the item's effect
            item.ApplyEffect(this.player);

            // Additional logic for displaying item name and description
            ShowItemNameAndDescription(item.itemName, item.description);
            // item.SetInventoryDisplay(inventoryDisplay);
            // ItemPickupManager pickupManager = FindObjectOfType<ItemPickupManager>(); // Find the item pickup manager in the scene
            
            UpdateInventoryDisplay(item);

            Destroy(other.gameObject); 
        }
        else if (other.GetComponent<ShopShelf>() != null){
            ShopShelf shopShelf = other.GetComponent<ShopShelf>();
            bool isPurchased = shopShelf.Interact(player);
            if (isPurchased) {
                Item item = shopShelf.GetItem();
                ShowItemNameAndDescription(item.itemName, item.description);
                UpdateInventoryDisplay(item);
            }
        }
    }

    void UpdateInventoryDisplay(Item item) {
        if (firstItem) {
            inventoryDisplay.OnItemPickedUp(item.itemSprite);
            firstItem = false;
        } else {
            inventoryDisplay.AddItem(item.itemSprite);
        }
    }

    void ShowItemNameAndDescription(string name, string description) {
        Debug.Log($"Picked up {name}: {description}");
        itemDisplay.ShowItemNameAndDescription(name, description);

        // Optionally, make the UI elements visible if they're hidden by default
    }
}
