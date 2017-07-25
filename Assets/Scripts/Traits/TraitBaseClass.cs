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
        i_TraitLevel = Mathf.Clamp(i_TraitLevel, 1, maxLevel);
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

    public string GetName(bool ForUI = false)
    {
        if (!ForUI)
        {
            return displayName + levelStr;
        }
        else
        {
            string romanNum = " ";

            switch (i_TraitLevel)
            {
                case 1: romanNum = " I"; break;
                case 2: romanNum = " II"; break;
                case 3: romanNum = " III"; break;
                case 4: romanNum = " IV"; break;
                case 5: romanNum = " V"; break;
                case 6: romanNum = " VI"; break;
                case 7: romanNum = " VII"; break;
                case 8: romanNum = " VIII"; break;
                case 9: romanNum = " XI"; break;
            }

            return displayName + romanNum;
        }
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

        return 1 + ((i_TraitLevel - 1) * levelMultiplier);
    }

    public int GetLevel()
    {
        return i_TraitLevel;
    }

    public virtual void DoTrait() { }

    public virtual void DoEffect() { }

    public virtual TRAIT_TYPE GetTraitType() { return TRAIT_TYPE.NIL; }
}
