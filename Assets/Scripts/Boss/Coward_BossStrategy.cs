using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward_BossStrategy : Base_BossStrategy {

    Vector3 origSpawn;

    bool isReturning = false;

    float returnDist = 2;       // Distance where boss would return to his 'room'
    float idleDist = 0.25f;     // Distance where boss is close enough to origSpawn to stop returning

    public override void Init(BossData boss)
    {
        base.Init(boss);

        m_name = "Coward";
        origSpawn = boss.transform.position;
    }

    public override void Idle(BossData boss)
    {
        base.Idle(boss);

        // Actions
        float dist = Vector3.Distance(boss.transform.position, origSpawn);        
        if (isReturning)
        {
            // Move back to origSpawn
            if (!boss.m_pathfinderRef.GetPathFound())
            {
                boss.m_pathfinderRef.FindPath(origSpawn);
            }
            else
            {
                direction = boss.m_pathfinderRef.FollowPath();
            }

            if (dist <= idleDist)
            {
                isReturning = false;
            }
        }
        else
        {
            if (dist > returnDist)
            {
                isReturning = true;
            }

            direction = Vector2.zero;
        }

        // Transitions
        if (IsTargetSeen(boss.m_player, boss))
        {
            m_currentState = STATES.ATTACKING;
        }
    }

    public override void Attacking(BossData boss)
    {
        base.Attacking(boss);

        // Actions
        if (Vector2.Distance(boss.transform.position, boss.m_player.transform.position) < 0.2f)
        {
            // Attack player
            if (timer > boss.m_attackSpeed)
            {
                boss.m_player.GetComponent<HealthComponent>().TakeDmg((int)boss.m_meleeDamage);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            // Move towards player
            direction = (boss.m_player.transform.position - boss.transform.position).normalized;
        }

        // Transitions
        float dist = Vector3.Distance(boss.transform.position, origSpawn);
        if (!IsTargetSeen(boss.m_player, boss) || dist > returnDist)
        {
            m_currentState = STATES.IDLE;
        }

        if (boss.m_health.CalculatePercentageHealth() <= 50)
        {
            m_currentState = STATES.RETREAT;
        }
    }

    public override void Searching(BossData boss)
    {
        // Should never go into this state
    }

    public override void Retreat(BossData boss)
    {
        base.Retreat(boss);
        origSpawn = boss.transform.position;

        if (!IsTargetSeen(boss.m_player, boss))
        {
            m_currentState = STATES.IDLE;
        }
    }

}
