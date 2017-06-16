using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject Enemy;
    public GameObject PlayerPrefab;
    public List<TraitBaseClass> TraitsList; // List of all traits

	public GameObject ObjectiveManager;
    private GameObject PlayerReference;

	// Use this for initialization
	void Start () {

        // Spawn player at the start
        GameObject go = Instantiate(PlayerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

        // Assign traits through persistentdata 
		for (int i = 0; i < TraitsList.Count; ++i) {
		
			foreach (string TraitName in PersistentData.m_Instance.PlayerTraitNames)
			{
				if (TraitName.Contains (TraitsList[i].name)) 
				{
					Debug.Log (TraitName);
					go.GetComponentInChildren<TraitHolder> ().TraitList.Add (TraitsList [i]);
				}
			}
		}

		PlayerReference = go;

        if (Enemy)
		    Enemy.GetComponent<EnemyController> ().player = PlayerReference.GetComponentInChildren<PlayerController>().gameObject;
		
        ObjectiveManager.GetComponent<ObjectiveManager> ().PlayerRef = go;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
