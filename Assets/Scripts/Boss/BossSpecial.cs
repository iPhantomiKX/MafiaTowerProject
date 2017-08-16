using System.Collections.Generic;
using UnityEngine;

public enum BOSS_SPECIAL_TYPE
{
    PASSIVE,
    AGGRESIVE,
    DEFENSIVE,
    MOBILITY,

    NUM_TRAIT_TYPE
}

//Each boss has only ONE special 
public abstract class BossSpecial
{
    public string m_name;
    public BOSS_SPECIAL_TYPE m_trait_type;
    public abstract void Init(BossData boss);
    public abstract void Update(BossData boss);   //Maybe I don't need this? Leave this here just in case I guess
    public abstract bool TriggerSpecial(BossData boss);
}

//Teleports to a random room
public class Teleport : BossSpecial
{
    List<Vector3>teleport_locations = new List<Vector3>();
    bool cooldown_done;
    float timer = 10.0f;
    float trigger_timer = 10.0f;

    //Aesthetic stuff
    int particle_emission = 50;

    public Teleport()
    {
        m_name = "Teleport";
        m_trait_type = BOSS_SPECIAL_TYPE.MOBILITY;
    }

    public override void Init(BossData boss)
    {

        //boss.gameObject.transform.GetChild(0).gameObject.AddComponent<ParticleSystem>();    //Add a ParticleSystem to the Child Object. The one with the SpriteRenderer.


        //Use LevelManager to get a list of all the rooms and add them to the list
        for (int i = 0; i < boss.m_pathfinderRef.theLevelManager.numberOfMiscRooms; ++i)
        {
            RoomScript temp = boss.m_pathfinderRef.theLevelManager.GetAllMiscRooms()[i];
            float tilespacing = boss.m_pathfinderRef.theLevelManager.tilespacing;
            Vector3 tempVec = new Vector3(tilespacing * Mathf.RoundToInt(temp.xpos + (temp.roomWidth * 0.5f)), tilespacing * Mathf.RoundToInt(temp.ypos + (temp.roomHeight * 0.5f)), 1f);

            teleport_locations.Add(tempVec);
        }
    }

    public override void Update(BossData boss)
    {
        if (cooldown_done)
            return;

        timer += Time.deltaTime;
        if(timer >= trigger_timer)
        {
            timer = 0.0f;
            cooldown_done = true;
        }
    }

    public override bool TriggerSpecial(BossData boss)
    {
        if (!cooldown_done)
            return false;

        boss.transform.GetChild(0).GetComponent<ParticleSystem>().Emit(particle_emission);
        boss.transform.position = teleport_locations[UnityEngine.Random.Range(0, teleport_locations.Count)];

        cooldown_done = false;

        return true;
    }
}

//Enrage increases damage when boss health is below 25%
public class Enrage : BossSpecial
{
    bool triggered = false;

    float health_percentage_treshold = 75f;

    float red_treshold = 0.75f;
    float red_fade_speed = 0.25f;
    SpriteRenderer sr;
    Color original_color;

    public Enrage()
    {
        m_name = "Enrage";
        m_trait_type = BOSS_SPECIAL_TYPE.AGGRESIVE;
    }

    public override void Init(BossData boss)
    {
        sr = boss.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();  //Because Boss Sprite is a child of BossObject
    }

    public override void Update(BossData boss)
    {
        TriggerSpecial(boss);

        if (!triggered)
            return;

        if (sr.color.r <= red_treshold)
            sr.color = new Color(1f, sr.color.g, sr.color.b);
        else
            sr.color = new Color(sr.color.r - red_fade_speed * Time.deltaTime, sr.color.g, sr.color.b);

        if (boss.m_health.CalculatePercentageHealth() > health_percentage_treshold)
            sr.color = original_color;
    }

    public override bool TriggerSpecial(BossData boss)
    {
        if (triggered)
            return false;

        if (boss.m_health.CalculatePercentageHealth() < health_percentage_treshold)
        {
            Debug.Log("ENRAGE TRIGGERED");
            triggered = true;
            boss.m_meleeDamage *= 1.5f;
            original_color = sr.color;
            sr.color = new Color(1f, sr.color.g * 0.5f, sr.color.b * 0.5f);
            return true;
        }

        return false;
    }
}

//Not actually instant kill but Melee damage is buffed like crazy
public class InstantKillMelee : BossSpecial
{
    public InstantKillMelee()
    {
        m_name = "InstantKillMelee";
        m_trait_type = BOSS_SPECIAL_TYPE.PASSIVE;
    }

    public override void Init(BossData boss)
    {
        boss.m_meleeDamage *= 99999.99999f;
    }

    public override void Update(BossData boss)
    {

    }

    public override bool TriggerSpecial(BossData boss)
    {
        return true;
    }
}

//Summons a few Guards 
public class SummonGuards : BossSpecial
{
    int guard_count = 5;

    public SummonGuards()
    {
        m_name = "SummonGuards";
        m_trait_type = BOSS_SPECIAL_TYPE.DEFENSIVE;
    }

    public override void Init(BossData boss)
    {
    }

    public override void Update(BossData boss)
    {
    }

    public override bool TriggerSpecial(BossData boss)
    {
        //Instantiate some guards
        GameObject.Instantiate(Resources.Load("MeleeEnemy 1"));//does not work, get the proper prefab name and remember to init - force change state to alert or something

        return false;
    }
}