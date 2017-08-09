using UnityEngine;

//Active and passive skills
//Active skills only activate ones in awhile
//Passive skills are applied constantly/one time at the start
public abstract class BossTrait
{
    public string m_name { get; protected set; }
    public abstract void Init(BossData boss);
    public abstract void Update(BossData boss);
}

public abstract class Active_BossTrait : BossTrait
{
    protected float timer = 0.0f;
    public float timer_trigger = 5.0f;
}

public abstract class Passive_BossTrait : BossTrait
{
    //Empty as intended. LMAO.
}

/*Regeneration
 *  Percentage/Value (should probably use percentage-based so that it can scale better)
 *  Burst regen - timer based - heals more in a burst
 *  Steady regen - regens every second - heals much less
 */
public class BurstRegeneration : Active_BossTrait
{
    public float regen_amount;

    public BurstRegeneration()
    {
        m_name = "BurstRegen";
        timer_trigger = 5.0f;
    }

    public override void Init(BossData boss)
    {
        regen_amount = boss.m_health.health * 0.1f;    //10% regen every 5 seconds
    }

    public override void Update(BossData boss)
    {
        timer += Time.deltaTime;
        if(timer >= timer_trigger)
        {
            timer = 0.0f;
            boss.m_health.health += (int)regen_amount;
        }
    }
}

public class ConstantRegeneration : Active_BossTrait
{
    float regen_amount;

    public ConstantRegeneration()
    {
        m_name = "PassiveRegen";
    }

    public override void Init(BossData boss)
    {
        regen_amount = boss.m_health.health * 0.01f;    //1% regen every second(NOT FRAME)
    }

    public override void Update(BossData boss)
    {
        //boss.m_health.health += (int)(regen_amount * Time.deltaTime);   //Isn't this going to round down to 0 everytime? Godfuckingdamnit
        boss.m_health.health = (int)((float)boss.m_health.health + (regen_amount * Time.deltaTime));
    }
}

public class MoveSpeedBurst : Active_BossTrait
{
    float move_speed_bonus = 1.2f;

    float original_movespeed;
    bool isTriggered = false;
    float move_speed_timer = 0.0f;
    float move_speed_duration = 3.0f;

    public MoveSpeedBurst()
    {
        m_name = "MoveSpeedBurst";
        timer_trigger = 5.0f;
    }

    public override void Init(BossData boss)
    {

    }

    public override void Update(BossData boss)
    {
        if(isTriggered)
        {
            move_speed_timer += Time.deltaTime;

            if(move_speed_timer >= move_speed_duration)
            {
                move_speed_timer = 0.0f;
                boss.m_moveSpeed = original_movespeed;
                isTriggered = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= timer_trigger)
            {
                timer = 0.0f;
                original_movespeed = boss.m_moveSpeed;
                boss.m_moveSpeed *= move_speed_bonus;    //20% extra movement speed
                isTriggered = true;
            }
        }
    }
}

public class MoveSpeedIncrease : Passive_BossTrait
{
    float move_speed_increase = 1.2f;       //Percentage based  //20% extra movement speed

    public MoveSpeedIncrease()
    { 
        m_name = "MoveSpeedIncrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_moveSpeed *= move_speed_increase;    
    }

    public override void Update(BossData boss){}
}

public class MoveSpeedDecrease : Passive_BossTrait
{
    float move_speed_decrease = 0.8f;       //Percentage based  //20% decreased movement speed

    public MoveSpeedDecrease()
    {
        m_name = "MoveSpeedDecrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_moveSpeed *= move_speed_decrease;    
    }

    public override void Update(BossData boss) { }
}

public class MeleeDamageIncrease : Passive_BossTrait
{
    public float melee_damage_increase = 1.5f; //Percentage increase

    public MeleeDamageIncrease()
    {
        m_name = "MeleeDamageIncrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_meleeDamage *= melee_damage_increase;
    }

    public override void Update(BossData boss){ }

}

public class MeleeDamageDecrease : Passive_BossTrait
{
    public float melee_damage_decrease = 0.8f; //20% decrease //Percentage increase

    public MeleeDamageDecrease()
    {
        m_name = "MeleeDamageDecrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_meleeDamage *= melee_damage_decrease;
    }

    public override void Update(BossData boss) { }

}

public class MeleeDefenseIncrease : Passive_BossTrait
{
    public float melee_defense_increase = 1.5f; //Percentage increase

    public MeleeDefenseIncrease()
    {
        m_name = "MeleeDefenseIncrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_meleeDefense *= melee_defense_increase;
    }

    public override void Update(BossData boss) { }

}

public class MeleeDefenseDecrease : Passive_BossTrait
{
    public float melee_defense_decrease = 0.8f; //20% decrease //Percentage increase

    public MeleeDefenseDecrease()
    {
        m_name = "MeleeDefenseDecrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_meleeDefense *= melee_defense_decrease;
    }

    public override void Update(BossData boss) { }

}

public class RangeDamageIncrease : Passive_BossTrait
{
    public float range_damage_increase = 1.5f; //Percentage increase

    public RangeDamageIncrease()
    {
        m_name = "RangeDamageIncrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_rangeDamage *= range_damage_increase;
    }

    public override void Update(BossData boss) { }

}

public class RangeDamageDecrease : Passive_BossTrait
{
    public float range_damage_decrease = 0.8f; //20% decrease //Percentage increase

    public RangeDamageDecrease ()
    {
        m_name = "RangeDamageDecrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_rangeDamage *= range_damage_decrease;
    }

    public override void Update(BossData boss) { }

}

public class RangeDefenseIncrease : Passive_BossTrait
{
    public float range_defense_increase = 1.5f; //Percentage increase

    public RangeDefenseIncrease()
    {
        m_name = "RangeDefenseIncrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_rangeDefense *= range_defense_increase;
    }

    public override void Update(BossData boss) { }

}

public class RangeDefenseDecrease : Passive_BossTrait
{
    public float range_defense_decrease = 0.8f; //20% decrease //Percentage increase

    public RangeDefenseDecrease()
    {
        m_name = "RangeDefenseDecrease";
    }

    public override void Init(BossData boss)
    {
        boss.m_rangeDefense *= range_defense_decrease;
    }

    public override void Update(BossData boss) { }

}

public class Template : Active_BossTrait
{
    public override void Init(BossData boss)
    {
        
    }

    public override void Update(BossData boss)
    {
        
    }
}
