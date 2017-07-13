using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject Enemy;
    public GameObject PlayerPrefab;
	public GameObject ObjectiveManager;
    
    private GameObject PlayerReference;
    private List<TraitBaseClass> TraitsList;
	
    void Awake()
    {
        // Spawn player at the start
        Debug.Log("test 2");
        GameObject go = Instantiate(PlayerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        PlayerReference = go;
    }

    // Use this for initialization
	void Start () {

        PlayerReference.GetComponentInChildren<TraitHolder>().SetTraits(PersistentData.m_Instance.PlayerTraits);

        if (Enemy)
		    Enemy.GetComponent<EnemyController> ().player = PlayerReference.GetComponentInChildren<PlayerController>().gameObject;

        if (!ObjectiveManager)
            ObjectiveManager = GameObject.FindObjectOfType<ObjectiveManager>().gameObject;

        ObjectiveManager.GetComponent<ObjectiveManager>().PlayerRef = PlayerReference;

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
