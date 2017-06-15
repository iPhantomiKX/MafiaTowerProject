using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
