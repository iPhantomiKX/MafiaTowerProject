﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Pathfinder))]
[RequireComponent(typeof(HealthComponent))]
public abstract class BaseSM : MonoBehaviour {

    public MessageBoard theBoard;
    public Message CurrentMessage = null;     // Handle to message

    public float MoveSpeed = 1f;
    public Vector3 PatrolPosition;
    public float idleTime;
    public float idleAngle;
    [Tooltip("Capped between 1 and 99")]
    public float retreatThreshold;  // At what percent of health will the state machine retreat

    public GameObject player;
    protected Rigidbody2D rb;

    public List<Vector3> PatrolPoints = new List<Vector3>();
    protected int patrolIndex = 0;

    protected GameStateManager GameStateRef;

    protected Pathfinder PathfinderRef;

    public bool isBeingDragged = false;

    protected Vector2 lastGridPos;

    // Use this for initialization
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PathfinderRef = GetComponent<Pathfinder>();

        retreatThreshold = Mathf.Clamp(retreatThreshold, 1, 100);
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameStateRef)
            GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;
        if (!theBoard)
            theBoard = GameObject.Find("MessageBoard").GetComponent<MessageBoard>();
        
        if (GameStateRef.GetState() == GameStateManager.GAME_STATE.RUNNING)
        {
            FSM();
        }

        //// Remove old gridpos
        //PathfinderRef.theLevelManager.RemoveFromArray(lastGridPos);
        //// Update the LevelManager gridmap
        //lastGridPos = PathfinderRef.theLevelManager.AddToArray(transform.position, LevelManager.TileType.ENTITY);
	}
    public void FSM()
    {
        Sense();

        int actValue = Think();
        if (actValue != -1)
        {
            Act(actValue);
        }
    }

    public abstract void Sense();           // get/receive updates from the world
    public abstract int Think();            // process the updates
    public abstract void Act(int value);    // act upon any change in behaviour
    public abstract void ProcessMessage();

    public Message ReadFromMessageBoard()
    {
        if (theBoard != null)
            return theBoard.GetMessage(this.gameObject.GetInstanceID());
        else
            return null;
    }

    public void FaceTowardPoint(Vector3 point, float percenDelta)
    {
        Vector3 toTarget = point - this.transform.position;
        float angle = (Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, percenDelta);
    }

    public void WalkTowardPoint(Vector3 point)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, point, MoveSpeed * Time.deltaTime);
        FaceTowardPoint(point, 0.33f);
    }

	public void WalkPathFinder(Vector3 point){
		if (PathfinderRef.GetPathFound())
		{
			PathfinderRef.FollowPath();
		}
		else
		{
			PathfinderRef.FindPath(point);
		}
	}

    public void FaceTowardAngle(float angle, float percenDelta)
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, percenDelta);
    }

	public virtual void TakeDamage(float damage)
    {
       GetComponent<HealthComponent>().TakeDmg((int)damage);
    }

    public bool IsDead()
    {
        return GetComponent<HealthComponent>().health == 0;
    }

    protected virtual void CheckForBodyDrag()
    {
        if (!isBeingDragged)
            return;

        if (gameObject.GetComponent<DistanceJoint2D>() == null)
            gameObject.AddComponent<DistanceJoint2D>();

        gameObject.GetComponent<DistanceJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
    }

    public virtual void ToggleBodyDrag()
    {
        isBeingDragged = !isBeingDragged;
        gameObject.GetComponent<DistanceJoint2D>().enabled = isBeingDragged;
    }

    public virtual void OnDeath()
    {
        //Remember to disable speech script;
        gameObject.AddComponent<BodyInspect>();
        gameObject.AddComponent<SpriteOutline>();
        gameObject.layer = LayerMask.NameToLayer("Inspectables");
        MoveSpeed = player.GetComponent<PlayerController>().speed;
    }
}
