using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float speed = 5f;
    [SerializeField] float fireRate = 5f; // Shots per second

    private float timeSinceLastShot = 0f;

    void Update()
    {
        // Update the timer every frame
        if (timeSinceLastShot < 1 / fireRate)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    public void TryLaunch(Vector3 direction)
    {
        // Check if enough time has passed since the last shot
        if (timeSinceLastShot >= 1 / fireRate)
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
        Destroy(newProjectile, 1.5f);
    }
}
