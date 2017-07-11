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

    int waypointIdx;
    float origIdleTime;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        waypointIdx = 0;
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
	}

    public override int Think()
    {
        switch (CurrentState)
        {
            case CIVILIAN_STATE.IDLE:
                if (idleTime <= 0)
                {
                    if (waypointIdx == PatrolPoints.Count - 1)
                    {
                        PatrolPoints.Reverse();
                        waypointIdx = 0;
                    }
                    else
                    {
                        waypointIdx++;
                    }

                    PatrolPosition = PatrolPoints[waypointIdx];
                    return (int)CIVILIAN_STATE.PATROLLING;
                }
                return (int)CIVILIAN_STATE.IDLE;

            case CIVILIAN_STATE.PATROLLING:
                if (Vector2.Distance(this.transform.position, PatrolPosition) < 0.2f)
                    return (int)CIVILIAN_STATE.IDLE;

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
        if (PatrolPoints.Count <= 0)
        {
            idleTime = origIdleTime;
            return;
        }

        Debug.DrawLine(transform.position, PatrolPosition);

        WalkTowardPoint(PatrolPosition);
    }

    private void DoRun()
    {

    }
}
