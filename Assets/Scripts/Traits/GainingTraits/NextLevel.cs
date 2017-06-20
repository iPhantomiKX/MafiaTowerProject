using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoNextLevel()
	{
		int NextLevelIdx = PersistentData.m_Instance.CurrentLevel + 1;
        PersistentData.m_Instance.CurrentLevel = NextLevelIdx;

		SceneManager.LoadScene(NextLevelIdx);
	}
}
