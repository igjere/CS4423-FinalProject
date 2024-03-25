using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Creature enemy = other.GetComponent<Creature>();
            if (enemy != null)
            {
                GetComponent<AudioSource>().Play();
                enemy.GiveDamage(1);
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
}
