using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Creature : MonoBehaviour
{
    [Header("Stats")]
    public bool isAlive = true;
    public float speed = 0f;
    [SerializeField] float jumpForce = 10;
    // [SerializeField] public float health = 3;
    [SerializeField] private float currentHealth = 3; // Current filled health.
    [SerializeField] private float maxHealth = 3; //
    [SerializeField] int stamina = 3;
    [SerializeField] bool isInvincible = false;
    [SerializeField] float invincibilityDurationSeconds = 5f; // Duration of i-frames in seconds

    public enum CreatureMovementType { tf, physics };
    [SerializeField] CreatureMovementType movementType = CreatureMovementType.tf;
    public enum CreaturePerspective { topDown, sideScroll };
    [SerializeField] CreaturePerspective perspectiveType = CreaturePerspective.topDown;

    [Header("Physics")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpOffset = -.5f;
    [SerializeField] float jumpRadius = .25f;

    [Header("Paper")]
    public float paperRate = 0.5f;
    public float paperDamage = 1;
    [SerializeField] float paperSize = 0.5f;
    /* [SerializeField] public float paperDamage = 1;
    [SerializeField] public float paperSpeed = 5f;
    [SerializeField] public float paperRate = 5f; // Shots per second
    [SerializeField] public float paperRange = 1.5f; */

    [Header("Flavor")]
    [SerializeField] string creatureName = "Meepis";
    public GameObject body;
    [SerializeField] private List<AnimationStateChanger> animationStateChangers;

    [Header("Tracked Data")]
    [SerializeField] Vector3 homePosition = Vector3.zero;
    [SerializeField] CreatureSO creatureSO;
    // [SerializeField] ItemDatabase itemDB;

    [SerializeField] private ScreenFader screenFader;

    [SerializeField] private int coins = 0;

    Rigidbody2D rb;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(health);

    }



    // Update is called once per frame
    void Update()
    {
        if(creatureSO != null){
            creatureSO.currentHealth = currentHealth;
            creatureSO.maxHealth = maxHealth;
            creatureSO.stamina = stamina;
        }
    }

    void FixedUpdate(){

    }

    public RoomInstance roomInstance;

    public void SetRoomInstance(RoomInstance room) {
        roomInstance = room;
    }

    public void MoveCreature(Vector3 direction)
    {
        if (!isAlive) return;

        if (movementType == CreatureMovementType.tf)
        {
            MoveCreatureTransform(direction);
        }
        else if (movementType == CreatureMovementType.physics)
        {
            MoveCreatureRb(direction);
        }

        //set animation
        if(direction != Vector3.zero){
            foreach(AnimationStateChanger asc in animationStateChangers){
                asc.ChangeAnimationState("Walk",speed);
            }
        }else{
            foreach(AnimationStateChanger asc in animationStateChangers){
                asc.ChangeAnimationState("Idle");
            }
        }
    }

    public void MoveCreatureToward(Vector3 target){
        Vector3 direction = target - transform.position;
        MoveCreature(direction.normalized);
    }

    public void Stop(){
        MoveCreature(Vector3.zero);
    }

    public void MoveCreatureRb(Vector3 direction)
    {
        if (!isAlive) return;
        Vector3 currentVelocity = Vector3.zero;
        if(perspectiveType == CreaturePerspective.sideScroll){
            currentVelocity = new Vector3(0, rb.velocity.y, 0);
        }

        rb.velocity = (currentVelocity) + (direction * speed);
        if(rb.velocity.x < 0){
            body.transform.localScale = new Vector3(-1,1,1);
        }else if(rb.velocity.x > 0){
            body.transform.localScale = new Vector3(1,1,1);
        }
        //rb.AddForce(direction * speed);
        //rb.MovePosition(transform.position + (direction * speed * Time.deltaTime))
    }

    public void MoveCreatureTransform(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetCurrentSpeed() => speed;

    /* public float GetHealth()
    {
        return health;
    } */

    public int AddCoins(int amount)
    {
        coins += amount;
        return coins;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void Jump()
    {
        if(Physics2D.OverlapCircleAll(transform.position + new Vector3(0,jumpOffset,0),jumpRadius,groundMask).Length > 0){
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvincible  || currentHealth <= 0){ 
            return; // Do nothing if invincible
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't fall below 0 or above maxHealth

        CheckDeath();
        /* if (health <= 0)
        {
            Debug.Log(creatureName + " has died.");
            // yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
            SceneManager.LoadScene("MainMenu");
            // screenFader.FadeToColor("MainMenu");
        }
        else
        {
            // Debug.Log(creatureName + " takes " + damageAmount + " damage, " + health + " health remaining.");
            StartCoroutine(BecomeTemporarilyInvincible()); // Start invincibility frames
        } */
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed maxHealth
    }

    // Call this method to increase max health (heart containers) and fully heal
    public void IncreaseMaxHealth(float increaseAmount)
    {
        maxHealth += increaseAmount;
        currentHealth = maxHealth; // Fully heal the player
    }

    public void DecreaseMaxHealth(float decreaseAmount)
    {
        maxHealth -= decreaseAmount;
        currentHealth = maxHealth; // Fully heal the player
        StartCoroutine(BecomeTemporarilyInvincible()); // Start invincibility frames
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log($"{creatureName} has died.");
            isAlive = false;
            /* ItemDatabase itemDatabase = FindObjectOfType<ItemDatabase>(); // Assuming it's attached to a GameObject
            if (itemDatabase != null) {
                itemDatabase.ResetAvailableItems();
            } */
            ItemDatabase.Instance.ResetAvailableItems();
            Destroy(gameObject);
            Destroy(this.gameObject);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Debug.Log(creatureName + " takes " + damageAmount + " damage, " + health + " health remaining.");
            StartCoroutine(BecomeTemporarilyInvincible()); // Start invincibility frames
        }
    }
    public IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        for (float i = 0; i < 1f; i += 0.1f)
        {
            body.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.15f);
            body.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
        // Debug.Log("i Frames end");
        isInvincible = false;
    }

    public void GiveDamage(float damage)
    {
        if (currentHealth <= 0){ 
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Debug.Log(creatureName + " has died.");
        
            // Notify RoomInstance to remove this creature
            if (roomInstance != null) {
                roomInstance.enemiesInRoom.Remove(this.gameObject);
                roomInstance.CheckAndUpdateDoors(); // Suggestive method name
                Destroy(gameObject);
            }
        }
        else
        {
            // Debug.Log(creatureName + " takes " + damageAmount + " damage, " + health + " health remaining.");
            // StartCoroutine(BecomeTemporarilyInvincible()); // Start invincibility frames
        }
    }

    /* TODO: 
       Implement functions for adding to health and other stats for items
    */
}
