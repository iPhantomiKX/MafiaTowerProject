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

public class BossGenerator : MonoBehaviour {

    public LevelManager levelManagerRef;
    public PlayerController playerRef;

    public List<MinMaxData> BossStatsRanges = new List<MinMaxData>();
    public List<Base_BossStrategy> BossStrategyList = new List<Base_BossStrategy>();
    public List<BossSpecial> BossSpecialList = new List<BossSpecial>();
    public List<BossTrait> BossTraitList = new List<BossTrait>();

    public GameObject currentBossObject;
    public bool b_BossSpawned = false;

	// Use this for initialization
	void Start () {
        //BossStatsRanges.Add(new MinMaxData("Movement Speed"));
        //BossStatsRanges.Add(new MinMaxData("Attack Speed"));
        //BossStatsRanges.Add(new MinMaxData("Melee Attack Damage"));
        //BossStatsRanges.Add(new MinMaxData("Ranged Attack Damage"));
        //BossStatsRanges.Add(new MinMaxData("Melee Attack Defense"));
        //BossStatsRanges.Add(new MinMaxData("Ranged Attack Defense"));

        BossStrategyList.Add(new Coward_BossStrategy());
        BossStrategyList.Add(new Aggro_BossStrategy());

        BossSpecialList.Add(new Teleport());
        BossSpecialList.Add(new Enrage());
        //BossSpecialList.Add(new InstantKillMelee());
        //BossSpecialList.Add(new SummonGuards());
        BossSpecialList.Add(new Invulnerability());

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

        if (!levelManagerRef)
            levelManagerRef = FindObjectOfType<LevelManager>();

        if (!playerRef)
            playerRef = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!levelManagerRef)
            levelManagerRef = FindObjectOfType<LevelManager>();

        if (!playerRef)
            playerRef = FindObjectOfType<PlayerController>();

        if (!b_BossSpawned)
            CreateBoss(this.transform.position);

        // Debug
        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateBoss(this.transform.position);
        }
	}

    public void CreateBoss(Vector3 spawnPos)
    {
        b_BossSpawned = true;

        // Create object and give it the boss data component
        GameObject go = Instantiate(currentBossObject);

        go.transform.position = spawnPos;

        // Set rigidbody2d
        go.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;

        // Set player
        go.GetComponentInChildren<BossData>().m_player = playerRef.gameObject;

        // Set pathfinder
        go.GetComponentInChildren<Pathfinder>().theLevelManager = levelManagerRef;

        // Random values for various stats
        go.GetComponentInChildren<BossData>().m_moveSpeed = GetRandomValue("Movement Speed") + (0.2f * (LevelManager.GetCurrentStage() - 1));
        go.GetComponentInChildren<BossData>().m_attackSpeed = GetRandomValue("Attack Speed") + (0.35f * (LevelManager.GetCurrentStage() - 1));
        go.GetComponentInChildren<BossData>().m_meleeDamage = GetRandomValue("Melee Attack Damage") + (LevelManager.GetCurrentStage());
        go.GetComponentInChildren<BossData>().m_rangeDamage = GetRandomValue("Ranged Attack Damage") + (LevelManager.GetCurrentStage());
        go.GetComponentInChildren<BossData>().m_meleeDefense = GetRandomValue("Melee Attack Defense") + (LevelManager.GetCurrentStage() - 1);
        go.GetComponentInChildren<BossData>().m_rangeDefense = GetRandomValue("Ranged Attack Defense") + (LevelManager.GetCurrentStage() - 1);

        // Random strategy -> the sprite used will be based on this strategy
        // Coward -> 'Thin' sprite
        // Aggro -> 'Muscle' sprite
        go.GetComponentInChildren<BossData>().strategy = BossStrategyList[Random.Range(0, BossStrategyList.Count)];
        
        // Random special
        go.GetComponentInChildren<BossData>().special = BossSpecialList[Random.Range(0, BossSpecialList.Count)];

        // Random modifiers
        int randAmount = Random.Range(1, BossTraitList.Count);
        List<BossTrait> tempList = BossTraitList;

        while (randAmount > 0)
        {
            int rand = Random.Range(0, tempList.Count);
            go.GetComponentInChildren<BossData>().modifierList.Add(tempList[rand]);

            tempList.RemoveAt(rand);

            randAmount--;
        }

        // Random attack type
        go.GetComponentInChildren<BossData>().m_currentAttackType = (BossData.ATTACK_TYPE)Random.Range(0, (int)BossData.ATTACK_TYPE.NUM_STATES);
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
