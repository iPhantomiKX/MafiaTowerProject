﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    //Player Mods
    public float speed;
    public float base_speed = 1;
    public float carrying_speed = 0.5f; //speed if carrying heavy object
    public float mod_speed = 1; // speed added on by traits

    //Button stuff
	public static bool shootButton;
	public static bool meleeButton;

    //Canvas UI stuff
    public Canvas PauseCanvasTemplate;
    private Canvas PauseCanvasRef;

    //Game State
    private GameStateManager GameStateRef;

    //Inspection stuff
    public float inspectionRange;
    Image currentInspectionPanel;
    public GameObject inspectingObject;
    List<Collider2D> nearObj = new List<Collider2D>();

    //Player movement
    private MovementScript Movement;

    //Quick bool to freeze update
    public bool freeze = false;

    //For moving through vents
    public bool inVent = false;

    //For Body Dragging and dumping
    public bool dragging = false;
    public GameObject draggedObject = null;

    /*I have to leave this here cause of PlayerAnimationController class 
        - I think I'll integrate the animation controller stuff into here
        - Either that or I just make a "GetRigidbody" function
    */
    public Rigidbody2D rb;

	//Sound stuff
	public static int hunted;
	public AudioSource bkSource;
	public AudioClip bkPeacefulSound;
	public AudioClip bkBattleSound;

	void Awake(){
		hunted = 0;
		bkSource = this.transform.parent.GetComponent<AudioSource> ();
	}

    // Use this for initialization
    void Start () {

        Canvas cv = Instantiate(PauseCanvasTemplate) as Canvas;
        cv.gameObject.SetActive(false);
        PauseCanvasRef = cv;

        GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();

        Movement = GetComponent<MovementScript>();

    }

    // Update is called once per frame
    void Update () {
		//Debug.Log ("Count hunted " + hunted);
        if (freeze)
            return;

        //Move player
        if (GameStateRef.GetState() == GameStateManager.GAME_STATE.RUNNING)
        {
            Move();
            FaceMousePos();
            Shootbutton();
			Meleebutton();
			UpdateBackgroundSound ();
        }

        GetKeyInputs();
        CheckSurroundings();
		
	}

    void Move()
    {
        //Movement
        Movement.Move
            (
            new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), //Direction
            (speed * mod_speed)                                                  //Speed
            );
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
		if (Input.GetMouseButtonDown (1) && inspectingObject == null) {
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
            {
                GameStateRef.SetState(GameStateManager.GAME_STATE.PAUSED);
                Time.timeScale = 0;
            }
            else
            {
                GameStateRef.SetState(GameStateManager.GAME_STATE.RUNNING);
                Time.timeScale = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (inspectingObject == null)
            {
                int layerMaskInspect = 1 << LayerMask.NameToLayer("Inspectables");
                int layerMaskVent = 1 << LayerMask.NameToLayer("Vent_Player");

                int layerMask = layerMaskInspect | layerMaskVent;

                Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, inspectionRange, layerMask);
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D mouseOver = Physics2D.OverlapPoint(mouse, layerMask);
                Debug.Log("Mouse is on object: = " + mouseOver);
                foreach (Collider2D col in obj)
                {
                    if (col != null && col.GetComponent<Inspect>() != null && mouseOver != null && col.gameObject == mouseOver.gameObject)
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
        //Added by RANDALL - If checks are added by meeeee. Mwahahahaa. 
        //Quick checks to only take objects with colliders that are on the same layer as the player
        // Get objs nearby
        Collider2D[] obj;
        if (inVent)
            obj = Physics2D.OverlapCircleAll(transform.position, inspectionRange, 1 << LayerMask.NameToLayer("Vent_Player"));
        else
            obj = Physics2D.OverlapCircleAll(transform.position, inspectionRange, 1 << LayerMask.NameToLayer("Inspectables") | 1 << LayerMask.NameToLayer("Enemy"));

        List<Collider2D> temp = new List<Collider2D>();
        foreach (Collider2D col in obj)
        {
            temp.Add(col);
        }
        if(inspectingObject != null && !temp.Contains(inspectingObject.GetComponent<Collider2D>()))
        {
            Debug.Log("Object is longer in player's range");
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
                if (col.gameObject.name == "Civilian")
                    Debug.Log("found");

                col.GetComponent<Inspect>().outline(true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, inspectionRange);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //if(currentInspectionPanel != null && other.gameObject.GetComponent<Inspect>() != null)
        //{
        //    Destroy(currentInspectionPanel.gameObject);
        //    currentInspectionPanel = null;
        //    inspectingObject = null;
        //}
    }

    //Trait_Dash requires this
    public void SetDash(Vector2 something, float otherthing)
    {
        //Sorry, working on this right now. Don't want conflicts from Version Control.
    }

	private void UpdateBackgroundSound(){
		if (hunted > 0 && bkSource.clip == bkPeacefulSound) {
			bkSource.clip = bkBattleSound;
			bkSource.Play ();
		} else if(hunted == 0 && bkSource.clip == bkBattleSound){
			bkSource.clip = bkPeacefulSound;
			bkSource.Play ();
		}
	}

}
