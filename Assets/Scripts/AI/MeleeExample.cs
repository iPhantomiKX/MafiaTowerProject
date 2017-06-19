﻿//using UnityEngine;
//using System.Collections;

//using System.Linq;

//public class MeleeEnemy : FSMBase
//{
//    public enum ENEMY_STATE
//    {
//        IDLE,
//        MOVING,
//        ATTACKING,
//        DEAD,
//    }

//    [Header("Enemy Variables")]
//    public ENEMY_STATE CurrentState = ENEMY_STATE.IDLE;
//    public float MoveSpeed = 0.5f;
//    [Tooltip("Amount of Grids the enemy will walk through before pathfinding again")]
//    public int WalkRange;

//    bool b_InRange = false;
//    double d_Timer = 0.0;

//    // Use this for initialization
//    public override void Start()
//    {
//        base.Start();
//    }

//    public override void Sense()
//    {
//        Message temp = ReadFromMessageBoard();
//        if (temp != null)
//        {
//            CurrentMessage = temp;
//        }

//        // Search for target
//        if (!m_TargetedEnemy)
//        {
//            b_InRange = false;
//            SearchForTarget(true);
//        }
//        else
//        {
//            if (!b_InRange)
//                CheckIfInRange();

//            if (!m_TargetedEnemy.activeInHierarchy)
//                m_TargetedEnemy = null;
//        }

//        if (theAnimator.GetInteger("ANIM_STATE") != (int)CurrentState || !GetAnimatorIsPlaying())
//        {
//            theAnimator.SetInteger("ANIM_STATE", (int)CurrentState);

//            // Check if death animation is done
//            if (!GetAnimatorIsPlaying() && CurrentState == ENEMY_STATE.DEAD)
//                GetComponent<Health>().DeathAnimationDone = true;
//        }

//        if (!b_InGrid)
//        {
//            b_InGrid = GetComponent<Pathfinder>().CheckIfInGrid();
//        }
//    }

//    public override int Think()
//    {
//        switch (CurrentState)
//        {
//            case ENEMY_STATE.IDLE:

//                if (m_TargetedEnemy)
//                    return (int)ENEMY_STATE.MOVING;

//                if (GetComponent<Health>().HP <= 0)
//                    return (int)ENEMY_STATE.DEAD;

//                return (int)ENEMY_STATE.IDLE;

//            case ENEMY_STATE.MOVING:

//                if (GetComponent<Health>().HP <= 0)
//                    return (int)ENEMY_STATE.DEAD;

//                if (b_InRange)
//                {
//                    GetComponent<Pathfinder>().Reset();
//                    return (int)ENEMY_STATE.ATTACKING;
//                }

//                if (!m_TargetedEnemy)
//                    return (int)ENEMY_STATE.IDLE;

//                return (int)ENEMY_STATE.MOVING;

//            case ENEMY_STATE.ATTACKING:

//                if (GetComponent<Health>().HP <= 0)
//                    return (int)ENEMY_STATE.DEAD;

//                if (!m_TargetedEnemy || !b_InRange)
//                    return (int)ENEMY_STATE.IDLE;

//                return (int)ENEMY_STATE.ATTACKING;
//        }

//        return -1;
//    }

//    public override void Act(int value)
//    {
//        switch (value)
//        {
//            case (int)ENEMY_STATE.IDLE:
//                CurrentState = ENEMY_STATE.IDLE;
//                break;

//            case (int)ENEMY_STATE.MOVING:
//                CurrentState = ENEMY_STATE.MOVING;

//                if (b_InGrid)
//                {
//                    if (GetComponent<Pathfinder>().b_PathFound)
//                    {
//                        GetComponent<Pathfinder>().FollowPath();
//                    }
//                    else
//                    {
//                        // Find Destination
//                        Vector3 dest = new Vector3(0, 0, 0);

//                        // Pathfind to TargetEnemy, find grid closest
//                        dest = GetClosestDestination(WalkRange, m_TargetedEnemy);

//                        // Check if can pathfind to dest (TownHall)
//                        if (!GetComponent<Pathfinder>().FindPath(dest))
//                        {
//                            // If can't find, Target nearest building/tower
//                            SearchForTarget();
//                            GetComponent<Pathfinder>().FindPath(m_TargetedEnemy.transform.position);
//                        }
//                    }
//                }
//                else
//                {
//                    // Translate
//                    Vector3 dir = (m_TargetedEnemy.transform.position - this.gameObject.transform.position).normalized;
//                    dir.y = 0;
//                    this.gameObject.transform.position += dir * MoveSpeed * Time.deltaTime;

//                    // Rotate
//                    Quaternion lookRotation = Quaternion.LookRotation(dir);
//                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3.0f);
//                }

//                break;

//            case (int)ENEMY_STATE.ATTACKING:
//                CurrentState = ENEMY_STATE.ATTACKING;

//                if (d_Timer > AttackSpeed)
//                {
//                    m_TargetedEnemy.GetComponent<Health>().TakeDamage(AttackDamage);
//                    d_Timer = 0.0;

//                    //theAnimator.StopPlayback();
//                }
//                else
//                    d_Timer += Time.deltaTime;
//                break;

//            case (int)ENEMY_STATE.DEAD:
//                CurrentState = ENEMY_STATE.DEAD;

//                this.GetComponent<BoxCollider>().enabled = false;
//                break;
//        }
//    }

//    public override void ProcessMessage()
//    {

//    }

//    void SearchForTarget(bool SearchTownHall = false)
//    {
//        if (SearchTownHall)
//        {
//            // Find TownHall   
//            GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");

//            foreach (GameObject go in AllBuildings)
//            {
//                if (go.name.Contains("Town"))
//                {
//                    m_TargetedEnemy = go;
//                    break;
//                }
//            }
//        }
//        else
//        {
//            // Get available targets
//            GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");
//            GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
//            GameObject[] AllWalls = GameObject.FindGameObjectsWithTag("Wall");

//            GameObject[] AvailableTargets = ((AllBuildings.Union<GameObject>(AllTowers)).Union<GameObject>(AllWalls)).ToArray<GameObject>();

//            // Search for closest one
//            float closestDist = 9999999;
//            GameObject closestGo = null;

//            foreach (GameObject go in AvailableTargets)
//            {
//                if (!go.activeInHierarchy)
//                    continue;

//                float dist = (go.transform.position - this.gameObject.transform.position).magnitude;
//                if (dist < closestDist)
//                {
//                    closestDist = dist;
//                    closestGo = go;
//                }
//            }

//            m_TargetedEnemy = closestGo;
//        }
//    }

//    void CheckIfInRange()
//    {
//        // Get available targets
//        GameObject[] AllBuildings = GameObject.FindGameObjectsWithTag("Buildings");
//        GameObject[] AllTowers = GameObject.FindGameObjectsWithTag("Tower");
//        GameObject[] AllWalls = GameObject.FindGameObjectsWithTag("Wall");

//        GameObject[] AvailableTargets = ((AllBuildings.Union<GameObject>(AllTowers)).Union<GameObject>(AllWalls)).ToArray<GameObject>();

//        foreach (GameObject go in AvailableTargets)
//        {
//            if (this.gameObject.GetComponent<Collider>().bounds.Intersects(go.GetComponent<Collider>().bounds))
//            {
//                b_InRange = true;
//                m_TargetedEnemy = go;
//                break;
//            }
//            else
//            {
//                b_InRange = false;
//            }
//        }
//    }

//    public override float GetMoveSpeed()
//    {
//        return MoveSpeed;
//    }
//}