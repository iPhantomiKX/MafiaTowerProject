using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour {

	public GameObject ObjectiveManager;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(ObjectiveManager.GetComponent<ObjectiveManager>().IsComplete()){
			SceneManager.LoadScene("NextLevelScene");
			PersistentData.m_Instance.CurrentLevel = SceneManager.GetActiveScene ().buildIndex;
		}
	}

}
