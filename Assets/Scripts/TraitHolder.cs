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

        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (aTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.PASSIVE)
            {
                if (!aTrait.GetComponent<PassiveTrait>().ConstantEffect)
                {
                    aTrait.DoTrait();
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (aTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.PASSIVE)
            {
                if (aTrait.GetComponent<PassiveTrait>().ConstantEffect)
                    aTrait.DoTrait();
            }

            if (aTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.ABILITY)
                (aTrait as AbilityTrait).DoCooldown();
        }

        // CHEAT
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

	IEnumerator DelayDeathScene()
	{
		yield return new WaitForSeconds(0.7f);
		SceneManager.LoadScene("DeathScene");
	}

    public void OnDeath()
    {
		StartCoroutine (DelayDeathScene ());
        //SceneManager.LoadScene("DeathScene");
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

    public bool CheckForTrait(TraitBaseClass checkTrait, int level = 1)
    {
        if (level == 1)
        {
            return TraitList.Contains(checkTrait);
        }
        else
        {
            for (int idx = 0; idx < TraitList.Count; ++idx)
            {
                if (TraitList[idx] == checkTrait)
                {
                    if (TraitList[idx].GetLevel() == level)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
