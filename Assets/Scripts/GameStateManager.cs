using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public enum GAME_STATE
    {
        RUNNING,
        PAUSED,
    }

    GAME_STATE CurrentState = GAME_STATE.RUNNING;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetState(GAME_STATE newState)
    {
        Rigidbody2D[] allRb = GameObject.FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in allRb)
        {
            if (newState == GAME_STATE.PAUSED)
            {
                // Sleep all rb
                rb.Sleep();
            }
            else
            {
                // Wake all rb
                if (rb.IsSleeping())
                    rb.WakeUp();
            }
        }
    }

    public GAME_STATE GetState()
    {
        return CurrentState;
    }
}
