using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSM : MonoBehaviour {

    public MessageBoard theBoard;
    public Message CurrentMessage = null;     // Handle to message

    public float MoveSpeed { get; set; }

    public GameObject player;
    protected Rigidbody2D rb;

    public List<Vector3> PatrolPoints = new List<Vector3>();

    protected GameStateManager GameStateRef;

	// Use this for initialization
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    protected void WalkTowardPoint(Vector3 point)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, point, MoveSpeed * Time.deltaTime);
        FaceTowardPoint(point, 0.33f);
    }

    public void FaceTowardAngle(float angle, float percenDelta)
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, percenDelta);
    }

    public void TakeDamage(float damage)
    {
       GetComponent<HealthComponent>().TakeDmg((int)damage);
    }

    public bool IsDead()
    {
        return GetComponent<HealthComponent>().health <= 0;
    }

}
