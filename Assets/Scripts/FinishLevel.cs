using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour {

	public GameObject ObjectiveManager;
	// Use this for initialization
	void Start () {

        if (!ObjectiveManager)
            ObjectiveManager = GameObject.FindObjectOfType<ObjectiveManager>().gameObject;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
        if (col.gameObject.name.Contains("Player"))
        {
            FinishLevelInTime[] finishLvObjts = FindObjectsOfType<FinishLevelInTime>();
            if (finishLvObjts.Length > 0)
            foreach (FinishLevelInTime objt in finishLvObjts) objt.complete = true;
            if (ObjectiveManager.GetComponent<ObjectiveManager>().IsComplete())
            {
                SceneManager.LoadScene("NextLevelScene");
                PersistentData.m_Instance.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
            }
        }
	}

}
