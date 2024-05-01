using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this if you're using TextMeshPro

public class ItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemNameText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI itemDescriptionText;
    public CanvasGroup canvasGroup; // Assign in the inspector
    // Start is called before the first frame update
    // public Text itemNameText;
    // public Text itemDescriptionText;
    void Start()
    {
        // Ensure text is initially invisible
        canvasGroup.alpha = 0f;
    }

    public void ShowItemNameAndDescription(string name, string description)
    {
        itemNameText.text = name;
        itemDescriptionText.text = description;

        // Stop any FadeOut coroutine if it's already running
        StopAllCoroutines();
        // Start a new FadeOut coroutine
        StartCoroutine(FadeTextInAndOut());
    }

    public void ShowVictoryMessage(string message)
    {
        itemNameText.text = message;
        itemDescriptionText.text = "Press R to try again or ESC to go back to the main menu!";
        StartCoroutine(FadeTextInAndOut());
    }

    IEnumerator FadeTextInAndOut()
    {
        // Make text visible
        canvasGroup.alpha = 1f;
        // Wait for 4 seconds
        yield return new WaitForSeconds(1f);
        // Gradually make the text transparent again
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2; // Adjust fade duration by changing denominator
            yield return null;
        }
    }
}
