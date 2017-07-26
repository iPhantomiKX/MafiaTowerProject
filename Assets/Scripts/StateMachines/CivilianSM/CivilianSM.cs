using System.Collections;
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
        DEAD,
    }

    [Space]
    [Header("CivilianSM Attributes")]
    public CIVILIAN_STATE CurrentState = CIVILIAN_STATE.IDLE;
    public float increasedSpeed = 1f;
    [Tooltip("Will always be at least 2")]
    public int maxPatrolRooms = 3;

    [Space]
    public double PersuadeActionTime = 5.0;
    public float emitSoundInterval = 2f;

    float origIdleTime;
    float origMoveSpeed;
    double d_Timer = 0.0;
    double d_RepeatTimer = 0.0;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        origIdleTime = idleTime;
        origMoveSpeed = MoveSpeed;

        // Set idle to -1 to make the civilian immediately start
        idleTime = -1;

        // Get a few random rooms to move between
        int rand = Random.Range(2, maxPatrolRooms);
        for (int count = 0; count < rand; ++count)
        {
            RoomScript temp = PathfinderRef.theLevelManager.GetRandomRoom();
			float tilespacing = PathfinderRef.theLevelManager.tilespacing;

			Vector3 tempVec = new Vector3(tilespacing * Mathf.RoundToInt(temp.xpos + (temp.roomWidth * 0.5f)), tilespacing * Mathf.RoundToInt(temp.ypos + (temp.roomHeight * 0.5f)), 1f);

            if (!PatrolPoints.Contains(tempVec))
            {
                PatrolPoints.Add(tempVec);
            }
            else
                count--;
        }
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
        if (IsDead())
            return (int)CIVILIAN_STATE.DEAD;

        switch (CurrentState)
        {
            case CIVILIAN_STATE.IDLE:
                {
                    if (idleTime <= 0)
                    {
                        // Random a new position to walk to 
                        if (patrolIndex >= PatrolPoints.Count)
                        {
                            // If at the end of patrol, random a position around current pos
                            patrolIndex = -1;
                            PatrolPosition = PathfinderRef.RandomPos(3, transform.position);
                        }
                        else
                        {
                            // Move towards the next room in patrol points
                            PatrolPosition = PathfinderRef.RandomPos(3, PatrolPoints[patrolIndex]);
                        }
                            return (int)CIVILIAN_STATE.PATROLLING;
                    }

                    if (GetComponent<HealthComponent>().CalculatePercentageHealth() <= retreatThreshold)
                    {
                        PathfinderRef.Reset();
                        return (int)CIVILIAN_STATE.RUNNING;
                    }

                    return (int)CurrentState;
                }

            case CIVILIAN_STATE.PATROLLING:
                {
                    if (PathfinderRef.GetPathComplete())
                    {
                        idleTime = origIdleTime;
                  
                        patrolIndex++;
                        return (int)CIVILIAN_STATE.IDLE;
                    }

                    if (GetComponent<HealthComponent>().CalculatePercentageHealth() <= retreatThreshold)
                    {
                        PathfinderRef.Reset();
                        return (int)CIVILIAN_STATE.RUNNING;
                    }

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

                        idleTime = -1;
                        return (int)CIVILIAN_STATE.IDLE;
                    }

                    if (GetComponent<HealthComponent>().CalculatePercentageHealth() <= retreatThreshold)
                    {
                        PathfinderRef.Reset();
                        return (int)CIVILIAN_STATE.RUNNING;
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
            case (int)CIVILIAN_STATE.DEAD: DoDead(); break;
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
            Vector3 RandPos = PathfinderRef.RandomPos(15, transform.position);
            PathfinderRef.FindPath(RandPos);
        }
    }

    public void DoDead()
    {
        base.CheckForBodyDrag();
    }

    private void DoRun()
    {
        MoveSpeed = origMoveSpeed + increasedSpeed;
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

    public void StartRunning()
    {
        CurrentState = CIVILIAN_STATE.RUNNING;
        transform.parent.GetComponentInChildren<SpeechScript>().SetDisplayText(SpeechType.Damaged);
        PathfinderRef.Reset();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("PlayerSpawn") && CurrentState == CIVILIAN_STATE.RUNNING)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public override void OnDeath()
    {
        Destroy(GetComponent<CivilianPersuadeInspect>());
        Destroy(GetComponent<SpriteOutline>());
        Destroy(GetComponent<TraitObstacle>());
        base.OnDeath();
    }
}
