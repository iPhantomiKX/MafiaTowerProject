using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RandomTraitList : MonoBehaviour {

    public Button buttonToTurnOn;
    public GameObject buttonPrefab;
    public int numRandomTraits = 3;

    private List<TraitBaseClass> TraitList;
    private ButtonElement selectedButton;

	// Use this for initialization
    void Start()
    {
        TraitList = PersistentData.m_Instance.AllTraits;

        // Get eligible traits
        List<TraitBaseClass> eligibleList = new List<TraitBaseClass>();
        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (!aTrait.GetIfMaxLevel())
                eligibleList.Add(aTrait);
        }

        if (eligibleList.Count > 0)
        {
            List<TraitBaseClass> randList = new List<TraitBaseClass>();
            for (int count = 0; count < numRandomTraits; ++count)
            {
                int rand = Random.Range(0, eligibleList.Count);

                if (randList.Contains(eligibleList[rand]))
                {
                    if (randList.Count != eligibleList.Count)
                        --count;
                    continue;
                }

                GameObject go = Instantiate(buttonPrefab) as GameObject;
                go.transform.SetParent(transform);
                go.GetComponent<ButtonElement>().AttachedTrait = eligibleList[rand];

                go.SetActive(true);

                randList.Add(eligibleList[rand]);
            }
        }
        else
        {
            buttonToTurnOn.interactable = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetButtonActive()
    {
        buttonToTurnOn.interactable = true;
    }

    public void SetSelectedButton(ButtonElement aButton)
    {
        selectedButton = aButton;
    }

    public ButtonElement GetSelectedButton()
    {
        return selectedButton;
    }
}
