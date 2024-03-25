using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAIPatrolState : CreatureAIState
{
    public CreatureAIPatrolState(CreatureAI creatureAI) : base(creatureAI){}

    public override void BeginState()
    {
        MoveRandom();
        creatureAI.SetColor(Color.blue);
    }
    Vector3 moveVec;
    public override void UpdateState()
    {
        if(timer > 1.5f){
            timer = 0;

            MoveRandom();
        }

        creatureAI.myCreature.MoveCreature(moveVec);

        if(creatureAI.GetTarget() != null){
            creatureAI.ChangeState(creatureAI.hugState);
        }
    }

    public void MoveRandom(){
        moveVec = (new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0));
    }

}
