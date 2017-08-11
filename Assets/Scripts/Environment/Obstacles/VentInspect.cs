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
    private GameObject black_image;

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 5;
        player_object = GameObject.Find("PlayerObject");
        vents_layout = GameObject.Find("VentsLayout");
        //RANDALL - TODO. Can just leave as "vents" to remove this shit.
        //this fucking sucks la. idk.
        vent_entrances = GameObject.Find("VentsEntranceLayout");

        //RANDALL - TODO: Find someway to instantiate this only once
        black_image = GameObject.Find("BlackImage");
        if (black_image == null)
            black_image = Instantiate(Resources.Load("BlackImage")) as GameObject;
        black_image.SetActive(false);
    }

    public override void inspect()
    {
        if(player_object.layer == LayerMask.NameToLayer("Player"))
        {
            player_object.GetComponent<PlayerController>().inVent = true;
            player_object.layer = LayerMask.NameToLayer("Vent_Player");
            player_object.GetComponent<SpriteRenderer>().sortingOrder = 5;
            ChangeActionName("Exit Vents");
            vents_layout.SetActive(true);
            black_image.SetActive(true);
        }
        else
        {
            player_object.GetComponent<PlayerController>().inVent = false;
            player_object.layer = LayerMask.NameToLayer("Player");
            player_object.GetComponent<SpriteRenderer>().sortingOrder = 2;
            ChangeActionName("Enter Vents");
            vents_layout.SetActive(false);
            black_image.SetActive(false);
        }

        player_object.transform.position = transform.position;
    }

    void ChangeActionName(string name)
    {
        foreach (var comp in vent_entrances.GetComponentsInChildren<VentInspect>())
        {
            if (player_object.GetComponent<PlayerController>().inVent)
                comp.gameObject.layer = LayerMask.NameToLayer("Vent_Player");
            else
                //Changed from default layer to the new Inspectables layer - Pitchaya
                comp.gameObject.layer = LayerMask.NameToLayer("Inspectables");

            comp.actionName = name;
        }
    }
}
