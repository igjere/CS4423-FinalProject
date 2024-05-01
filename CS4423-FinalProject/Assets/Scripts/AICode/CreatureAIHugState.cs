using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAIHugState : CreatureAIState
{
    private float initialSpeed;
    private float maxSpeedIncrease = 2f;  // Max speed factor
    private float speedIncreaseDuration = 5.0f;  // Duration of the speed increase phase

    private float bounceAmplitude = 0.5f; // Height of the bounce
    private float bounceFrequency = 6.0f; // Speed of the bounce

    public CreatureAIHugState(CreatureAI creatureAI) : base(creatureAI){}


    public override void BeginState()
    {
        if (creatureAI.type == CreatureAI.CreatureType.Enemy){
            creatureAI.SetColor(Color.red);
        }
        initialSpeed = creatureAI.myCreature.speed;
    }

    public override void UpdateState()
    {
        if (creatureAI.GetTarget() != null)
        {
            if (creatureAI.type == CreatureAI.CreatureType.Boss)
            {
                // Speed increase logic for Boss type
                if (timer < speedIncreaseDuration)
                {
                    creatureAI.myCreature.speed = Mathf.Lerp(initialSpeed, initialSpeed * maxSpeedIncrease, timer / speedIncreaseDuration);
                }
                else
                {
                    creatureAI.myCreature.speed = initialSpeed;  // Reset speed after duration ends
                    creatureAI.ChangeState(creatureAI.cooldownState);
                }

                // Bouncing motion only for the Boss type
                Vector3 targetPosition = creatureAI.GetTarget().transform.position;
                Vector3 moveDirection = (targetPosition - creatureAI.myCreature.transform.position).normalized;
                float bounce = Mathf.Sin(Time.time * bounceFrequency) * bounceAmplitude;
                Vector3 newPosition = creatureAI.myCreature.transform.position + moveDirection * creatureAI.myCreature.speed * Time.deltaTime;
                newPosition.y += bounce;

                // Flip the boss sprite based on the direction of movement
                if (moveDirection.x < 0) {
                    creatureAI.myCreature.transform.localScale = new Vector3(-80, 80, 80); // Flip sprite to face left
                } else if (moveDirection.x > 0) {
                    creatureAI.myCreature.transform.localScale = new Vector3(80, 80, 80); // Normal orientation, facing right
                }
                
                creatureAI.myCreature.transform.position = newPosition;
            }
            else if (creatureAI.type == CreatureAI.CreatureType.Ranged){
                creatureAI.ChangeState(creatureAI.cooldownState);
            }
            else
            {
                // Normal movement for regular enemies
                creatureAI.myCreature.MoveCreatureToward(creatureAI.GetTarget().transform.position);
            }
        }
        else
        {
            creatureAI.ChangeState(creatureAI.investigateState);
        }
    }

}
