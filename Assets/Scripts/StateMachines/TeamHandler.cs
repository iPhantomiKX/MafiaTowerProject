using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHandler : MonoBehaviour
{

    public enum TEAM
    {
        PLAYER,
        NEUTRAL,
        ENEMY,
    }

    public TEAM currentTeam;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckIfCanInteract(TEAM otherTeam)
    {
        if (currentTeam == TEAM.PLAYER)
        {
            return true;
        }

        if (currentTeam == TEAM.NEUTRAL)
        {
            if (otherTeam != TEAM.NEUTRAL)
                return true;
        }

        if (currentTeam == TEAM.ENEMY)
        {
            if (otherTeam != TEAM.ENEMY)
                return true;
        }

        return false;
    }
}
