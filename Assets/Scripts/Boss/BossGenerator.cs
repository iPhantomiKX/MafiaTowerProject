using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinMaxData
{
    public MinMaxData(string name)
    {
        this.name = name;
    }

    public string name;
    public float min_value;
    public float max_value;
}

// Not too sure id this will work in the exe build or not - Don
// Used to populate the list with various boss variables
// If needed the variables can be populated manually regardless
[ExecuteInEditMode]
public class BossGenerator : MonoBehaviour {

    public List<MinMaxData> BossStatsRanges = new List<MinMaxData>();
    public List<Base_BossStrategy> BossStrategyList = new List<Base_BossStrategy>();
    public List<BossSpecial> BossSpecialList = new List<BossSpecial>();
    public List<BossTrait> BossTraitList = new List<BossTrait>();

    GameObject currentBossObject;

	// Use this for initialization
	void Start () {
        BossStatsRanges.Add(new MinMaxData("Movement Speed"));
        BossStatsRanges.Add(new MinMaxData("Attack Speed"));
        BossStatsRanges.Add(new MinMaxData("Melee Attack Damage"));
        BossStatsRanges.Add(new MinMaxData("Ranged Attack Damage"));
        BossStatsRanges.Add(new MinMaxData("Melee Attack Defense"));
        BossStatsRanges.Add(new MinMaxData("Ranged Attack Defense"));

        BossStrategyList.Add(new Coward_BossStrategy());

        BossSpecialList.Add(new Teleport());
        BossSpecialList.Add(new Enrage());
        BossSpecialList.Add(new InstantKillMelee());
        BossSpecialList.Add(new SummonGuards());

        BossTraitList.Add(new BurstRegeneration());
        BossTraitList.Add(new ConstantRegeneration());
        BossTraitList.Add(new MoveSpeedBurst());
        BossTraitList.Add(new MoveSpeedIncrease());
        BossTraitList.Add(new MoveSpeedDecrease());
        BossTraitList.Add(new MeleeDamageIncrease());
        BossTraitList.Add(new MeleeDamageDecrease());
        BossTraitList.Add(new MeleeDefenseIncrease());
        BossTraitList.Add(new MeleeDefenseDecrease());
        BossTraitList.Add(new RangeDamageIncrease());
        BossTraitList.Add(new RangeDamageDecrease());
        BossTraitList.Add(new RangeDefenseIncrease());
        BossTraitList.Add(new RangeDefenseDecrease());
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void CreateBoss(Vector3 spawnPos)
    {
        // Create object and give it the boss data component
        currentBossObject = new GameObject("BossObject");
        currentBossObject.AddComponent<BossData>();

        // Random values for various stats
        currentBossObject.GetComponent<BossData>().m_moveSpeed = GetRandomValue("Movement Speed");
        currentBossObject.GetComponent<BossData>().m_attackSpeed = GetRandomValue("Attack Speed");
        currentBossObject.GetComponent<BossData>().m_meleeDamage = GetRandomValue("Melee Attack Damage");
        currentBossObject.GetComponent<BossData>().m_rangeDamage = GetRandomValue("Ranged Attack Damage");
        currentBossObject.GetComponent<BossData>().m_meleeDefense = GetRandomValue("Melee Attack Defense");
        currentBossObject.GetComponent<BossData>().m_rangeDefense = GetRandomValue("Ranged Attack Defense");

        // Random strategy
        
        // Random special

        // Random modifiers
    }

    public float GetRandomValue(string name)
    {
        foreach (MinMaxData mmd in BossStatsRanges)
        {
            if (mmd.name == name)
            {
                return Random.Range(mmd.min_value, mmd.max_value);
            }
        }

        return 0;
    }
}
