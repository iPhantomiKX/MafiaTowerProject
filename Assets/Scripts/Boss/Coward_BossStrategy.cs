using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward_BossStrategy : Base_BossStrategy {

    public override void Init(BossData boss)
    {
        base.Init(boss);
    }

    public override void Idle(BossData boss)
    {
        base.Idle(boss);

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
            direction = boss.m_player.transform.position - boss.transform.position;
        }

        // Transitions
        if (!IsTargetSeen(boss.m_player, boss))
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

        if (!IsTargetSeen(boss.m_player, boss))
        {
            m_currentState = STATES.IDLE;
        }
    }

}
