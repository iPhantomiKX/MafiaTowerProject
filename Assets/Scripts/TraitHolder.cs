using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TraitHolder : MonoBehaviour {

    List<TraitBaseClass> TraitList= new List<TraitBaseClass>();

	bool b_InTrigger = false;

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
            if (aTrait.traitType == TraitBaseClass.TRAIT_TYPE.PASSIVE)
                aTrait.DoTrait();
            else
                aTrait.DoCooldown();
        }

        //if (Input.GetKeyDown(KeyCode.E) && b_InTrigger)
        
        //if (Input.anyKeyDown)
            //SetInputReceived(FetchKey());

        // Debug
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene("LoseTraitScene");
	}

    public void SetCheckObjects(GameObject checkObject)
	{
        foreach (TraitBaseClass aTrait in TraitList)
        {
            aTrait.SetCheckObject(checkObject);
        }
    }

    public void SetInputReceived(KeyCode key)
    {
        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (key == KeyCode.None)
                aTrait.SetInput(false, key);
            else
                aTrait.SetInput(true, key);
        }
    }

	public void SetInTrigger(bool status)
	{
		b_InTrigger = status;
	}

    public void OnDeath()
    {
        //foreach (TraitBaseClass aTrait in TraitList)
        //{
        //    if (!PersistentData.m_Instance.PlayerTraitNames.Contains(aTrait.DisplayName))
        //    {
        //        PersistentData.m_Instance.PlayerTraitNames.Add(aTrait.DisplayName);
        //    }
        //}

        PersistentData.m_Instance.CurrentLevel = 0;
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
}
