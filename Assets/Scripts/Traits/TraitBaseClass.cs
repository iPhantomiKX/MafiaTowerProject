using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TraitBaseClass : MonoBehaviour {

    public enum TRAIT_TYPE
    {
        NIL,
        PASSIVE,    // Traits that add to player's stats
        OBSTACLE,     // Traits that have a specific object to trigger
        ABILITY,    // Traits that can be used anywhere
    }

    [Header("Base Trait Values")]
    public string DisplayName;
    public string DisplayDescription;

    protected PlayerController playerObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public void StartUp()
	{
		playerObject = null;
	}

    public void SetPlayer(GameObject Player)
    {
        playerObject = Player.GetComponent<PlayerController>();
    }

    public virtual void DoTrait() { }

    public virtual void DoEffect() { }

    public virtual TRAIT_TYPE GetTraitType() { return TRAIT_TYPE.NIL; }
}
