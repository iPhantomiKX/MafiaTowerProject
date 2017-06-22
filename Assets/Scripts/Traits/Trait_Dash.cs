using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_Dash : TraitBaseClass
{
    [Header("Dash Trait Values")]
    public float DashDistance;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public override bool Check(GameObject checkObject)
    {
        if (checkObject.name == ConditionObject.name)
        {
            return true;
        }
        else
            return false;
    }

    public override void DoEffect()
    {
        Debug.Log("Dash");
        //playerObject.transform.Translate((Quaternion.Euler(0, 0, playerObject.transform.rotation.z) * new Vector2(1, 0)).normalized * Force);

        Vector3 worldMouse = Input.mousePosition;
        worldMouse = Camera.main.ScreenToWorldPoint(worldMouse);

        Vector2 vec2MousePos = new Vector2(worldMouse.x, worldMouse.y);
        Vector2 vec2PlayerPos = new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);

        Vector2 dir = (vec2MousePos - vec2PlayerPos).normalized;

        //playerObject.rb.velocity = (dir * DashDistance );

        playerObject.SetDash(dir, DashDistance);
    }
}
