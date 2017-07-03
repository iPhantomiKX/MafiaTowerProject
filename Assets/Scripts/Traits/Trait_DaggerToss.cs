using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_DaggerToss : AbilityTrait {

    [Header("DaggerToss Trait Values")]
    public GameObject DaggerPrefab;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void DoEffect()
    {
        Vector3 worldMouse = Input.mousePosition;
        worldMouse = Camera.main.ScreenToWorldPoint(worldMouse);

        Vector2 vec2MousePos = new Vector2(worldMouse.x, worldMouse.y);
        Vector2 vec2PlayerPos = new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);

        Vector2 dir = (vec2MousePos - vec2PlayerPos).normalized;

        GameObject go = (GameObject)Instantiate(DaggerPrefab);
        go.transform.position = playerObject.transform.position;

        go.GetComponent<Bullet>().SetDirection(dir);
        go.GetComponent<Bullet>().mod_speed = GetLevelMultiplier();
    }
}
