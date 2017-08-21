using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// A script that is meant to be attached to the children colldier of the vent tiles.
/// 
/// On the first update of the game, it destroys any other collider it touches,
/// creating a pathway for the player to walk on.
/// 
/// The Rigidbody of the vent tile is then set to STATIC so that it no longer moves but 
/// keeps its rigidbody functionality.
/// 
/// The length of the colliders are increased so that there are no gaps inbetween.
/// 
/// After the first Update is called, the script is disabled 
/// This is to allow the collision of the originally Dynamic Rigidbody BoxCollider2Ds to resolve 
/// their collision to create the pathways.
/// 
/// </summary>
public class VentColliderScript : MonoBehaviour {

    public bool HorizontalPiece;
    public float Incremental = 0.05f;

    void Update()
    {
        //Change the Rigidbody to Static so they no longer collide with each other
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //Increase the length of the colliders so that there are no gaps
        if (HorizontalPiece)
            GetComponent<BoxCollider2D>().size += new Vector2(Incremental, 0);
        else
            GetComponent<BoxCollider2D>().size += new Vector2(0, Incremental);

        //The script then no longer updates - Read Summary for more info on why I did this
        Destroy(this);
    }

    //This is turned off as the script is disabled so that Players are not Despawned.
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Vent")
            return;

        Destroy(gameObject);
        Destroy(coll.gameObject);
    }

}
