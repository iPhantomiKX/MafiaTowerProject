using System;
using UnityEngine;

public class BodyTossScript : Inspect
{
    public PlayerController player;

    void Start()
    {
        player = GameObject.Find("PlayerObject").GetComponent<PlayerController>();
    }

    public override void inspect()
    {
        if(player.dragging)
        {
            GameObject dragged_object = player.draggedObject;
            player.draggedObject.GetComponent<BaseSM>().ToggleBodyDrag();
            Destroy(dragged_object.transform.parent.gameObject);    //This is pretty volatile. Becareful to delete the correct parent and not the entire heriarchy.
        }
    }
}
