using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAIHugState : CreatureAIState
{
    private float initialSpeed;
    private float maxSpeedIncrease = 2.5f;  // Max speed factor
    private float speedIncreaseDuration = 5.0f;  // Duration of the speed increase phase

    public CreatureAIHugState(CreatureAI creatureAI) : base(creatureAI){}


    public override void BeginState()
    {
        creatureAI.SetColor(Color.red);
        initialSpeed = creatureAI.myCreature.speed;
    }

    public override void UpdateState()
    {
        // Apply speed increase only for Boss type
        if (creatureAI.type == CreatureAI.CreatureType.Boss)
        {
            if (timer < speedIncreaseDuration)
            {
                creatureAI.myCreature.speed = Mathf.Lerp(initialSpeed, initialSpeed * maxSpeedIncrease, timer / speedIncreaseDuration);
            }
            else
            {
                creatureAI.myCreature.speed = initialSpeed;  // Reset speed
                creatureAI.ChangeState(creatureAI.cooldownState);
            }
        }

        if (creatureAI.GetTarget() != null)
        {
            creatureAI.myCreature.MoveCreatureToward(creatureAI.GetTarget().transform.position);
        }
        else
        {
            creatureAI.ChangeState(creatureAI.investigateState);
        }
    }

}
