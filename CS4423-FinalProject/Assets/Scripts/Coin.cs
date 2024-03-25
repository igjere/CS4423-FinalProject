using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{

    [SerializeField] private AudioClip coinClip;



    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.GetComponent<Creature>() != null){
    //         CoinCounter.singleton?.RegisterCoin();
    //         other.GetComponent<AudioSource>()?.PlayOneShot(coinClip);
    //         Destroy(this.gameObject);


    //     }

    // }


}
