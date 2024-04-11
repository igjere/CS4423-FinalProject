using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpikeStepper : MonoBehaviour
{
    public UnityEvent onSpikeStep;
    public Creature player;

    void Start(){
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>();
        }
        onSpikeStep.AddListener(PrintPickup);

    }

    void PrintPickup(){
        Debug.Log("Ouch!");
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Spike>() != null){
            player.TakeDamage(1f);
            onSpikeStep.Invoke();
            //myPickupEvent.Invoke(42); //this is how we can pass along data to functions, select the dynamic option if doing so through inspector
            //GetComponent<AudioSource>().Play();
            //Destroy(other.gameObject);
        }
    }
}
