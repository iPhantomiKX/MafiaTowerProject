using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject Enemy;
    public GameObject PlayerPrefab;
	public GameObject ObjectiveManager;
    
    private GameObject PlayerReference;
    private List<TraitBaseClass> TraitsList;
	
    // Use this for initialization
	void Start () {

        // Spawn player at the start
        GameObject go = Instantiate(PlayerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

        go.GetComponentInChildren<TraitHolder>().SetTraits(PersistentData.m_Instance.PlayerTraits);

		PlayerReference = go;

        if (Enemy)
		    Enemy.GetComponent<EnemyController> ().player = PlayerReference.GetComponentInChildren<PlayerController>().gameObject;
		
        ObjectiveManager.GetComponent<ObjectiveManager> ().PlayerRef = go;

        Inspect[] Obstacles = GameObject.FindObjectsOfType<Inspect>();
        foreach (Inspect anObject in Obstacles)
        {
            Debug.Log(anObject.name);
            anObject.TraitHolderRef = PlayerReference.GetComponentInChildren<TraitHolder>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
