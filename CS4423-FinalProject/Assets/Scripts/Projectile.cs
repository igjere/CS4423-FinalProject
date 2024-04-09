using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* public class Projectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Creature enemy = other.GetComponent<Creature>();
            if (enemy != null)
            {
                GetComponent<AudioSource>().Play();
                enemy.GiveDamage();
                Destroy(this.gameObject);
            }
        }
        if(other.gameObject.tag == "Wall"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Obstacle"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
} */
public class Projectile : MonoBehaviour
{
    public float damage = 1f; // Default damage
    public bool isSpectral = false;
    public bool isHoming = false;

    // Call this method to set the projectile's attributes when instantiated
    public void SetAttributes(float newDamage, bool spectral, bool homing, float sizeMultiplier)
    {
        damage = newDamage;
        isSpectral = spectral;
        isHoming = homing;

        // Adjusting the scale based on the original size (17x17x17) and the sizeMultiplier
        transform.localScale = new Vector3(17, 17, 17) * sizeMultiplier;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Creature enemy = other.GetComponent<Creature>();
            if (enemy != null)
            {
                GetComponent<AudioSource>().Play();
                enemy.GiveDamage(damage); // Ensure GiveDamage can accept a damage parameter
                Destroy(this.gameObject);
            }
        }
        // Additional collision handling...
        if(other.gameObject.tag == "Wall"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Obstacle"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Door"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
    
    void Update()
    {
        if (isHoming)
        {
            // Homing logic here
        }
        if (isSpectral){

        }
    }
}
