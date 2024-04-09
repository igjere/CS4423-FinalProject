using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* public class ProjectileThrower : MonoBehaviour
{
    // public Creature player;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float speed = 5f;
    [SerializeField] float fireRate = 5f; // Shots per second
    [SerializeField] float paperRange = 1.5f;
    
    private float timeSinceLastShot = 0f;

    void Update()
    {
        // Update the timer every frame
        if (timeSinceLastShot < 1 / fireRate)
        // if (timeSinceLastShot < 1 / player.paperRate)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    public void TryLaunch(Vector3 direction)
    {
        // Check if enough time has passed since the last shot
        if (timeSinceLastShot >= 1 / fireRate)
        // if (timeSinceLastShot >= 1 / player.paperRate)
        {
            Launch(direction);
            timeSinceLastShot = 0; // Reset the timer
        }
    }

    private void Launch(Vector3 direction)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // GetComponent<AudioSource>().Play();
        newProjectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
        Destroy(newProjectile, paperRange);
    }
} */
public class ProjectileThrower : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    public float speed = 38;
    public float fireRate = 2; // Shots per second
    public float paperRange = 10;
    public float damage = 1; // Default damage
    public bool spectral = false;
    public bool homing = false;
    public float projectileSize = 1;

    private float timeSinceLastShot = 0f;

    void Update()
    {
        // Update the timer every frame
        if (timeSinceLastShot < 1 / fireRate)
        // if (timeSinceLastShot < 1 / player.paperRate)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    // Update attributes based on item effects
    public void UpdateProjectileAttributes(float newDamage, float newFireRate, bool newSpectral, bool newHoming, float newSize)
    {
        damage = newDamage;
        fireRate = newFireRate;
        spectral = newSpectral;
        homing = newHoming;
        projectileSize = newSize;
    }

    public void TryLaunch(Vector3 direction)
    {
        // Check if enough time has passed since the last shot
        if (timeSinceLastShot >= 1 / fireRate)
        // if (timeSinceLastShot >= 1 / player.paperRate)
        {
            Launch(direction);
            timeSinceLastShot = 0; // Reset the timer
        }
    }

    private void Launch(Vector3 direction)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            // Assuming projectileSize here is already a multiplier. E.g., 1 for original size, 2 for double size, etc.
            projectileScript.SetAttributes(damage, spectral, homing, projectileSize);
        }
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
        Destroy(newProjectile, paperRange);
    }
    // Rest of your existing code...
    public float GetCurrentDamage() => damage;
    public float GetCurrentPaperRate() => fireRate;
    public float GetCurrentPaperSpeed() => speed;
    public float GetCurrentRange() => paperRange;
}
