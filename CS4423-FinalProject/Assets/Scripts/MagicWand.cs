using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    //attributes
    [Header("Properties")]
    public float damage = 1f;
    public float projectileSpeed = 1f;
    public float cooldown = 1f;
    public float accuracy = 1f;
    public int projectiles = 1;
    public float projectileSize = .1f;
    public float projectileGravity = 0;
    public List<float> distribution;

    [Header("Prefabs")]
    public GameObject projectile;

    [Header("Transforms")]
    public Transform spawnPoint;

    [Header("Generation Config")]
    public bool randomizeAttributes = false;
    public bool generateFair = false;
    public bool randomizeSeed = false;
    public int seed = 0;

    [Header("Damage")]
    public float worstDamage = -10f;
    public float bestDamage = 10f;

    [Header("Speed")]
    public float worstSpeed = .1f;
    public float bestSpeed = 10f;

    [Header("Cooldown")]
    public float worstCooldown = 1f;
    public float bestCooldown = .1f;

    [Header("Accuracy")]
    public float worstAccuracy = -90f;
    public float bestAccuracy = 0f;

    [Header("Projectiles")]
    public int worstProjectiles = 1;
    public int bestProjectiles = 10;

    [Header("Size")]
    public float worstProjectileSize = .01f;
    public float bestProjectileSize = 2f;

    [Header("Gravity")]
    public float worstGravity = 1;
    public float bestGravity = 0;

    //Components
    SpriteRenderer spriteRenderer;

    List<GameObject> pool;

    void Start(){



        pool = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(randomizeAttributes){
            if(generateFair){
                GenerateFair();
            }else{
                Generate();
            }
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
        }
    }


    public void Generate(){
        //code for today!
        //Random.InitState(seed);
        if(randomizeSeed){

            seed = Random.Range(int.MinValue,int.MaxValue);
            Random.InitState(seed);
        }else{
            Random.InitState(seed);
        }

        //randomizeAttributes attributes
        damage =  Random.Range(worstDamage,bestDamage);
        projectileSpeed = Random.Range(worstSpeed,bestSpeed);
        cooldown = Random.Range(worstCooldown,bestCooldown);
        accuracy = Random.Range(worstAccuracy,bestAccuracy);


        projectiles = (int)Random.Range(worstProjectiles,bestProjectiles);


        projectileSize = Random.Range(worstProjectileSize,bestProjectileSize);
        projectileGravity = Random.Range(worstGravity,bestGravity);
        //let's randomizeAttributes our color too!

    }

    public void GenerateFair(){
        distribution = GetDistribution(7);
        damage = Mathf.Lerp(worstDamage,bestDamage,distribution[0]);
        projectileSpeed = Mathf.Lerp(worstSpeed,bestSpeed,distribution[1]);
        cooldown = Mathf.Lerp(worstCooldown,bestCooldown,distribution[2]);
        accuracy = Mathf.Lerp(worstAccuracy,bestAccuracy,distribution[3]);
        projectiles = (int)Mathf.Lerp((float)worstProjectiles,(float)bestProjectiles,distribution[4]) + 1;
        projectileSize = Mathf.Lerp(worstProjectileSize,bestProjectileSize,distribution[5]);
        projectileGravity = Mathf.Lerp(worstGravity,bestGravity,distribution[6]);
    }

    void Update(){
        Shoot();
    }


    bool onCooldown = false;
    public void Shoot(){
        if(onCooldown){
            return;
        }
        //begin cooldown
        ShootCooldown();

        //projectile count
        for(int i = 0; i<projectiles; i++){
            //create projectile, apply the damage and speed
            GameObject newProjectile;
            // if(pool.Count > 10){
            //     pool.RemoveAt(0);
            //     newProjectile = pool[0];
            //     newProjectile.transform.position = spawnPoint.position;
            // }else{
            newProjectile = Instantiate(projectile,spawnPoint.position,Quaternion.identity);
            Destroy(newProjectile,10);
            //}
            pool.Add(newProjectile);

            //apply the size
            newProjectile.transform.localScale = Vector3.one * projectileSize;

            //accuracy
            float angleOffset = Random.Range(-accuracy,accuracy);

            //launch the projectile!
            newProjectile.GetComponent<WandProjectile>().Launch(transform.rotation * Quaternion.Euler(0,0,angleOffset),projectileSpeed,damage);
            newProjectile.GetComponent<SpriteRenderer>().color = spriteRenderer.color;

            newProjectile.GetComponent<Rigidbody2D>().gravityScale = projectileGravity;
            //so we don't add up projectiles forever, destroy after 10 seconds
            //Destroy(newProjectile,10f);
        }
    }

    void ShootCooldown(){
        //prevent weird duplicate cooldown behaviors
       if(onCooldown){
        return;
       }
       //set on cooldown to true so we can't shoot
       onCooldown = true;

       //wait for cooldown seconds until we can shoot again
       StartCoroutine(ShootCooldownRoutine());
       IEnumerator ShootCooldownRoutine(){
            yield return new WaitForSeconds(cooldown);
            onCooldown = false;
        }
    }

    public static List<float> GetDistribution(int distLen){
        List<float> dist = new List<float>();
        for(int i = 0; i<distLen; i++){dist.Add(0);}
        Debug.Log(dist.Count);
        float total = ((float)distLen) / 2;
        int offset = Random.Range(0,distLen);
        for(int i = 0; i<distLen; i++){
            float min = Mathf.Max(0,total-((distLen-1.0f)-i));
            float max = Mathf.Min(total,1f);
            float randNum = Random.Range(min,max);
            total -= randNum;
            Debug.Log((i+offset)%distLen);
            dist[(i+offset)%distLen] = (randNum);
        }
        return dist;
    }





}
