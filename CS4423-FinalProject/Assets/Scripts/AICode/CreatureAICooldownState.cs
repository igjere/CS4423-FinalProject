using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAICooldownState : CreatureAIState
{
    ProjectileThrower projectileThrower;
    private float cooldownDuration = 3.0f;  // Cooldown period duration

    public CreatureAICooldownState(CreatureAI creatureAI) : base(creatureAI) {}

    public override void BeginState()
    {
        if (creatureAI.type == CreatureAI.CreatureType.Boss){
            creatureAI.SetColor(Color.yellow);
        }
        projectileThrower = creatureAI.myCreature.GetComponent<ProjectileThrower>();
        if (projectileThrower == null)
        {
            Debug.LogError("ProjectileThrower component not found on the creature.");
        }
    }

    public override void UpdateState()
    {
        if (timer > cooldownDuration && creatureAI.type == CreatureAI.CreatureType.Boss)
        {
            creatureAI.ChangeState(creatureAI.hugState);  // Go back to hug state
        }
        else
        {
            // Fire projectiles at the player
            if (creatureAI.GetTarget() != null)
            {
                Vector3 direction = (creatureAI.GetTarget().transform.position - creatureAI.myCreature.transform.position).normalized;
                // Debug.Log("Trying launch");
                projectileThrower.TryLaunch(direction);
            }
            else
            {
                // Debug.Log("No target available for projectile.");
            }
        }
    }
}
