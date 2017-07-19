﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSM : NeutralSM {

    public enum CIVILIAN_STATE
    {
        IDLE,           // Doing nothing       
        PATROLLING,     // Randomly walking around
        INTERACTING,    // Interacting with the player
        PERSUADED,      // Helping the player after being persuaded
        RUNNING,        // Running to the exit point
    }

    public CIVILIAN_STATE CurrentState = CIVILIAN_STATE.IDLE;
    public double PersuadeActionTime = 5.0;
    public float emitSoundInterval = 2f;
    public float increasedSpeed = 1f;

    float origIdleTime;
    float DebugTime = 99999999;
    float origMoveSpeed;
    double d_Timer = 0.0;
    double d_RepeatTimer = 0.0;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        origIdleTime = idleTime;
        origMoveSpeed = MoveSpeed;
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
                {
                    if (idleTime <= 0)
                    {
                        // Random a new position to walk to 
                        PatrolPosition = PathfinderRef.RandomPos(15);

                        return (int)CIVILIAN_STATE.PATROLLING;
                    }

                    if (DebugTime <= 0)
                        return (int)CIVILIAN_STATE.RUNNING;

                    return (int)CurrentState;
                }

            case CIVILIAN_STATE.PATROLLING:
                {
                    if (PathfinderRef.GetPathComplete())
                    {
                        idleTime = origIdleTime;
                        return (int)CIVILIAN_STATE.IDLE;
                    }
                    if (DebugTime <= 0)
                        return (int)CIVILIAN_STATE.RUNNING;

                    return (int)CurrentState;
                }

            case CIVILIAN_STATE.INTERACTING:
                {
                    return (int)CurrentState;
                }

            case CIVILIAN_STATE.PERSUADED:
                {
                    if (d_Timer <= 0)
                    {
                        d_RepeatTimer = 0.0;
                        MoveSpeed = origMoveSpeed;

                        return (int)CIVILIAN_STATE.IDLE;
                    }

                    return (int)CurrentState;
                }

            case CIVILIAN_STATE.RUNNING:
                {
                    return (int)CurrentState;
                }
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
            case (int)CIVILIAN_STATE.INTERACTING: DoInteract(); break;
            case (int)CIVILIAN_STATE.PERSUADED: DoPersuaded(); break;
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

    private void DoInteract()
    {
        FaceTowardPoint(player.transform.position, 0.5f);
    }

    private void DoPersuaded()
    {
        d_Timer -= Time.deltaTime;
        MoveSpeed = origMoveSpeed + increasedSpeed;

        // Move randomly around and emit sound
        if (PathfinderRef.GetPathFound())
        {
            PathfinderRef.FollowPath();
            d_RepeatTimer += Time.deltaTime;

            if (d_RepeatTimer >= emitSoundInterval)
            {
                GetComponent<EmitSound>().emitSound();
                d_RepeatTimer = 0.0;
            }
        }
        else
        {
            Vector3 RandPos = PathfinderRef.RandomPos(15);
            PathfinderRef.FindPath(RandPos);
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

    public void StartPersuade()
    {
        CurrentState = CIVILIAN_STATE.PERSUADED;
        d_Timer = PersuadeActionTime;
    }
}