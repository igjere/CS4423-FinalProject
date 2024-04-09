using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] CreatureSO creatureSO;



    public void Update(){
        float health = creatureSO.currentHealth; // If you want the current health value
        float maxHealth = creatureSO.maxHealth; // If you want the maximum health value

    }


}
