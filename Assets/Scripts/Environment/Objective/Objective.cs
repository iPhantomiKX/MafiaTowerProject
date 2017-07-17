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
    public int numCompleted = 0;
    public int numRequired = 1;
    public bool mandatory;

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
    }

    // Update is called once per frame
    public virtual void Update () {
        GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
        if (this.isTimed && timeBar != null)
        {
            if (GameStateRef.GetState() == GameStateManager.GAME_STATE.RUNNING)
            {
                remainingTime -= Time.deltaTime;

                Vector2 barSize = timeBar.sizeDelta;
                barSize.x = (remainingTime / time) * timeBarWidth;
                timeBar.sizeDelta = barSize;
            }
            
            if (remainingTime <= 0 && !complete)
            {
                onFail();
            }

        }
        if (numCompleted == numRequired)
        {
            complete = true;
            om.OnComplete(gameObject);
        }
	}

    public abstract void onFail();
}
