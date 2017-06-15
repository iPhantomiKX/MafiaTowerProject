using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TraitHolder : MonoBehaviour {

    public List<TraitBaseClass> TraitList;

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
            aTrait.DoTrait();
        }

        if (Input.GetKeyDown(KeyCode.E))
            SetInputReceived();

        if (Input.GetKeyUp(KeyCode.K))
            OnDeath();
	}

    public void SetCheckObjects(GameObject checkObject)
    {
        foreach (TraitBaseClass aTrait in TraitList)
        {
            aTrait.SetCheckObject(checkObject);
        }
    }

    public void SetInputReceived()
    {
        foreach (TraitBaseClass aTrait in TraitList)
        {
            aTrait.SetInput(true);
        }
    }

    public void OnDeath()
    {
        foreach (TraitBaseClass aTrait in TraitList)
        {
            if (!PersistentData.m_Instance.PlayerTraitNames.Contains(aTrait.name))
            {
                PersistentData.m_Instance.PlayerTraitNames.Add(aTrait.name);
            }
        }

        PersistentData.m_Instance.CurrentLevel = 1;
        SceneManager.LoadScene("DeathScene");
    }
}
