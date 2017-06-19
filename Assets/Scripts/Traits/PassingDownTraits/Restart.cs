using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

    public ChosenList ChosenTraitList = null;
    public ChosenTrait ChosenTraitPanel = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoRestart()
    {
        if (ChosenTraitList != null)
        {
            PersistentData.m_Instance.PlayerTraitNames.Clear();

            Transform[] TransformList = ChosenTraitList.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform aTransform in TransformList)
            {
                if (aTransform.gameObject.name.Contains("Button"))
                    PersistentData.m_Instance.PlayerTraitNames.Add(aTransform.gameObject.GetComponent<ButtonElement>().AttachedTrait);
            }

            PersistentData.m_Instance.CurrentLevel = 0;
        }
        else if (ChosenTraitPanel != null)
        {
            PersistentData.m_Instance.PlayerTraitNames.Remove(ChosenTraitPanel.GetComponent<ChosenTrait>().AttachedText.text);

            PersistentData.m_Instance.CurrentLevel = Mathf.Max(0, PersistentData.m_Instance.CurrentLevel - 1);
        }


        SceneManager.LoadScene(PersistentData.m_Instance.CurrentLevel);
    }

}
