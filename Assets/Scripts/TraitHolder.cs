using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TraitHolder : MonoBehaviour {

    List<TraitBaseClass> TraitList= new List<TraitBaseClass>();

	// Use this for initialization
	void Start () {
		
        foreach (TraitBaseClass aTrait in TraitList)
        {
			aTrait.StartUp();
            aTrait.SetPlayer(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (aTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.PASSIVE)
                aTrait.DoTrait();
            else if (aTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.ABILITY)
                (aTrait as AbilityTrait).DoCooldown();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene("LoseTraitScene");

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene("NextLevelScene");

        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (TraitBaseClass tr in TraitList)
            {
                tr.LevelTrait();
            }
        }
	}

    public void OnDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }

    public void SetTraits(List<TraitBaseClass> aTraitList)
    {
        TraitList = aTraitList;
        foreach (TraitBaseClass aTrait in TraitList)
        {
            aTrait.StartUp();
            aTrait.SetPlayer(gameObject);
        }
    }

    public bool CheckForTrait(TraitBaseClass checkTrait)
    {
        return TraitList.Contains(checkTrait);
    }
}
