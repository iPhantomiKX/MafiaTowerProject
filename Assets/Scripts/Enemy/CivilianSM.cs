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

	// Use this for initialization
    public override void Start()
    {
        base.Start();
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
                    return (int)CIVILIAN_STATE.PATROLLING;

                return (int)CIVILIAN_STATE.IDLE;

            case CIVILIAN_STATE.PATROLLING:
                if (idleTime > 0)
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
        idleTime -= Time.deltaTime;

        FaceTowardAngle(idleAngle, 0.03f);

        if (idleTime <= 0)
        {
            float rand = Random.Range(0f, 100f);
            if (rand <= 25 && PatrolPoints.Count > 0)
            {
                idleTime = 0;
                float nearest = 9999f;
                float tempF;
                int indexT = -1;
                for (int i = 0; i < PatrolPoints.Count; i++)
                {
                    tempF = Vector2.Distance(this.transform.position, PatrolPoints[i]);
                    if (tempF < 0.2)
                        continue;
                    if (tempF < nearest)
                    {
                        nearest = tempF;
                        indexT = i;
                    }
                }
                if (indexT >= 0)
                    PatrolPosition = PatrolPoints[indexT];
            }
            else
            {
                idleTime = rand / 25;
                float r_angle = Random.Range(0f, 360f);
                idleAngle = r_angle;
            }
        }
    }

    private void DoPatrol()
    {
        if (PatrolPoints.Count <= 0 || PatrolPosition == Vector3.forward)
        {
            idleTime = 1f;
            return;
        }
        if (Vector2.Distance(this.transform.position, PatrolPosition) > 0.2f)
        {
            WalkTowardPoint(PatrolPosition);
        }
        else
        {
            idleTime = 1f;
            PatrolPosition = Vector3.forward;
        }

    }

    private void DoRun()
    {

    }
}
