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

	// Use this for initialization
	public override void Start () {
		base.Start ();
		AttackDamage = 3f;
		AttackSpeed = 1.9f;
		MoveSpeed = 0.5f;
		HP = 10f;
		attackAble = true;
	}

	public override void Sense ()
	{
		Message temp = ReadFromMessageBoard();
		if (temp != null)
		{
			CurrentMessage = temp;	
		}
	}

	public override int Think(){
		print (CurrentState);
		switch (CurrentState)
		{                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
			//IDLE -> PATROL,SUS,ATTACK,DEAD
		case ENEMY_STATE.IDLE:
			if (IsDead ())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen ())
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
			if (IsPlayerSeen ())
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
			if (IsPlayerSeen())
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
			if(!IsPlayerSeen())
				return (int)ENEMY_STATE.SEARCHING;
			return (int)ENEMY_STATE.ATTACKING;

			//SEARCHING -> SUS, ATTACK, IDLE,DEAD
		case ENEMY_STATE.SEARCHING:
			if (IsDead())
				return (int)ENEMY_STATE.DEAD;
			if (IsPlayerSeen())
				return (int)ENEMY_STATE.ATTACKING;
			if (IsSuspicuous())
				return(int)ENEMY_STATE.SUSPICIOUS;
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
			break;

		case (int)ENEMY_STATE.PATROLLING:
			CurrentState = ENEMY_STATE.PATROLLING;
			if (PatrolPoints.Count <= 0 || PatrolPosition == Vector3.forward) {
				idleTime = 1f;
				break;
			}
			if (Vector2.Distance (this.transform.position, PatrolPosition) > 0.2f) {
				WalkTowardPoint (PatrolPosition);
			} else {
				idleTime = 1f;
				PatrolPosition = Vector3.forward;
			}

			break;

		case (int)ENEMY_STATE.SUSPICIOUS:
			CurrentState = ENEMY_STATE.SUSPICIOUS;
			if (Vector2.Distance (this.transform.position, SuspiciousPosition) < 0.3f) {
				SuspiciousTime -= Time.deltaTime;
				float angle;
				if (SuspiciousTime > 8) {
				} else if (SuspiciousTime > 6) {
					angle = 0;
					FaceTowardAngle (angle, 0.10f);
				} else if (SuspiciousTime > 4) {
					angle = 180;
					FaceTowardAngle (angle, 0.10f);
				} else if (SuspiciousTime > 2) {
					angle = 90;
					FaceTowardAngle (angle, 0.10f);
				} else if (SuspiciousTime > 0) {
					angle = 270;
					FaceTowardAngle (angle, 0.10f);
				} else {
					SuspiciousTime = 0;
					StopSuspicious ();
					idleTime = 1f;
				}
			} else
				WalkTowardPoint (SuspiciousPosition);
			break;

		case (int)ENEMY_STATE.ATTACKING:
			CurrentState = ENEMY_STATE.ATTACKING;
			if (!alert)
				alert = true;
			StopSuspicious ();
			WalkTowardPoint (player.transform.position);
			if (attackAble) {
				if (Vector2.Distance (this.transform.position, player.transform.position) < 0.4f) {
					player.GetComponent<HealthComponent> ().health -= 1;

					attackAble = false;
					Invoke ("ResetAttack", AttackSpeed);
				}

			}

			break;

		case (int)ENEMY_STATE.SEARCHING:
			CurrentState = ENEMY_STATE.IDLE;
			break;

		case (int)ENEMY_STATE.DEAD:
			CurrentState = ENEMY_STATE.DEAD;
			this.GetComponent<CircleCollider2D> ().enabled = false;
			Destroy (this.gameObject);
			break;
		}
	}

}
