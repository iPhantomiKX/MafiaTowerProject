using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

    public ChosenList ChosenTraitList = null;
    public ChosenTrait ChosenTraitPanel = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	    if (ChosenTraitPanel)
        {
            if (ChosenTraitPanel.AttachedText.text == "")
                GetComponent<Button>().interactable = false;
            else
                GetComponent<Button>().interactable = true;
            }

	}

    public void DoRestart()
    {
        if (ChosenTraitList != null)
        {
            // Restarting to start
            // Reset traits that are being removed from player traits
            foreach (TraitBaseClass aTrait in PersistentData.m_Instance.AllTraits)
            {
                if (!PersistentData.m_Instance.PlayerTraits.Contains(aTrait))
                {
                    aTrait.SetLevel(1);
                }
            }

            PersistentData.m_Instance.PlayerTraits.Clear();

            Transform[] TransformList = ChosenTraitList.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform aTransform in TransformList)
            {
                if (aTransform.gameObject.name.Contains("Button"))
                    PersistentData.m_Instance.PlayerTraits.Add(aTransform.gameObject.GetComponent<ButtonElement>().AttachedTrait);
            }

            Debug.Log("...restarted... playertrait size " + PersistentData.m_Instance.PlayerTraits.Count);
            PersistentData.m_Instance.CurrentLevel = 0;
        }
        else if (ChosenTraitPanel != null)
        {
            // Restarting back one level
            ChosenTraitPanel.GetComponent<ChosenTrait>().AttachedTrait.SetLevel(1);
            PersistentData.m_Instance.PlayerTraits.Remove(ChosenTraitPanel.GetComponent<ChosenTrait>().AttachedTrait);

            PersistentData.m_Instance.CurrentLevel = Mathf.Max(0, PersistentData.m_Instance.CurrentLevel - 1);
        }


        SceneManager.LoadScene("GameScene");
    }

}
