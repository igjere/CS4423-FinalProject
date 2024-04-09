using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class AttributesDisplay : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI paperRateText;
    public TextMeshProUGUI paperSpeedText;
    public TextMeshProUGUI paperRangeText;
    public Creature player; // Reference to the player object
    ProjectileThrower thrower;

    void Start()
    {
        // Ensure player is assigned in the inspector or find it dynamically if needed
        if(player != null)
        {
            thrower = player.GetComponent<ProjectileThrower>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(thrower != null)
        {
            // Assuming each of these GetCurrentX methods exist in ProjectileThrower
            // You might need to adjust these lines if your methods are named differently or don't exist yet
            damageText.text = "Dmg: " + thrower.damage.ToString();
            speedText.text = "Speed: " + player.speed.ToString(); // Adjust according to how you access speed in Creature
            paperRateText.text = "PRate: " + thrower.fireRate.ToString();
            paperSpeedText.text = "PSpeed: " + thrower.speed.ToString();
            paperRangeText.text = "PRange: " + thrower.paperRange.ToString();
        }
    }
}