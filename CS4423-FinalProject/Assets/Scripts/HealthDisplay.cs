using UnityEngine;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI coinsText;
    public Creature player; // Reference to the player object
    

    // Update is called once per frame
    void Update()
    {
        // Update the health display each frame
        healthText.text = $"Health: {player.GetCurrentHealth()}/{player.GetMaxHealth()}";
        coinsText.text = "Coins: " + player.GetCoins().ToString();
    }
}