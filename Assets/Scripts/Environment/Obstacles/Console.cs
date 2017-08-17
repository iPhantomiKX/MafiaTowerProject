using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : TraitObstacle {

    public GameObject AttachedDoor;

    [Header("AI Values")]
    public double TimeToCloseDoor = 3.0;
    public double TimeToOpenDoor = 0.5;

    double d_timer = 0.0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (!AttachedDoor.activeInHierarchy)
            d_timer += Time.deltaTime;

        if (d_timer >= TimeToCloseDoor)
        {
            CloseDoor();

            d_timer = 0.0;
        }
    }

    public void OpenDoor()
    {
        AttachedDoor.SetActive(false);
    }

    public void CloseDoor()
    {
        AttachedDoor.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Enemy") || col.gameObject.name.Contains("Civilian") || col.gameObject.name.Contains("Boss"))
        {
            if (d_timer >= TimeToOpenDoor)
            {
                if (AttachedDoor.activeInHierarchy)
                    OpenDoor();
                else
                    CloseDoor();

                d_timer = -5;
            }
            else
            {
                d_timer += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Enemy") || col.gameObject.name.Contains("Civilian") || col.gameObject.name.Contains("Boss"))
        {
            d_timer = 0.0;
        }
    }
}
