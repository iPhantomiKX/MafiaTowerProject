using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentInspect : Inspect
{
    [Space]
    //This shit doesn't fucking work what even
    //public LayerMask PlayerLayer;
    //public LayerMask VentLayer;

    private GameObject vents_layout;
    private GameObject player_object;

    private GameObject vent_entrances;

    void Start()
    {
        player_object = GameObject.Find("PlayerObject");
        vents_layout = GameObject.Find("VentsLayout");

        //RANDALL - TODO. Can just leave as "vents" to remove this shit.
        //this fucking sucks la. idk.
        vent_entrances = GameObject.Find("VentsEntranceLayout");
    }

    public override void inspect()
    {
        if(player_object.layer == LayerMask.NameToLayer("Player"))
        {
            player_object.layer = LayerMask.NameToLayer("Vent_Player");
            ChangeActionName("Exit Vents");
            vents_layout.SetActive(true);
        }
        else
        {
            player_object.layer = LayerMask.NameToLayer("Player");
            ChangeActionName("Enter Vents");
            vents_layout.SetActive(false);
        }

        player_object.transform.position = transform.position;
    }

    void ChangeActionName(string name)
    {
        foreach (var comp in vent_entrances.GetComponentsInChildren<VentInspect>())
            comp.actionName = name;
    }
}
