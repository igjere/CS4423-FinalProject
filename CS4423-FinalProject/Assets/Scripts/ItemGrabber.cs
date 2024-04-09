using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrabber : MonoBehaviour
{
    public UnityEvent onItemPickup;
    public Creature player;

    void Start(){
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>();
        }
        onItemPickup.AddListener(PrintPickup);

    }

    void PrintPickup(){
        //player.health++;  //to test if items can affect the player (it works)
        Debug.Log("Picked up item!");
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<BookShelf>() != null){
            // player.AddCoins(1);
            onItemPickup.Invoke();
            //myPickupEvent.Invoke(42); //this is how we can pass along data to functions, select the dynamic option if doing so through inspector
            // GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    }
}
