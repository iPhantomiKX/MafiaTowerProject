using System.Collections.Generic;
using UnityEngine;

//Require componenet -> HealthComponenet
//Requite componenet -> MovementScript
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(MovementScript))]
[RequireComponent(typeof(Pathfinder))]
public class BossData : MonoBehaviour {

    public HealthComponent m_health;
    public MovementScript m_movement;

    public float m_moveSpeed = 10.0f;

    public float m_meleeDamage = 10.0f;
    public float m_rangeDamage = 10.0f;
    public float m_attackSpeed = 0.5f;      // How seconds before an attack can occur

    //These act as a multiplier modifer (Higher the float value, more damage taken)
    public float m_meleeDefense = 10.0f;
    public float m_rangeDefense = 10.0f;

    public List<BossTrait> modifierList = new List<BossTrait>();
    public Base_BossStrategy strategy = null;
    public BossSpecial special = null;

    // For pathfinding between rooms etc.
    public Pathfinder m_pathfinderRef;

    // For searching for the player and damaging the player
    public GameObject m_player;
    public float m_visionFOV = 45;
    public float m_visionDistance = 1;

    // Attack type of the boss
    public enum ATTACK_TYPE
    {
        MELEE,
        RANGED,

        NUM_STATES,
    }
    public ATTACK_TYPE m_currentAttackType;
    
    void Awake()
    {
        //Some Example shit
        //strategy = new Coward_BossStrategy();
        //special = new Teleport();

        //modifierList.Add(new MeleeDamageIncrease());
        //modifierList.Add(new RangeDamageIncrease());
        //modifierList.Add(new MeleeDefenseDecrease());
        //modifierList.Add(new RangeDefenseDecrease());
        //modifierList.Add(new ConstantRegeneration());
        //modifierList.Add(new MoveSpeedIncrease());
    }

	// Use this for initialization
	void Start ()
    {
        m_health = GetComponent<HealthComponent>();
        m_movement = GetComponent<MovementScript>();
        m_pathfinderRef = GetComponent<Pathfinder>();

        strategy.Init(this);
        special.Init(this);

        for (int i = 0; i < modifierList.Count; ++i)
            modifierList[i].Init(this);
    }
	
	// Update is called once per frame
	void Update ()
    {
        strategy.Update(this);
        special.Update(this);

        for (int i = 0; i < modifierList.Count; ++i)
            modifierList[i].Update(this);

        if (strategy.isMoving)
            m_movement.Move(strategy.direction, m_moveSpeed);
        
        m_movement.RotateToDirection(strategy.direction);

        // Debug
        if (Input.GetKeyDown(KeyCode.N))
        {
            PrintBossStats();
        }
    }

    public void PrintBossStats()
    {
        Debug.Log("m_moveSpeed: " + m_moveSpeed);
        Debug.Log("m_meleeDamage: " + m_meleeDamage);
        Debug.Log("m_rangeDamage: " + m_rangeDamage);
        Debug.Log("m_meleeDefense: " + m_meleeDefense);
        Debug.Log("m_rangeDefense: " + m_rangeDefense);
        Debug.Log("strategy: " + strategy.m_name);
        Debug.Log("special: " + special.m_name);
        Debug.Log("modifiers:");

        for (int i = 0; i < modifierList.Count; ++i)
            Debug.Log((i+1) + ": " + modifierList[i].m_name);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //If it's a Melee Attack
        {
            //Health -= Damage * m_meleeDefense;
        }

        //If it's a Range Attack
        {
            //Health -= Damage * m_rangeDefense;
        }

        strategy.OnCollide(coll.gameObject, this);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        strategy.OnCollide(coll.gameObject, this);
    }
}
