using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

    public RandomTraitList randomTraitListRef;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoNextLevel()
	{
        if (randomTraitListRef.GetSelectedButton())
        {
            Debug.Log(randomTraitListRef.GetSelectedButton().AttachedTrait.GetName());

            if (!PersistentData.m_Instance.PlayerTraits.Contains(randomTraitListRef.GetSelectedButton().AttachedTrait))
            {
                PersistentData.m_Instance.PlayerTraits.Add(randomTraitListRef.GetSelectedButton().AttachedTrait);
            }
            else
            {
                int idx = PersistentData.m_Instance.PlayerTraits.IndexOf(randomTraitListRef.GetSelectedButton().AttachedTrait);
                PersistentData.m_Instance.PlayerTraits[idx].LevelTrait();
            }
        }

        PersistentData.m_Instance.CurrentLevel++;

		Debug.Log("...entering next level... playertrait size " + PersistentData.m_Instance.PlayerTraits.Count);

		SceneManager.LoadScene("GameScene");
	}
}
