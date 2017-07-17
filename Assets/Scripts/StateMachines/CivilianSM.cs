using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSM : NeutralSM {

    public enum CIVILIAN_STATE
    {
        IDLE,       
        PATROLLING,
        RUNNING,
    }

    public CIVILIAN_STATE CurrentState = CIVILIAN_STATE.IDLE;

    float origIdleTime;
    float DebugTime = 9999999.0f;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        origIdleTime = idleTime;
    }

    public override void Sense ()
	{
		Message temp = ReadFromMessageBoard();
		if (temp != null)
		{
			CurrentMessage = temp;	
		}

		ProcessMessage ();

        DebugTime -= Time.deltaTime;
	}

    public override int Think()
    {
        switch (CurrentState)
        {
            case CIVILIAN_STATE.IDLE:
                if (idleTime <= 0)
                {
                    // Random a new position to walk to 
                    PatrolPosition = PathfinderRef.RandomPos(15);
                    Debug.DrawLine(transform.position, PatrolPosition, Color.black, 9999);

                    return (int)CIVILIAN_STATE.PATROLLING;
                }

                if (DebugTime <= 0)
                    return (int)CIVILIAN_STATE.RUNNING;

                return (int)CIVILIAN_STATE.IDLE;

            case CIVILIAN_STATE.PATROLLING:
                if (PathfinderRef.GetPathComplete())
                {
                    Debug.Log("Going back to idle");
                    idleTime = origIdleTime;
                    return (int)CIVILIAN_STATE.IDLE;
                }
                if (DebugTime <= 0)
                    return (int)CIVILIAN_STATE.RUNNING;
                
                return (int)CIVILIAN_STATE.PATROLLING;

            case CIVILIAN_STATE.RUNNING:
                return (int)CIVILIAN_STATE.RUNNING;

            default:
                return -1;
        }
    }

    public override void Act(int value)
    {
        CurrentState = (CIVILIAN_STATE)value;

        switch (value)
        {
            case (int)CIVILIAN_STATE.IDLE: DoIdle(); break;
            case (int)CIVILIAN_STATE.PATROLLING: DoPatrol(); break;
            case (int)CIVILIAN_STATE.RUNNING: DoRun(); break;
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

    private void DoIdle()
    {
        // Random a direction to look around
        if ((int)idleTime % 5 == 0)
        {
            idleAngle = Random.Range(0, 360);
        }

        idleTime -= Time.deltaTime;
        FaceTowardAngle(idleAngle, 0.03f);

    }

    private void DoPatrol()
    {
        if (PathfinderRef.GetPathFound())
        {
            PathfinderRef.FollowPath();
        }
        else
        {
            PathfinderRef.FindPath(PatrolPosition);
        }
    }

    private void DoRun()
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
}
