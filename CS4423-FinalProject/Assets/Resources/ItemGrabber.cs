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
    // public Text itemNameText; // Assign in Inspector
    // public Text itemDescriptionText; // Assign in Inspector
    // public TextMeshProUGUI itemNameText; // Reference to the TextMeshProUGUI component
    // public TextMeshProUGUI itemDescriptionText;

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

    /* void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<BookShelf>() != null){
            // player.AddCoins(1);
            onItemPickup.Invoke();
            //myPickupEvent.Invoke(42); //this is how we can pass along data to functions, select the dynamic option if doing so through inspector
            // GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    } */
    /* void OnTriggerEnter2D(Collider2D other){
        BookShelf bookShelf = other.GetComponent<BookShelf>();
        if(bookShelf != null){
            Item item = bookShelf.GetItem();
            // Assuming you have a method to display the item's name and description on the UI
            DisplayItemInfo(item.itemName, item.description);
            item.ApplyEffect(player); // Apply the item's effect on the player
            Destroy(other.gameObject); // Destroy the bookshelf after picking up the item
        }
    } */

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<BookShelf>() != null) {
            // Get the item from the BookShelf
            Item item = other.GetComponent<BookShelf>().GetItem();

            // Apply the item's effect
            item.ApplyEffect(this.player);

            // Additional logic for displaying item name and description
            ShowItemNameAndDescription(item.itemName, item.description);
            // item.SetInventoryDisplay(inventoryDisplay);
            // ItemPickupManager pickupManager = FindObjectOfType<ItemPickupManager>(); // Find the item pickup manager in the scene
            if(firstItem)
            {
                inventoryDisplay.OnItemPickedUp(item.itemSprite); // yourItemSprite is the sprite of the item that was picked up
                firstItem = false;
            }
            else{ 
                inventoryDisplay.AddItem(item.itemSprite);
            }  

            Destroy(other.gameObject); // Assuming you want to remove the bookshelf/item after pickup
        }
    }

    /* void DisplayItemInfo(string name, string description) {
        Debug.Log($"Picked up {name}: {description}");
        // Here you would update your UI elements to show the item's name and description.
    } */
    void ShowItemNameAndDescription(string name, string description) {
        Debug.Log($"Picked up {name}: {description}");
        itemDisplay.ShowItemNameAndDescription(name, description);

        // Optionally, make the UI elements visible if they're hidden by default
    }
}
