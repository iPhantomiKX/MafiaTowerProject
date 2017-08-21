using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemySM {

	public enum ENEMY_STATE
	{
		IDLE,
		PATROLLING,
		SUSPICIOUS,
		ATTACKING,
		SEARCHING,
		DEAD
	}
		
	public ENEMY_STATE CurrentState = ENEMY_STATE.IDLE;
	public Animator animator;
	public int maxPatrolRooms = 3;

	void Awake(){
		animator = this.gameObject.transform.GetChild (0).GetComponent<Animator>();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		AttackDamage = 3f;
		AttackSpeed = 1.3f;
		MoveSpeed = 1f;
		attackAble = true;

		int rand = Random.Range(2, maxPatrolRooms);
		for (int count = 0; count < rand; ++count)
		{
			RoomScript temp;
			if (count == 0) {
				temp = PathfinderRef.theLevelManager.GetObjtRooms ();
			} else {
				int random = Random.Range (1, 3);
				if (random == 1) {
					temp = PathfinderRef.theLevelManager.GetRandomRoom ();
				} else {
					temp = PathfinderRef.theLevelManager.GetObjtRooms ();
				}
			}
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

	public override int Think(){
		//print (CurrentState);
		switch (CurrentState)
		{                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
			//IDLE -> PATROL,SUS,ATTACK,DEAD
		case ENEMY_STATE.IDLE:
			if (IsDead ())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen () || knowPlayerPosition || IsVIPSeen())
				return (int)ENEMY_STATE.ATTACKING;
			if (IsSuspicuous ())
				return (int)ENEMY_STATE.SUSPICIOUS;
			if (idleTime <= 0)
				return (int)ENEMY_STATE.PATROLLING;
			return (int)ENEMY_STATE.IDLE;

			//PATROL -> IDLE ,SUS,ATTACK,DEAD
		case ENEMY_STATE.PATROLLING:
			if (IsDead ())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen () || knowPlayerPosition || IsVIPSeen())
				return (int)ENEMY_STATE.ATTACKING;
			if (IsSuspicuous ())
				return(int)ENEMY_STATE.SUSPICIOUS;
			if (idleTime > 0)
				return (int)ENEMY_STATE.IDLE;
			return (int)ENEMY_STATE.PATROLLING;

			//SUS -> IDLE , ATTACK ,DEAD,SEARCHING
		case ENEMY_STATE.SUSPICIOUS:
			if (IsDead())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen() || knowPlayerPosition || IsVIPSeen())
				return (int)ENEMY_STATE.ATTACKING;
			if (!IsSuspicuous() && !alert)		
				return(int)ENEMY_STATE.IDLE;
			if (!IsSuspicuous() && alert)		
				return(int)ENEMY_STATE.SEARCHING;
			return (int)ENEMY_STATE.SUSPICIOUS;

			//ATTACK -> SEARCHING ,DEAD
		case ENEMY_STATE.ATTACKING:
			if (IsDead())
				return (int)ENEMY_STATE.DEAD;
			if(!IsPlayerSeen() && !knowPlayerPosition && !IsVIPSeen())
				return (int)ENEMY_STATE.SEARCHING;
			return (int)ENEMY_STATE.ATTACKING;

			//SEARCHING -> SUS, ATTACK, IDLE,DEAD
		case ENEMY_STATE.SEARCHING:
			if (IsDead ())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen () || knowPlayerPosition || IsVIPSeen())
				return (int)ENEMY_STATE.ATTACKING;
			if (IsSuspicuous ())
				return(int)ENEMY_STATE.SUSPICIOUS;
			if (LastPLayerPosition == Vector3.forward)
				return(int)ENEMY_STATE.IDLE;
			return (int)ENEMY_STATE.SEARCHING;

			//Dead
		case ENEMY_STATE.DEAD:
			return (int)ENEMY_STATE.DEAD;

		default:
			return -1;
		}
	}
		
	public override void Act(int value){
		switch (value)
		{

		case (int)ENEMY_STATE.IDLE:
			animator.SetBool ("isSuspicious", false);
			CurrentState = ENEMY_STATE.IDLE;
			DoIdle ();
			break;

		case (int)ENEMY_STATE.PATROLLING:
			animator.SetBool ("isSuspicious", false);
			CurrentState = ENEMY_STATE.PATROLLING;
			DoPatrol ();
			break;

		case (int)ENEMY_STATE.SUSPICIOUS:
			animator.SetBool ("isSuspicious" , true);
			CurrentState = ENEMY_STATE.SUSPICIOUS;
			DoSuspicious();
			break;

		case (int)ENEMY_STATE.ATTACKING:
			animator.SetBool ("isSuspicious" , true);
			CurrentState = ENEMY_STATE.ATTACKING;
			DoAttacking ();
			break;

		case (int)ENEMY_STATE.SEARCHING:
			animator.SetBool ("isSuspicious" , true);
			CurrentState = ENEMY_STATE.SEARCHING;
			DoSearching ();
			break;

		case (int)ENEMY_STATE.DEAD:
			animator.SetBool ("isDead", true);
			CurrentState = ENEMY_STATE.DEAD;
			DoDead ();
			break;
		}
	}

	private void DoIdle(){
		idleTime -= Time.deltaTime;

		FaceTowardAngle (idleAngle, 0.03f);

		if (idleTime <= 0) {
			if (role == ENEMY_ROLE.PATROL) {
				float rand = Random.Range (0f, 100f);
				if (rand <= 25 && PatrolPoints.Count > 0) {
					idleTime = 0;
					if (patrolIndex >= PatrolPoints.Count) {
						// If at the end of patrol, random a position around current pos
						patrolIndex = -1;
						PatrolPosition = PathfinderRef.RandomPos (3, transform.position);
					} else {
						// Move towards the next room in patrol points
						PatrolPosition = PathfinderRef.RandomPos (3, PatrolPoints [patrolIndex]);
					}
				} else {
					idleTime = rand / 25;
					float r_angle = Random.Range (0f, 360f);
					idleAngle = r_angle;
				}
			} else if (role == ENEMY_ROLE.WANDER) {
				float rand = Random.Range (0f, 100f);
				if (rand <= 25) {
					idleTime = 0;
					PatrolPosition = PathfinderRef.RandomPos (15, transform.position);
				} else {
					idleTime = rand / 25;
					float r_angle = Random.Range (0f, 360f);
					idleAngle = r_angle;
				}
			} else if (role == ENEMY_ROLE.GUARD) {
				float rand = Random.Range (0f, 100f);
				if (rand <= 25) {
					idleTime = 0;
					PatrolPosition = PathfinderRef.RandomPos (3, SpawnPoint);
				} else {
					idleTime = rand / 25;
					float r_angle = Random.Range (0f, 360f);
					idleAngle = r_angle;
				}
			}
		}
	}

	private void DoPatrol(){
		if (PatrolPoints.Count <= 0 || PatrolPosition == Vector3.forward) {
			idleTime = 1f;
			return;
		}
		if (Vector2.Distance (this.transform.position, PatrolPosition) > 0.2f) {
			//WalkTowardPoint (PatrolPosition);
			WalkPathFinder(PatrolPosition);
		} else {
			patrolIndex++;
			idleTime = 1f;
			PatrolPosition = Vector3.forward;
			PathfinderRef.Reset ();
		}

	}

	private void DoSuspicious(){
		StopSearching ();
		if (Vector2.Distance (this.transform.position, SuspiciousPosition) < 0.3f) {
			SuspiciousTime -= Time.deltaTime;
			float angle;
			float delayAct = SuspiciousMax / 5;
			if (SuspiciousTime > SuspiciousMax * 8/10) {
			} else if (SuspiciousTime > SuspiciousMax *6/10) {
				angle = 0;
				FaceTowardAngle (angle, 0.1f);
			} else if (SuspiciousTime > SuspiciousMax *4/10) {
				angle = 180;
				FaceTowardAngle (angle, 0.1f);
			} else if (SuspiciousTime > SuspiciousMax * 2/10) {
				angle = 90;
				FaceTowardAngle (angle, 0.1f);
			} else if (SuspiciousTime > 0) {
				angle = 270;
				FaceTowardAngle (angle, 0.1f);
			} else {
				SuspiciousTime = 0;
				StopSuspicious ();
				idleTime = 1f;
				PathfinderRef.Reset ();
			}
		} else
			//WalkTowardPoint (SuspiciousPosition);
			WalkPathFinder(SuspiciousPosition);
	}

	private void DoAttacking(){
		PathfinderRef.Reset ();
		knowPlayerPosition = false;
		if (CurrentTarget) {
			if (CurrentTarget == player) {
				if (!alert)
					alert = true;
				StopSuspicious ();
				StopSearching ();
				LastPLayerPosition = player.transform.position;
				AlertTime += Time.deltaTime;
				if (AlertTime >= 3) {
					//Push Message in MessageBoard
					List<EnemySM> enems = theBoard.getEnemyList ();
					foreach (EnemySM enem in enems) {
						if (enem == this.GetComponent<EnemySM> ())
							continue;
						Message aMessage = new Message ();
						aMessage.theMessageType = Message.MESSAGE_TYPE.ENEMY_SPOTPLAYER;
						aMessage.theSender = this.gameObject;
						aMessage.theReceiver = enem.gameObject;
						aMessage.theTarget = null;
						aMessage.theDestination = LastPLayerPosition;

						theBoard.AddMessage (aMessage);
					}
					AlertTime = 0f;
				}
			} else if (CurrentTarget.tag == "VIP") {
				StopSuspicious ();
				StopSearching ();
				LastPLayerPosition = Vector3.forward;
			}

			WalkTowardPoint (CurrentTarget.transform.position);
			if (attackAble) {
				if (Vector2.Distance (this.transform.position, CurrentTarget.transform.position) < 0.4f) {
					animator.Play ("EnemyMeleeAttack");
					CurrentTarget.GetComponent<HealthComponent> ().TakeDmg ((int)AttackDamage);
					attackAble = false;
					Invoke ("ResetAttack", AttackSpeed);
				}
			}


		}
	}

	private void DoSearching(){
		AlertTime = 0f;
		if (LastPLayerPosition == Vector3.forward)
			return;
		if (SearchingRoute.Count <= 0) {
			searchIndex = 0;
			if (PathfinderRef.GetPathComplete()) {
				int layerMask = 1 << 8;
				RaycastHit2D hit1 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis (this.transform.up.z, Vector3.forward) * this.transform.up, 2f, layerMask);
				RaycastHit2D hit2 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis (this.transform.up.z + 45f, Vector3.forward) * this.transform.up, 2f, layerMask);
				RaycastHit2D hit3 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis (this.transform.up.z + 90f, Vector3.forward) * this.transform.up, 2f, layerMask);
				RaycastHit2D hit4 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis (this.transform.up.z - 45f, Vector3.forward) * this.transform.up, 2f, layerMask);
				RaycastHit2D hit5 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis (this.transform.up.z - 90f, Vector3.forward) * this.transform.up, 2f, layerMask);
				//Raycast Check 5 way and walk to
				Vector3 pos = Vector3.forward;

				if (hit1.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit1.collider.transform.position) < 0.4)) {
						pos = ((hit1.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward && PathfinderRef.ValidPos(pos)) {
					SearchingRoute.Add (pos);
				}
				if (hit2.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit2.collider.transform.position) < 0.4)) {
						pos = ((hit2.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z +45f, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward && PathfinderRef.ValidPos(pos)) {
					SearchingRoute.Add (pos);
				}
				if (hit3.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit3.collider.transform.position) < 0.4)) {
						pos = ((hit3.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z +90f, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward && PathfinderRef.ValidPos(pos)) {
					SearchingRoute.Add (pos);
				}
				if (hit4.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit4.collider.transform.position) < 0.4)) {
						pos = ((hit4.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z -45f, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward && PathfinderRef.ValidPos(pos)) {
					SearchingRoute.Add (pos);
				}
				if (hit5.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit5.collider.transform.position) < 0.4)) {
						pos = ((hit5.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z -90f, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward && PathfinderRef.ValidPos(pos)) {
					SearchingRoute.Add (pos);
				}
				if (SearchingRoute.Count <= 0) {
					StopSearching ();
				} else {
					searchTime = 2f;
					searchIndex = 0;
				}
			} else {
				//WalkTowardPoint (LastPLayerPosition);
				WalkPathFinder(LastPLayerPosition);
			}
		} else {
			if (PathfinderRef.GetPathComplete()) {
				searchTime -= Time.deltaTime;
				if (searchTime <= 0) {
					searchIndex += 1;
					PathfinderRef.Reset ();
					if (SearchingRoute.Count > searchIndex) {
						searchTime = 2f;
					} else {
						StopSearching();
					}
				}
			} else {
				WalkPathFinder (SearchingRoute [searchIndex]);
			}

		}

	}

	private void DoDead()
	{
		
		if (this.GetComponent<LineRenderer> ().enabled) {
			this.GetComponent<LineRenderer> ().enabled = false;
			OnDeath ();
		}
        base.CheckForBodyDrag();
    }

    public override void ProcessMessage ()
	{
		if (CurrentMessage == null)
		{

			return;

		}

		switch (CurrentMessage.theMessageType)
		{

		case Message.MESSAGE_TYPE.ENEMY_SPOTPLAYER:
			knowPlayerPosition = true;
			LastPLayerPosition = CurrentMessage.theDestination;
			CurrentTarget = player;
			break;

		}
		CurrentMessage = null;
	}

}
