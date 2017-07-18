using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueSM : NeutralSM {

    public enum RESCUE_STATE
    {
        WAITING,
        FOLLOWING,
        EXITING,
        RESCUED,
    }

    RESCUE_STATE CurrentState = RESCUE_STATE.WAITING;
    bool b_UsePathfinder;

    [Tooltip("To be capped between 1 and 100")]
    public float followDist;
    public float startFollowDist;
    public float startPathfindDist;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        rb.mass = 9999;
        rb.Sleep();

        b_UsePathfinder = false;

        followDist = Mathf.Clamp(followDist, 1, 100f);
    }

    public override void Sense()
    {
        Message temp = ReadFromMessageBoard();
        if (temp != null)
        {
            CurrentMessage = temp;
        }

        ProcessMessage();
    }

    public override int Think()
    {
        switch (CurrentState)
        {
            case RESCUE_STATE.WAITING:
                return (int)RESCUE_STATE.WAITING;

            case RESCUE_STATE.FOLLOWING:
                if (Vector2.Distance(transform.position, ExitPoint.transform.position) < 1f)
                    return (int)RESCUE_STATE.EXITING;

                return (int)RESCUE_STATE.FOLLOWING;

            case RESCUE_STATE.EXITING:
                return (int)RESCUE_STATE.EXITING;

            case RESCUE_STATE.RESCUED:
                return (int)RESCUE_STATE.RESCUED;

            default:
                return -1;
        }
    }

    public override void Act(int value)
    {
        CurrentState = (RESCUE_STATE)value;

        switch (value)
        {
            case (int)RESCUE_STATE.WAITING: DoWait(); break;
            case (int)RESCUE_STATE.FOLLOWING: DoFollow(); break;
            case (int)RESCUE_STATE.EXITING: DoExit(); break;
            case (int)RESCUE_STATE.RESCUED: DoRescue(); break;
        }
    }

    public override void ProcessMessage()
    {
        if (CurrentMessage == null)
        {
            return;
        }

        CurrentMessage = null;
    }

    public void SetState(RESCUE_STATE newState)
    {
        CurrentState = newState;

        if (CurrentState != RESCUE_STATE.WAITING)
        {
            gameObject.tag = "VIP";
            gameObject.layer = LayerMask.NameToLayer("VIP");
        }
    }

    void DoWait()
    {
        // Do Nothing
    }

    void DoFollow()
    {
        // Follow the player
        if (rb.IsSleeping() && GameStateRef.GetState() == GameStateManager.GAME_STATE.RUNNING)
            rb.WakeUp();

        Vector3 dir = (transform.position - player.transform.position);

        if (dir.magnitude > startFollowDist)
        {
            Vector3 point = player.transform.position + (dir.normalized * followDist);

            if (dir.magnitude > startPathfindDist && !b_UsePathfinder)
            {
                b_UsePathfinder = true;
            }
            else
            {
                b_UsePathfinder = false;
            }

            if (!b_UsePathfinder)
            {
                WalkTowardPoint(point);
            }
            else
            {
                if (PathfinderRef.GetPathFound())
                {
                    PathfinderRef.FollowPath();
                }
                else
                {
                    PathfinderRef.FindPath(player.transform.position);
                }
            }
        }
        else 
        {
            b_UsePathfinder = false;
        }

    }

    void DoExit()
    {
        if (PathfinderRef.GetPathFound())
        {
            PathfinderRef.FollowPath();
        }
        else
        {
            PathfinderRef.FindPath(ExitPoint.transform.position);
        }
    }

    void DoRescue()
    {
        Objective objt = GetComponent<Objective>();
        if (!objt.complete && (objt.remainingTime > 0 || !objt.isTimed))
        {
            Debug.Log("Rescued an objective person!");
            objt.numCompleted++;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("PlayerSpawn") && CurrentState == RESCUE_STATE.EXITING)
        {
            CurrentState = RESCUE_STATE.RESCUED;
        }
    }
}
