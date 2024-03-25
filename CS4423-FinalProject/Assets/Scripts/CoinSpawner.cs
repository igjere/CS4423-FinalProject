using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnRange = 10;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCoins();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnCoins(){
        StartCoroutine(SpawnCoinsRoutine());
        IEnumerator SpawnCoinsRoutine(){
            while(true){
                yield return new WaitForSeconds(1);
                SpawnCoinRandom();
            }
        }
    }




    void SpawnCoinRandom(){

       float randomX = Random.Range(-spawnRange,spawnRange);
       float randomY = Random.Range(-spawnRange,spawnRange);

       GameObject newCoin = Instantiate(coinPrefab,new Vector3(randomX,randomY,0),Quaternion.identity);
       Destroy(newCoin,10);
       newCoin.transform.eulerAngles = new Vector3(0,0,45);
    }
}
