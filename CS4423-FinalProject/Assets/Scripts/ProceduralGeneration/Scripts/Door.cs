using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    
    // [SerializeField] private AudioClip coinClip;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer unlockSpriteRenderer;
    private SpriteRenderer lockSpriteRenderer;
    public Color defaultColor = Color.green; // Set this to whatever your default door color is
    public Color cooldownColor = Color.red; // Color when interaction is not available

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        unlockSpriteRenderer = transform.Find("door_open").GetComponent<SpriteRenderer>();
        unlockSpriteRenderer.enabled = true;
        lockSpriteRenderer = transform.Find("door_locked").GetComponent<SpriteRenderer>();
        lockSpriteRenderer.enabled = false; // Start with the lock symbol hidden
        ResetColor(); // Set to default color initially
    }

    public void SetCooldownColor()
    {
        if(spriteRenderer != null)
        {
            // spriteRenderer.color = cooldownColor;
            unlockSpriteRenderer.enabled = false;
            lockSpriteRenderer.enabled = true;
        }
    }

    public void ResetColor()
    {
        if(spriteRenderer != null)
        {
            // spriteRenderer.color = defaultColor;
            unlockSpriteRenderer.enabled = true;
            lockSpriteRenderer.enabled = false;
        }
    }
}
