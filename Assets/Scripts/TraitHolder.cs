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

        //if (Input.GetKeyDown(KeyCode.E) && b_InTrigger)
        
        //if (Input.anyKeyDown)
            //SetInputReceived(FetchKey());

        // Debug
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene("LoseTraitScene");
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

    KeyCode FetchKey()
    {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }

    public bool CheckForTrait(TraitBaseClass checkTrait)
    {
        return TraitList.Contains(checkTrait);
    }
}
