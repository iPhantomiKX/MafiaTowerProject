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
    private List<TraitBaseClass> randList = new List<TraitBaseClass>();

	// Use this for initialization
	void Start () {
        TraitList = PersistentData.m_Instance.AllTraits;

        for (int count = 0; count < numRandomTraits; ++count)
        {
            int rand = Random.Range(0, TraitList.Count);

            if (randList.Contains(TraitList[rand]))
            {
                --count;
                continue;
            }

            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            go.GetComponent<ButtonElement>().AttachedTrait = TraitList[rand];

            go.SetActive(true);

            randList.Add(TraitList[rand]);
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
