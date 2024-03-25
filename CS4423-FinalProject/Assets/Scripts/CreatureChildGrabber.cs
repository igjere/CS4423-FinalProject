using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureChildGrabber : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Creature>() != null){
            other.transform.parent = this.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.GetComponent<Creature>() != null){
            other.transform.parent = null;
        }
    }
}
