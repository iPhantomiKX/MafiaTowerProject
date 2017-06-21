using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public enum GAME_STATE
    {
        RUNNING,
        PAUSED,
    }

    public GAME_STATE CurrentState = GAME_STATE.RUNNING;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
