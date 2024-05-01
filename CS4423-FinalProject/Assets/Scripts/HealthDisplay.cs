using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class HealthDisplay : MonoBehaviour
{
    //public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI coinsText;
    public Creature player; // Reference to the player object

    //public Image[] heartSlots; // Array to hold heart images

    public GameObject heartIconPrefab; // Assign a prefab for the heart icons in the inspector
    public Transform heartsContainer;
    private List<Image> heartSlots = new List<Image>();
    public Image heartDisplayUI;

    public Sprite fullHeart; // Sprite for a full heart
    public Sprite halfHeart; // Sprite for a half-full heart
    public Sprite emptyHeart; // Sprite for an empty heart
    
    private void Start() {
        //heartDisplayUI.sprite = fullHeart;
        InitializeHeartSlots();
        UpdateHealthDisplay(player.GetCurrentHealth(), player.GetMaxHealth());
    }

    private void InitializeHeartSlots() {
        if (heartDisplayUI != null) {
            heartSlots.Add(heartDisplayUI); // Add the existing UI element to the list
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health display each frame
        // healthText.text = $"Health: {player.GetCurrentHealth()}/{player.GetMaxHealth()}";
        // coinsText.text = ": " + player.GetCoins().ToString();
        UpdateHealthDisplay(player.GetCurrentHealth(), player.GetMaxHealth());

        UpdateCoinDisplay(player.GetCoins());
    }
    public void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        int numHearts = Mathf.CeilToInt(maxHealth);
        //Debug.Log(numHearts);

        // Ensure there are enough heart slots, adding or removing as necessary
        if (heartSlots.Count < numHearts) {
            for (int i = heartSlots.Count; i < numHearts; i++) {
                GameObject heartIcon = Instantiate(heartIconPrefab, heartsContainer);
                heartSlots.Add(heartIcon.GetComponent<Image>());
                heartIcon.GetComponent<Image>().enabled = true;
            }
        } else if (heartSlots.Count > numHearts) {
            for (int i = numHearts; i < heartSlots.Count; i++) {
                Destroy(heartSlots[i].gameObject);
            }
            heartSlots.RemoveRange(numHearts, heartSlots.Count - numHearts);
        }

        // Update the sprites of each heart slot
        for (int i = 0; i < numHearts; i++) {
            // Debug.Log("i: " + i + "");
            if (currentHealth >= 1f)
            {
                heartSlots[i].sprite = fullHeart;
                currentHealth -= 1f;
            }
            else if (currentHealth >= 0.5f)
            {
                heartSlots[i].sprite = halfHeart;
                currentHealth -= 0.5f;
            }
            else
            {
                heartSlots[i].sprite = emptyHeart;
            }
        }
    }

    void UpdateCoinDisplay(int coins)
    {
        coinsText.text = $": {coins}";
    }
}