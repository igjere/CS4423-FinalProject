using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinPickerUpper : MonoBehaviour
{
    public UnityEvent onCoinPickup;
    public Creature player;

    void Start(){
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>();
        }
        // onCoinPickup.AddListener(PrintPickup);

    }

    /* void PrintPickup(){
        Debug.Log("Picked up coin!");
    } */

    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Coin>() != null){
            player.AddCoins(1);
            onCoinPickup.Invoke();
            //myPickupEvent.Invoke(42); //this is how we can pass along data to functions, select the dynamic option if doing so through inspector
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
        else if(other.GetComponent<Heart>() != null){
            if(player.GetCurrentHealth() < player.GetMaxHealth()){
                player.Heal(1f);
                onCoinPickup.Invoke();
                //myPickupEvent.Invoke(42); //this is how we can pass along data to functions, select the dynamic option if doing so through inspector
                //GetComponent<AudioSource>().Play();
                Destroy(other.gameObject);
            }
        }
    }
}
