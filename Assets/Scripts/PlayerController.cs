using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed;
    public float mod_speed = 1; // speed added on by traits

	public static bool shootButton;
	public static bool meleeButton;
    public float inspectionRange;

    public Canvas PauseCanvasTemplate;

    private Vector2 velocity;
    private Canvas PauseCanvasRef;
    private GameStateManager GameStateRef;

    private bool IsDashing = false;
    private Vector2 DashDir;
    private float DashSpeed;
    Image currentInspectionPanel;
    public GameObject inspectingObject;

    List<Collider2D> nearObj = new List<Collider2D>();

	// Use this for initialization
	void Start () {

        Canvas cv = Instantiate(PauseCanvasTemplate) as Canvas;
        cv.gameObject.SetActive(false);
        PauseCanvasRef = cv;

        GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
        //Move player

        if (GameStateRef.CurrentState == GameStateManager.GAME_STATE.RUNNING)
        {
            if (rb.IsSleeping())
                rb.WakeUp();

            Move();
            FaceMousePos();
            Shootbutton();
			Meleebutton();
        }
        else
        {
            rb.Sleep();
        }

        GetKeyInputs();
        CheckSurroundings();

		Camera.main.gameObject.transform.position = new Vector3 (rb.position.x, rb.position.y, -10);
		Camera.main.orthographicSize = 2;
		
	}

    void Move()
    {
        Vector2 MoveDirectionLR = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        velocity = MoveDirectionLR * (speed * mod_speed);
        rb.velocity = new Vector2(velocity.x, velocity.y);

        if (IsDashing)
        {
            Vector2 newForce = DashDir * DashSpeed;
            rb.velocity = newForce;
            DashSpeed -= Time.deltaTime * 25;

            if (DashSpeed <= 0)
                IsDashing = false;
        }
    }

	void FaceMousePos()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	bool Shootbutton()
	{
		if (Input.GetMouseButtonDown (0) && inspectingObject == null) {
			shootButton = true;
		} 
		else
			shootButton = false;

		return shootButton;
	}

	bool Meleebutton()
	{
		if (Input.GetKeyDown(KeyCode.C) && inspectingObject == null) {
			meleeButton = true;
		} 
		else
			meleeButton = false;

		return shootButton;
	}

    void GetKeyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PauseCanvasRef.gameObject.SetActive(!PauseCanvasRef.gameObject.activeInHierarchy);

            if (PauseCanvasRef.gameObject.activeInHierarchy)
                GameStateRef.CurrentState = GameStateManager.GAME_STATE.PAUSED;
            else
                GameStateRef.CurrentState = GameStateManager.GAME_STATE.RUNNING;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (inspectingObject == null)
            {
                Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, inspectionRange);

                foreach (Collider2D col in obj)
                {
                    if (col != null && col.GetComponent<Inspect>() != null)
                    {
                        Debug.Log("Inspect " + col);
                        //col.GetComponent<Inspect>().inspect();
                        GameObject em = GameObject.Find("EnvironmentManager");
                        inspectingObject = col.gameObject;
                        currentInspectionPanel = em.GetComponent<InspectionManager>().CreateInspectionMenu(col.gameObject);

                        break;
                    }

                }
            }
            else if(currentInspectionPanel != null)
            {
                Destroy(currentInspectionPanel.gameObject);
                currentInspectionPanel = null;
                inspectingObject = null;
            }
        }
    }

    void CheckSurroundings()
    {
        // Get objs nearby
        Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, inspectionRange);
        
        List<Collider2D> temp = new List<Collider2D>();
        foreach (Collider2D col in obj)
        {
            temp.Add(col);
        }
        if(inspectingObject != null && !temp.Contains(inspectingObject.GetComponent<Collider2D>()))
        {
            Destroy(currentInspectionPanel.gameObject);
            currentInspectionPanel = null;
            inspectingObject = null;
        }
        // Check if any objects in nearObj are not near anymore
        foreach (Collider2D col in nearObj)
        {
            if (!temp.Contains(col))
            {
                if (col != null && col.GetComponent<Inspect>() != null)
                    col.GetComponent<Inspect>().outline(false);
                
            }
        }

        nearObj.Clear();
        nearObj = temp;

        // Find inspectable objects
        foreach (Collider2D col in obj)
        {
            if (!nearObj.Contains(col))
                nearObj.Add(col);

            if (col != null && col.GetComponent<Inspect>() != null)
            {
                col.GetComponent<Inspect>().outline(true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, inspectionRange);
    }

    public void SetDash(Vector2 dir, float force)
    {
        IsDashing = true;

        DashDir = dir;
        DashSpeed = force;
    }

    void OnCollisionEnter()
    {
        IsDashing = false;

    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(currentInspectionPanel != null && other.gameObject.GetComponent<Inspect>() != null)
        {
            Destroy(currentInspectionPanel.gameObject);
            currentInspectionPanel = null;
            inspectingObject = null;
        }
    }
}
