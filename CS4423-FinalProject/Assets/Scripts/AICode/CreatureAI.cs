using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding; //import the pathfinding library

public class CreatureAI : MonoBehaviour
{

    //blackboard=======================================================
    public Creature myCreature; //the creature we are piloting
    public Creature targetCreature;

    [Header("Config")]
    public LayerMask obstacles;
    public float sightDistance = 50;

    [Header("Pathfinding")]
    Pathfinder<Vector2> pathfinder;
    [SerializeField] float gridSize = 10f;

    //State machine====================================================
    //States go here
    CreatureAIState currentState;
    public CreatureAIIdleState idleState{get; private set;}
    public CreatureAIHugState hugState{get; private set;}
    public CreatureAIPatrolState patrolState{get; private set;}
    public CreatureAIInvestigateState investigateState{get; private set;}


    public void ChangeState(CreatureAIState newState){

        currentState = newState;

        currentState.BeginStateBase();
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Adjust based on your player's setup
         if (playerObject != null) { // Make sure the player object was found
            targetCreature = playerObject.GetComponent<Creature>(); // Assuming Creature component is what you need
        } else {
            Debug.LogError("Player not found. Make sure your player GameObject is tagged correctly.");
        }
        idleState = new CreatureAIIdleState(this);
        hugState = new CreatureAIHugState(this);
        patrolState = new CreatureAIPatrolState(this);
        investigateState = new CreatureAIInvestigateState(this);
        currentState = idleState;

        pathfinder = new Pathfinder<Vector2>(GetDistance,GetNeighbourNodes,1000);
    }


    void FixedUpdate()
    {
        currentState.UpdateStateBase(); //work the current state
        DamageTarget();

    }

    public Creature GetTarget(){
        //are we close enough?
        if(Vector3.Distance(myCreature.transform.position,targetCreature.transform.position) > sightDistance){
            return null;
        }

        //is vision blocked by a wall?
        RaycastHit2D hit = Physics2D.Linecast(myCreature.transform.position, targetCreature.transform.position,obstacles);
        if(hit.collider != null){
            return null;
        }

        return targetCreature;

    }

    public void SetColor(Color c){
        myCreature.body.GetComponent<SpriteRenderer>().color = c;
    }

    //pathfinding
    public float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
    }

    Dictionary<Vector2,float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i=-1;i<2;i++)
        {
            for (int j=-1;j<2;j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j)*gridSize;
                if (!Physics2D.Linecast(pos,pos+dir, obstacles))
                {
                    neighbours.Add(GetClosestNode( pos + dir), dir.magnitude);
                }
            }

        }
        return neighbours;
    }

    //find the closest spot on the grid to begin our pathfinding adventure
    Vector2 GetClosestNode(Vector2 target){
        return new Vector2(Mathf.Round(target.x/gridSize)*gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    public void GetMoveCommand(Vector2 target, ref List<Vector2> path) //passing path with ref argument so original path is changed
    {
        path.Clear();
        Vector2 closestNode = GetClosestNode(myCreature.transform.position);
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
        {
            path.Add(target); //add the final position as our last stop
        }



    }

    //simple wrapper to pathfind to our target
    public void GetTargetMoveCommand(ref List<Vector2> path){
        GetMoveCommand(targetCreature.transform.position, ref path);

    }

    public void DamageTarget()
    {
        // Use a small range for checking proximity instead of an exact value.
        float distanceToTarget = Vector3.Distance(myCreature.transform.position, targetCreature.transform.position);
        float attackRange = 15f; // The range within which you consider "hitting" the target
        float allowedVariance = 2f; // How much variance you allow from the exact range

        // Check if within a "close enough" range instead of an exact match
        if (distanceToTarget >= (attackRange - allowedVariance) && distanceToTarget <= (attackRange + allowedVariance))
        {
            // Debug.Log("Target hit!");
            // Implement the logic to deduct health or damage the target here
            if ((targetCreature != null) && (!myCreature.IsInvincible()))
            {
                // Debug.Log("Got here");
                // Call TakeDamage on targetCreature with the damage amount, e.g., 1
                targetCreature.TakeDamage(1);
            }
        }
    }
}
