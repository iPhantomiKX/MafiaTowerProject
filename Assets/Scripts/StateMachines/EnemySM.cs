using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySM : BaseSM {

	public enum ENEMY_ROLE
	{
		GUARD,
		PATROL,
		WANDER
	}

	public ENEMY_ROLE role = ENEMY_ROLE.PATROL;

	public float AttackDamage{ get; set; }
	public float AttackSpeed{ get; set; }
	public bool attackAble;
	public bool isHunting;

	public float angleFOV;
	public float visionRange;

	public Vector3 SuspiciousPosition;
	public Vector3 LastPLayerPosition;
	public float SuspiciousTime;
	public float SuspiciousMax;
	public float AlertTime;
	public bool alert;

	public float searchTime;
	public int searchIndex;
	public List<Vector3> SearchingRoute = new List<Vector3> ();


	public float detectGauge;
	public float detectValue;

	public bool knowPlayerPosition;


	public List<GameObject> VIPs = new List<GameObject> ();
	public GameObject CurrentTarget;

	// Use this for initialization
	public virtual void Start() {
		SuspiciousPosition = Vector3.forward;
		PatrolPosition = Vector3.forward;
		SuspiciousTime = 0f;
		AlertTime = 0f;
		searchTime = 0f;
		searchIndex = -1;
		idleTime = 2.0f;
		detectGauge = 0f;
		detectValue = 0f;
		knowPlayerPosition = false;
		isHunting = false;
	}

	protected void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, visionRange);
	}

	protected bool IsTargetSeen(GameObject target){
		//check player in cone and in range
		Vector3 targetDir = target.transform.position - this.transform.position;
		Vector3 forward = this.transform.up;
		float angle = Vector2.Angle (targetDir, forward);
		float distance = Vector2.Distance (target.transform.position, this.transform.position);
		if (angle < angleFOV && distance < visionRange) {
			//check if player behind any obstacle
			int layerMask = (1 << 8 | 1 << 11 | 1 << 12);
			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, targetDir,Mathf.Infinity,layerMask);
			if (hit.collider != null) {
				if (CheckValidTarget(hit.collider.gameObject.tag,target)) 
				{
					return true;
				}
			} else {
				//In case part of the body is seen
				RaycastHit2D hit2 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(targetDir.z + 10f, Vector3.forward) * targetDir,Mathf.Infinity,layerMask);
				if (hit2.collider != null) {
					if (CheckValidTarget(hit2.collider.gameObject.tag,target)) 
					{
						return true;
					}
				} else {
					RaycastHit2D hit3 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(targetDir.z - 10f, Vector3.forward) * targetDir,Mathf.Infinity,layerMask);
					if (hit3.collider != null) {
						if (CheckValidTarget(hit3.collider.gameObject.tag,target))
						{
							return true;
						}
					} 
				}
			}

			return false;
		}
		return false;
	}

	protected bool IsPlayerSeen(){
		if (!alert) {
			if (detectValue > 0f) {
				detectValue = 0f;
			} else {
				ReduceDetectGauge ();
			}
		} else if(isHunting){
			//isHunting = false;
			Debug.Log("Enter IsPlayerSeen and isHunting");
			PlayerController.hunted++;
		}
		Debug.Log("Enter IsPlayerSeen");
		return IsTargetSeen (player);
	}

	protected void ReduceDetectGauge(){
		detectGauge -= Time.deltaTime / 5f;
		if (detectGauge <= 0) {
			detectGauge = 0;
		}
	}

	protected bool IsVIPSeen(){
		if (VIPs.Count <= 0) {
			GameObject[] goList = GameObject.FindGameObjectsWithTag ("VIP");

			foreach (GameObject go in goList) {


				if (VIPs.Contains (go)) {
					continue;
				}
				VIPs.Add (go);
			}
		}
		for (int i = 0; i < VIPs.Count; i++) {
			if (VIPs [i] == null) {
				VIPs.RemoveAt (i);
			}
		}
		if (VIPs.Count > 0) {
			foreach (GameObject vip in VIPs) {
				if(IsTargetSeen(vip)){
					return true;
				}
			}
		}
		return false;
	}

	bool CheckValidTarget(string tag,GameObject target)
    {
        switch (tag)
        {
		case "Player": 
			if (alert) {
				Debug.Log ("Enter CheckValidTarget");
				CurrentTarget = player;
				isHunting = true;
				return true;
			} else {
				//Last float value will be depend on player action.
				detectValue = 0.3f;
				Animator anim = player.GetComponent<Animator> ();
				if (anim.GetCurrentAnimatorStateInfo (0).IsName ("PlayerShooting")) {
					detectValue = 2f;
				} else if (anim.GetCurrentAnimatorStateInfo (0).IsName ("PlayerStabbing")) {
					detectValue = 1.2f;
				} else if (player.GetComponent<PlayerController>().inspectingObject != null) {
					detectValue = 0.4f;
				} else {
					detectValue = 0.1f;
				}
				detectGauge += (Time.deltaTime / Vector2.Distance (this.transform.position, player.transform.position)) * detectValue;
				if (detectGauge >= 1f) {
					CurrentTarget = player;
					isHunting = true;
					return true;
				} else {
					return false;
				}
			}
		case "VIP":
			CurrentTarget = target;
			return true;

		default: 
			CurrentTarget = null;
			return false;
        }
    }

	protected bool IsSuspicuous(){
		if (SuspiciousPosition != Vector3.forward) {
			return true;
		} else
			return false;
	}

	public void StopSuspicious(){
		SuspiciousPosition = Vector3.forward;
		SuspiciousTime = 0;
		SuspiciousMax = 0;
	}

	public void StartSuspicious(Vector3 pos,float t){
		SuspiciousPosition = pos;
		PathfinderRef.Reset ();
		SuspiciousMax = t;
		SuspiciousTime = t;
	}


	public void StopSearching(){

		if (searchIndex == -1) {
			return ;
		}

		searchTime = 0f;
		searchIndex = -1;
		SearchingRoute.Clear ();
		LastPLayerPosition = Vector3.forward;
		if (isHunting) {
			isHunting = false;
			PlayerController.hunted--;
		}
	}

	protected void WalkForward(){
		rb.velocity = transform.up * 0.5f;
	}

	protected void ChangeDirection(){
		//angle is counter clockwise ,saveup for later
		float angle = Random.Range(0f, 360f);
		FaceTowardAngle (angle, 0.33f);
	}

	public void ResetAttack(){
		attackAble = true;
	}

	public void AddPatrolPoint(Vector3 point){
		PatrolPoints.Add (point);
	}

	public void ClearPatrolPoint(){
		PatrolPoints.Clear ();
	}

	public override void TakeDamage (float damage)
	{
		base.TakeDamage (damage);
		knowPlayerPosition = true;
		CurrentTarget = player;
	}
}


