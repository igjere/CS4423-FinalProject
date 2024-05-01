using System.Collections;
using System.Collections.Generic;
using UnityEngine; // This line is essential for accessing Unity-specific classes like Sprite.


[System.Serializable]
public class Item {
    [Header("Item Details")]
    public string itemName;
    public int itemID;
    public string description;
    public Sprite itemSprite; // Sprite to display on the bookshelf
    public int quality;
    [Header("Spawn Weight")]
    public float weight = 1f; // Default weight is 1
    [Header("Player Attributes")]
    public float healthEffect;
    public int speedEffect;
    public bool flight;
    public Sprite playerSprite;
    [Header("Paper Attributes")]
    public float damageEffect;
    public float damageMultiplier = 1f;
    public float paperRateEffect;
    public float rangeEffect;
    public float paperSpeedEffect;
    public float projectileSize = 1f;
    public bool spectral;
    public bool honing;
    public Sprite projectileSprite;

    public InventoryDisplay inventoryDisplay;

    public void ApplyEffect(Creature player) {
        Debug.Log($"Applying effect of {itemName} to the player.");

        // Modify player attributes based on the item's effects
        if (healthEffect > 0) {
            player.IncreaseMaxHealth(healthEffect); // Cast to int if necessary, or change healthEffect to int
        } else if (healthEffect < 0) {
            player.DecreaseMaxHealth(-healthEffect); 
        }
        player.speed += this.speedEffect; // Ensure Creature has a public property or method to modify speed
        if (this.flight) {
            // Implement flight logic, possibly toggling a 'canFly' boolean in Creature
        }

        ProjectileThrower thrower = player.GetComponent<ProjectileThrower>();
        if (thrower != null)
        {
            thrower.UpdateProjectileAttributes(
                this.damageEffect + thrower.damage * this.damageMultiplier, // Example calculation
                thrower.fireRate + this.paperRateEffect,
                this.rangeEffect + thrower.paperRange,
                this.paperSpeedEffect + thrower.speed,
                this.spectral,
                this.honing,
                this.projectileSize
            );
        }
    }
}
