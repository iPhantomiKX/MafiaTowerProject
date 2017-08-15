using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro_BossStrategy : Base_BossStrategy {

    float playerSearchDist = 3.0f; 

    public override void Init(BossData boss)
    {
        base.Init(boss);

        m_name = "Aggro";
    }

    public override void Idle(BossData boss)
    {
        base.Idle(boss);

        isMoving = false;

        // Randomly look around
        if (look_timer > look_time)
        {
            direction = new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
            look_timer = 0;
        }
        else
        {
            look_timer += Time.deltaTime;
        }
        
        // Transitions
        if (IsTargetSeen(boss.m_player, boss))
        {
            m_currentState = STATES.ATTACKING;
            isMoving = true;
        }
    }

    public override void Attacking(BossData boss)
    {
        base.Attacking(boss);

        if (Vector2.Distance(boss.transform.position, boss.m_player.transform.position) < 0.5f)
        {
            // Attack player
            if (attack_timer > boss.m_attackSpeed)
            {
                boss.m_player.GetComponent<HealthComponent>().TakeDmg((int)boss.m_meleeDamage);
                attack_timer = 0;

                if (isSuspicious)
                {
                    isSuspicious = false;
                    suspicion_timer = 0;
                }
            }
            else
            {
                attack_timer += Time.deltaTime;
            }
        }
        else
        {
            if (isSuspicious)
            {
                // Pathfind to player
                if (!boss.m_pathfinderRef.GetPathFound())
                {
                    boss.m_pathfinderRef.FindPath(boss.m_player.transform.position);
                }
                else
                {
                    direction = boss.m_pathfinderRef.FollowPath();
                }

                if ((suspicion_timer += Time.deltaTime) > suspicion_time && !IsTargetSeen(boss.m_player, boss))
                {
                    isSuspicious = false;
                    suspicion_timer = 0;
                }
            }
            else
            {
                // Move towards player
                direction = (boss.m_player.transform.position - boss.transform.position).normalized;
            }
        }

        // Transitions
        if (!IsTargetSeen(boss.m_player, boss) && !isSuspicious)
        {
            m_currentState = STATES.SEARCHING;
        }
    }

    public override void Searching(BossData boss)
    {
        // If player is too far away, use special to find 
        if (Vector2.Distance(boss.transform.position, boss.m_player.transform.position) > playerSearchDist)
        {
            if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.MOBILITY)
            {
                if (boss.special.TriggerSpecial(boss))
                    boss.m_pathfinderRef.Reset();
            }
        }

        // Randomly pathfind around 
        if (!boss.m_pathfinderRef.GetPathFound())
        {
            boss.m_pathfinderRef.FindPath(boss.m_pathfinderRef.RandomPos(5, boss.transform.position));
        }
        else
        {
            isMoving = true;
            direction = boss.m_pathfinderRef.FollowPath();
        }

        // Transitions
        if (IsTargetSeen(boss.m_player, boss))
        {
            m_currentState = STATES.ATTACKING;
            isMoving = true;
        }
    }

    public override void Retreat(BossData boss)
    {
        base.Retreat(boss);
    }

    public override void OnCollide(GameObject collGO, BossData boss)
    {
        base.OnCollide(collGO, boss);

        if (collGO.GetComponent<SoundCircleController>() && m_currentState == STATES.IDLE)
        {
            m_currentState = STATES.SEARCHING;

            isSuspicious = true;
            suspiciousPos = collGO.transform.position;
        }

        if (collGO.GetComponent<Bullet>())
        {
            m_currentState = STATES.ATTACKING;

            boss.m_pathfinderRef.Reset();
            isMoving = true;

            isSuspicious = true;
            suspiciousPos = collGO.transform.position;
        }
    }
}
