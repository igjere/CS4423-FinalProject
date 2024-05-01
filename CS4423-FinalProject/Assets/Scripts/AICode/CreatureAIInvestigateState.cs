using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAIInvestigateState : CreatureAIState
{
    List<Vector2> path;
    public CreatureAIInvestigateState(CreatureAI creatureAI) : base(creatureAI){}

    float pauseTime = 0.1f;
    public override void BeginState()
    {
        if(path == null){
            path = new List<Vector2>();
        }
        creatureAI.GetTargetMoveCommand(ref path);
        // Debug.Log(path.Count);
        creatureAI.SetColor(Color.red);
    }
    public override void UpdateState()
    {

        //if we finished walking the path, or couldn't find one, start patrolling randomly
        if(path.Count == 0){
            creatureAI.ChangeState(creatureAI.patrolState);
            return;
        }

        //draw lines in scene view to see where we're going
        Debug.DrawLine(creatureAI.myCreature.transform.position,path[0]);
        for(int i = 0; i < path.Count-1; i++){
            Debug.DrawLine(path[i],path[i+1]);
        }


        //wait a bit before pursuing
        if(timer < pauseTime){
            creatureAI.myCreature.Stop();
            return;
        }

        //if we see the target, start pursuing immediately
        if(creatureAI.GetTarget() != null){
            creatureAI.ChangeState(creatureAI.hugState);
            // Debug.Log(path.Count);
            return;
        }

        creatureAI.myCreature.MoveCreatureToward(path[0]); //move to the next stop on the path
        if(Vector3.Distance(creatureAI.myCreature.transform.position,path[0]) < creatureAI.myCreature.speed * Time.fixedDeltaTime){
            creatureAI.myCreature.transform.position = path[0]; //teleport to path point so we don't overshoot
            path.RemoveAt(0); //remove element
        }





    }





}