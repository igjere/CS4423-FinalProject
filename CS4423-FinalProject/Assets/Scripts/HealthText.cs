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
        healthText.text = creatureSO.health.ToString();
    }


}
