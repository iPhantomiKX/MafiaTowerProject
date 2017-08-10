using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Objective : MonoBehaviour {

	public ObjectiveManager om;
    public string objtname;
	public bool complete{ get; set;}
	public bool isTimed;
    public float time;
    public float remainingTime;
    //public Rect timeBar;
    [HideInInspector]
    public RectTransform timeBar;
    [HideInInspector]
    public float timeBarWidth;
    public int numCompleted = 0;
    public int numRequired = 1;
    public bool mandatory;
    public Text objtText;
    public bool failed;
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
            
            if (remainingTime <= 0 && !complete && !failed)
            {
                failed = true;
                Debug.Log("Time's up!");
                onFail();
            }

        }
        if (numCompleted == numRequired && !complete)
        {
            complete = true;
            om.OnComplete(gameObject);
        }
	}

    public abstract void onFail();
    public abstract bool check();
}
