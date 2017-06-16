using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

    public GameObject ChosenTraitList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoRestart()
    {
        PersistentData.m_Instance.PlayerTraitNames.Clear();

        Transform[] TransformList = ChosenTraitList.GetComponentsInChildren<Transform>();
        foreach (Transform aTransform in TransformList)
        {
			if (aTransform.gameObject.name.Contains("Button"))
            	PersistentData.m_Instance.PlayerTraitNames.Add(aTransform.gameObject.GetComponent<ButtonElement>().AttachedTrait);
        }

        PersistentData.m_Instance.CurrentLevel = 1;
        SceneManager.LoadScene(PersistentData.m_Instance.CurrentLevel);
    }

}
