using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {

	public ObjectiveManager om;
	public string objtname{ get; set; }
	public bool complete{ get; set;}
	public bool isTimed;
    public float time;
    public float remainingTime;
    //public Rect timeBar;
    public RectTransform timeBar;
    public float timeBarWidth;

    private GameStateManager GameStateRef;

	// Use this for initialization
	public virtual void Start () {
        Debug.Log("Initiated objective: " + this.objtname);
        if (this.isTimed)
        {
            this.remainingTime = this.time;
            Debug.Log("Timer started. Remaining: " + this.remainingTime + " seconds");
        }
        om = GameObject.FindObjectOfType<ObjectiveManager>();
        GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(this.isTimed && timeBar != null)
        {
            if (GameStateRef.CurrentState == GameStateManager.GAME_STATE.RUNNING)
            {
                remainingTime -= Time.deltaTime;

                Vector2 barSize = timeBar.sizeDelta;
                barSize.x -= (Time.deltaTime / time) * timeBarWidth;
                timeBar.sizeDelta = barSize;
            }
            
            if (remainingTime <= 0 && !complete)
            {
                onFail();
            }

        }
	}
    public virtual void OnGUI()
    {
        //if(this.isTimed)
        //{
        //    GUI.Box(this.timeBar, Texture2D.whiteTexture);
        //}
    }
	void OnMouseDown(){
		if (om.PickAble(this.transform.position)) {
            if (remainingTime > 0 || !isTimed)
            {
                doAction();
            }
            else Debug.Log("Time's up");
		}
	}

    public abstract void onFail();
	public abstract void doAction ();
}
