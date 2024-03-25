using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    
    // [SerializeField] private AudioClip coinClip;
    private SpriteRenderer spriteRenderer;
    public Color defaultColor = Color.green; // Set this to whatever your default door color is
    public Color cooldownColor = Color.red; // Color when interaction is not available

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetCooldownColor(); // Set to default color initially
    }

    public void SetCooldownColor()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = cooldownColor;
        }
    }

    public void ResetColor()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        }
    }
}
