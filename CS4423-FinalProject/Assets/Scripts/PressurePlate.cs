using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Pressure plate was triggered.");
        GetComponent<SpriteRenderer>().color = Color.blue;

        //other.GetComponent<SpriteRenderer>().color = Color.red;

    }
    void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
