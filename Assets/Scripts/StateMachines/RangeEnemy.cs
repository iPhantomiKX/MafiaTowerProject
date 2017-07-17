using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemySM {

	public GameObject bulletPrefab;

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

	// Use this for initialization
	public override void Start () {
		base.Start ();
		AttackDamage = 5f;
		AttackSpeed = 1.9f;
		MoveSpeed = 1f;
		attackAble = true;
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
			CurrentState = ENEMY_STATE.IDLE;
			DoIdle ();
			break;


		//Currently best in 2 point
		case (int)ENEMY_STATE.PATROLLING:
			CurrentState = ENEMY_STATE.PATROLLING;
			DoPatrol ();
			break;

		case (int)ENEMY_STATE.SUSPICIOUS:
			CurrentState = ENEMY_STATE.SUSPICIOUS;
			DoSuspicious ();
			break;

		case (int)ENEMY_STATE.ATTACKING:
			CurrentState = ENEMY_STATE.ATTACKING;
			DoAttacking ();
			break;

		//WIP
		case (int)ENEMY_STATE.SEARCHING:
			CurrentState = ENEMY_STATE.SEARCHING;
			DoSearching ();
			break;

		case (int)ENEMY_STATE.DEAD:
			CurrentState = ENEMY_STATE.DEAD;
			DoDead ();
			break;
		}
	}

	private void DoIdle(){
		idleTime -= Time.deltaTime;

		FaceTowardAngle (idleAngle, 0.03f);

		if (idleTime <= 0) {
			float rand = Random.Range (0f, 100f);
			if (rand <= 25 && PatrolPoints.Count > 0) {
				idleTime = 0;
				float nearest = 9999f;
				float tempF;
				int indexT = -1;
				for (int i = 0; i < PatrolPoints.Count; i++) {
					tempF = Vector2.Distance (this.transform.position, PatrolPoints [i]);
					if (tempF < 0.2)
						continue;
					if (tempF < nearest) {
						nearest = tempF;
						indexT = i;
					}
				}
				if(indexT>=0)
					PatrolPosition = PatrolPoints [indexT];
			} else {
				idleTime = rand / 25;
				float r_angle = Random.Range (0f, 360f);
				idleAngle = r_angle;
			}
		}
	}

	private void DoPatrol(){
		if (PatrolPoints.Count <= 0 || PatrolPosition == Vector3.forward) {
			idleTime = 1f;
			return;
		}
		if (Vector2.Distance (this.transform.position, PatrolPosition) > 0.2f) {
			WalkTowardPoint (PatrolPosition);
		} else {
			idleTime = 1f;
			PatrolPosition = Vector3.forward;
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
			}
		} else
			WalkTowardPoint (SuspiciousPosition);
	}

	private void DoAttacking(){
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

			if (Vector2.Distance (this.transform.position, CurrentTarget.transform.position) > 0.7f)
				WalkTowardPoint (CurrentTarget.transform.position);
			else
				FaceTowardPoint (CurrentTarget.transform.position, 0.33f);
			if (attackAble) {
				if (Vector2.Distance (this.transform.position, CurrentTarget.transform.position) <= 1f) {
					GameObject go = Instantiate (bulletPrefab, this.transform.position + (transform.up * 0.3f), this.transform.rotation);
					go.GetComponent<EnemyBullet> ().Damage = AttackDamage;
					Physics2D.IgnoreCollision (go.GetComponent<Collider2D> (), this.GetComponent<Collider2D> ());
					go.GetComponent<Rigidbody2D> ().AddForce (this.transform.up * 400f);
					//player.GetComponent<HealthComponent> ().health -= (int)AttackDamage;

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
			if (this.transform.position == LastPLayerPosition) {
				rb.velocity = Vector3.zero;
				rb.angularVelocity = 0;
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
				if (pos != Vector3.forward) {
					SearchingRoute.Add (pos);
				}
				if (hit2.collider != null) {
					if (!(Vector2.Distance (this.transform.position, hit2.collider.transform.position) < 0.4)) {
						pos = ((hit2.collider.transform.position - this.transform.position) * 7 / 10) + this.transform.position;
					} else {
						pos = Vector3.forward;
					}
				} else {
					pos = this.transform.position + (Quaternion.AngleAxis (this.transform.up.z + 45f, Vector3.forward) * this.transform.up * 1.7f);
				}
				if (pos != Vector3.forward) {
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
				if (pos != Vector3.forward) {
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
				if (pos != Vector3.forward) {
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
				if (pos != Vector3.forward) {
					SearchingRoute.Add (pos);
				}
				if (SearchingRoute.Count <= 0) {
					StopSearching ();
				} else {
					searchTime = 2f;
					searchIndex = 0;
				}
			} else {
				WalkTowardPoint (LastPLayerPosition);
			}
		} else {
			if (Vector2.Distance (this.transform.position, SearchingRoute [searchIndex]) < 0.3) {
				searchTime -= Time.deltaTime;
				if (searchTime <= 0) {
					searchIndex += 1;
					if (SearchingRoute.Count > searchIndex) {
						searchTime = 2f;
					} else {
						StopSearching();
					}
				}
			} else {
				WalkTowardPoint (SearchingRoute [searchIndex]);
			}

		}

	}

	private void DoDead(){
		this.GetComponent<CircleCollider2D> ().enabled = false;
		Destroy (this.transform.parent.gameObject,1);
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
