using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward_BossStrategy : Base_BossStrategy {

    Vector3 origSpawn;

    bool isReturning = false;

    float returnDist = 1.5f;    // Distance where boss would return to his 'room'
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
            isMoving = true;

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
            isMoving = true;

            if (dist > returnDist)
            {
                isReturning = true;
            }

            // Look at suspicion position
            if (isSuspicious)
            {
                isMoving = false;

                direction = (suspiciousPos - boss.transform.position).normalized;

                if ((suspicion_timer += Time.deltaTime) > suspicion_time)
                {
                    isSuspicious = false;
                    suspicion_timer = 0;
                }

            }
            else
            {
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
            }
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

        // Actions
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

            // Pathfind to player
            //if (!boss.m_pathfinderRef.GetPathFound())
            //{
            //    boss.m_pathfinderRef.FindPath(boss.m_player.transform.position);
            //}
            //else
            //{
            //    direction = boss.m_pathfinderRef.FollowPath();
            //}
        }

        // Transitions
        float dist = Vector3.Distance(boss.transform.position, origSpawn);
        if ((!IsTargetSeen(boss.m_player, boss) || dist > returnDist) && !isSuspicious)
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

    public override void OnCollide(GameObject collGO, BossData boss)
    {
        base.OnCollide(collGO, boss);

        if (collGO.GetComponent<SoundCircleController>() && m_currentState == STATES.IDLE)
        {
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
