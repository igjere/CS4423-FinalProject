using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounterText;
    public static CoinCounter singleton;
    int coinsCollected = 0;

    void Awake(){
        if(singleton != null){
            Destroy(this.gameObject);
        }
        singleton = this;
    }
    void Start(){

    }

    public void RegisterCoin(int points = 1){
        coinsCollected += points;
        coinCounterText.text = coinsCollected.ToString();
    }


}
