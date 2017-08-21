using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Coward AI runs when at 50% health -> Doesn't search for player -> Hides in a room or something.
//Aggro AI always searches for and attacks player -> Just pretty retarded overall tbh -> Maybe retreat treshold is varied
//Strategy decides what needs to happen before AI goes into a specific state
public class Base_BossStrategy
{
    public string m_name = "Base";
    public enum STATES
    {
        IDLE,
        ATTACKING,
        SEARCHING,
        RETREAT,

        NUM_STATES
    }
    public STATES m_currentState;

    // direction to pass to movement script
    public Vector2 direction;

    public bool isMoving;

    // values for suspicion
    public Vector3 suspiciousPos;
    public bool isSuspicious;    
    public float suspicion_time = 2;
    public float suspicion_timer;
    
    public float attack_timer;          // timer to calculate when to attack
    public float melee_attack_dist = 0.3f;
    public GameObject bullet_prefab;

    public float look_time = 3;         // time it takes to look around
    public float look_timer;            // timer to calculate when to look around

    public float resetPathfindDist = 0.5f;

    public virtual void Init(BossData boss)
    {
        direction = Vector2.zero;
        m_currentState = STATES.IDLE;
        look_timer = suspicion_timer = 0;
        attack_timer = 99999;

        isMoving = true;
        isSuspicious = false;
    }

    //protected abstract void Init(BossData boss);

    public virtual void Update(BossData boss)
    {
        switch(m_currentState)
        {
            case STATES.IDLE: Idle(boss);
                break;
            case STATES.ATTACKING: Attacking(boss);
                break;
            case STATES.SEARCHING: Searching(boss);
                break;
            case STATES.RETREAT: Retreat(boss);
                break;
        }

        // Quick-fix to stop boss from not dieing
        if (boss.GetComponent<HealthComponent>().GetHealth() <= 0)
        {
            boss.transform.parent.gameObject.SetActive(false);
        }
    }

    //protected abstract void Idle(BossData boss);
    //protected abstract void Attacking(BossData boss);
    //protected abstract void Searching(BossData boss);
    //protected abstract void Retreat(BossData boss);

    public virtual void Idle(BossData boss)
    {
    }

    public virtual void Attacking(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.AGGRESIVE)
            boss.special.TriggerSpecial(boss);

        else if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.DEFENSIVE)
        {
            if (boss.m_health.CalculatePercentageHealth() < 50.0f)
                boss.special.TriggerSpecial(boss);
        }
    }

    public virtual void Searching(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.MOBILITY)
        {
            boss.special.TriggerSpecial(boss);
        }
    }

    public virtual void Retreat(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.MOBILITY)
            boss.special.TriggerSpecial(boss);
    }

    public virtual void OnCollide(GameObject collGO, BossData boss)
    {

    }

    protected bool IsTargetSeen(GameObject target, BossData boss)
    {
        //check player in cone and in range
        Vector3 targetDir = target.transform.position - boss.transform.position;
        Vector3 forward = boss.transform.up;

        float angle = Vector2.Angle(targetDir, forward);
        float distance = Vector2.Distance(target.transform.position, boss.transform.position);
        
        if (angle < boss.m_visionFOV && distance < boss.m_visionDistance)
        {
            int layerMask = Physics2D.DefaultRaycastLayers;
            layerMask = LayerMask.GetMask("Default", "Player");

            RaycastHit2D hit = Physics2D.Raycast(boss.transform.position, targetDir, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                if (CheckValidTarget(hit.collider.gameObject))
                {
                    return true;
                }
            }
            else
            {
                //In case part of the body is seen
                RaycastHit2D hit2 = Physics2D.Raycast(boss.transform.position, Quaternion.AngleAxis(targetDir.z + 10f, Vector3.forward) * targetDir, Mathf.Infinity, layerMask);
                if (hit2.collider != null)
                {
                    if (CheckValidTarget(hit2.collider.gameObject))
                    {
                        return true;
                    }
                }
                else
                {
                    RaycastHit2D hit3 = Physics2D.Raycast(boss.transform.position, Quaternion.AngleAxis(targetDir.z - 10f, Vector3.forward) * targetDir, Mathf.Infinity, layerMask);
                    if (hit3.collider != null)
                    {
                        if (CheckValidTarget(hit3.collider.gameObject))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        return false;
    }

    protected bool CheckValidTarget(GameObject checkObject)
    {
        if (checkObject.tag == "Player")
            return true;

        return false;
    }
}