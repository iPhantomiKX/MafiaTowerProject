using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    public GameObject PlayerPrefab;
    public List<TraitBaseClass> TraitsList; // List of all traits

    private GameObject PlayerReference;

	// Use this for initialization
	void Start () {

        // Spawn player at the start
        GameObject go = Instantiate(PlayerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

        // Assign traits through persistentdata 
        for (int i = 0; i < TraitsList.Count; ++i)
            go.GetComponent<TraitHolder>().TraitList.Add(TraitsList[i]);

        PlayerReference = go;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
