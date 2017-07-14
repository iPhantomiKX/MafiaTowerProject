using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySM : BaseSM {

	public float AttackDamage{ get; set; }
	public float AttackSpeed{ get; set; }
	public bool attackAble;

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
		knowPlayerPosition = false;
	}

	protected void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, visionRange);
	}

	protected bool IsTargetSeen(GameObject target){
		//check player in cone and in range
		Vector3 targetDir = target.transform.position - this.transform.position;
		Vector3 forward = this.transform.up;
		float angle = Vector3.Angle (targetDir, forward);
		float distance = Vector3.Distance (target.transform.position, this.transform.position);
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
		} else
			return false;
	}

	protected bool IsPlayerSeen(){
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
				CurrentTarget = player;
				return true;
			} else {
				//Last float value will be depend on player action.
				detectGauge += (Time.deltaTime / Vector2.Distance (this.transform.position, player.transform.position)) * 0.3f;
				if (detectGauge >= 1f) {
					CurrentTarget = player;
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
		SuspiciousMax = t;
		SuspiciousTime = t;
	}


	public void StopSearching(){
		searchTime = 0f;
		searchIndex = -1;
		SearchingRoute.Clear ();
		LastPLayerPosition = Vector3.forward;
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


