using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Coward AI runs when at 50% health -> Doesn't search for player -> Hides in a room or something.
//Aggro AI always searches for and attacks player -> Just pretty retarded overall tbh -> Maybe retreat treshold is varied
//Strategy decides what needs to happen before AI goes into a specific state
public class Base_BossStrategy
{
    public string m_name = "Base";
    public enum STATES
    {
        IDLE,
        ATTACKING,
        SEARCHING,
        RETREAT,

        NUM_STATES
    }
    public STATES m_currentState;

    public virtual void Init(BossData boss)
    {

    }

    public virtual void Update(BossData boss)
    {
        switch(m_currentState)
        {
            case STATES.IDLE: Idle(boss);
                break;
            case STATES.ATTACKING: Attacking(boss);
                break;
            case STATES.SEARCHING: Searching(boss);
                break;
            case STATES.RETREAT: Retreat(boss);
                break;
        }
    }

    public virtual void Idle(BossData boss)
    {

    }

    public virtual void Attacking(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.AGGRESIVE)
            boss.special.TriggerSpecial(boss);

        else if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.DEFENSIVE)
        {
            if(boss.m_health.CalculatePercentageHealth() < 50.0f)
                boss.special.TriggerSpecial(boss);
        }
    }

    public virtual void Searching(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.MOBILITY)
            boss.special.TriggerSpecial(boss);
    }

    public virtual void Retreat(BossData boss)
    {
        if (boss.special.m_trait_type == BOSS_SPECIAL_TYPE.MOBILITY)
            boss.special.TriggerSpecial(boss);
    }
}