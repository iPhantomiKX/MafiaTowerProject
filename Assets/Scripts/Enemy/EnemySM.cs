using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySM : MonoBehaviour {

	public MessageBoard theBoard;
	public Message CurrentMessage = null;     // Handle to message

	public float AttackDamage{ get; set; }
	public float AttackSpeed{ get; set; }
	public float MoveSpeed{ get; set; }
	public float HP{ get; set; }
	public bool attackAble;

	public GameObject player;
	private Rigidbody2D rb;

	public float angleFOV;
	public float visionRange;

	public Vector3 SuspiciousPosition;
	public Vector3 LastPLayerPosition;
	public Vector3 PatrolPosition;
	public float SuspiciousTime;
	public float SuspiciousMax;
	public float AlertTime;
	public bool alert;
	public float idleTime;
	public float idleAngle;

	public List<Vector3> PatrolPoints = new List<Vector3>();

	private GameStateManager GameStateRef;



	// Use this for initialization
	public virtual void Start() {
		SuspiciousPosition = Vector3.forward;
		PatrolPosition = Vector3.forward;
		SuspiciousTime = 0f;
		idleTime = 2.0f;
	}

	// Update is called once per frame
	void Update () {
		if (!GameStateRef)
			GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
		if (!player)
			player = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<PlayerController>().gameObject;
		

		if (GameStateRef.CurrentState == GameStateManager.GAME_STATE.RUNNING)
		{
			if (rb.IsSleeping())
				rb.WakeUp();

			FSM ();
		}
		else
		{
			rb.Sleep();
		}
	}

	protected void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}

	public void FSM(){


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

	public Message ReadFromMessageBoard()
	{
		if (theBoard != null)
			return theBoard.GetMessage(this.gameObject.GetInstanceID());
		else
			return null;
	}

	protected void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, visionRange);
	}

	protected bool IsPlayerSeen(){
		
		//check player in cone and in range
		Vector3 playerDir = player.transform.position - this.transform.position;
		Vector3 forward = this.transform.up;
		float angle = Vector3.Angle (playerDir, forward);
		float distance = Vector3.Distance (player.transform.position, this.transform.position);
		if (angle < angleFOV && distance < visionRange) {

			//check if player behind any obstacle
			int layerMask = (1 << 8 | 1 << 11);
			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, playerDir,Mathf.Infinity,layerMask);
			print (hit.collider.gameObject);
			if (hit.collider != null) {
				if (hit.collider.gameObject.tag == "Player") {
					return true;
				}
			} else {
				//In case part of the body is seen
				RaycastHit2D hit2 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(playerDir.z + 10f, Vector3.forward) * playerDir,Mathf.Infinity,layerMask);
				if (hit2.collider != null) {
					if (hit2.collider.gameObject.tag == "Player") {
						return true;
					}
				} else {
					RaycastHit2D hit3 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(playerDir.z - 10f, Vector3.forward) * playerDir,Mathf.Infinity,layerMask);
					if (hit3.collider != null) {
						if (hit3.collider.gameObject.tag == "Player") {
							return true;
						}
					} 
				}
			}

			return false;
		} else
			return false;
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

	protected void WalkForward(){
		rb.velocity = transform.up * 0.5f;
	}

	protected void ChangeDirection(){
		//angle is counter clockwise ,saveup for later
		float angle = Random.Range(0f, 360f);
		FaceTowardAngle (angle, 0.33f);
	}

	//need path finding
	protected void WalkTowardPoint(Vector3 point){
		this.transform.position = Vector2.MoveTowards (this.transform.position, point, MoveSpeed * Time.deltaTime);
		FaceTowardPoint (point,0.33f);
	}

	public void FaceTowardPoint(Vector3 point,float percenDelta){
		Vector3 toTarget = point - this.transform.position;
		float angle = (Mathf.Atan2 (toTarget.y, toTarget.x) * Mathf.Rad2Deg)-90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, q, percenDelta);
	}

	//Counter Clockwise
	public void FaceTowardAngle(float angle,float percenDelta){
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, q, percenDelta);
	}

	public bool IsDead(){
		return HP <= 0;
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
}


