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
    public string displayName;
    public string displayDescription;
    public float levelMultiplier;
    public int maxLevel = 5;

    protected string levelStr = "_1";
    protected PlayerController playerObject;
    protected int i_TraitLevel = 1;

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

    public void SetLevel(int level)
    {
        Debug.Log(displayName + " Level Set");

        i_TraitLevel = Mathf.Min(level, maxLevel);
        levelStr = "_" + i_TraitLevel.ToString();
    }

    public void LevelTrait()
    {
        Debug.Log(displayName + " Level Up");

        i_TraitLevel = Mathf.Min(i_TraitLevel + 1, maxLevel);
        levelStr = "_" + i_TraitLevel.ToString();
    }

    public string GetName()
    {
        return displayName + levelStr;
    }

    public bool GetIfMaxLevel()
    {
        if (i_TraitLevel == maxLevel)
            return true;
        return false;
    }

    protected float GetLevelMultiplier()
    {
        if (i_TraitLevel == 1)
            return 1;

        return 1 + (i_TraitLevel * levelMultiplier);
    }

    public virtual void DoTrait() { }

    public virtual void DoEffect() { }

    public virtual TRAIT_TYPE GetTraitType() { return TRAIT_TYPE.NIL; }
}
