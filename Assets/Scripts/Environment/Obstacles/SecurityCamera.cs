using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour {

    public float detectionAngle;
    public float detectionDistance;

    public float rotationAngle;
    public float rotationSpeed;

    public bool isActive = true;

    private GameObject player;

    public float soundInterval = 1.0f;
    private float sound_counter;

    // Use this for initialization
    void Start()
    {
        sound_counter = soundInterval;
        //GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, detectionAngle / 360.0f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if (IsPlayerSeen())
            EmitSound();
        else
            Rotate();
    }

    protected bool IsPlayerSeen()
    {
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;

        //check player in cone and in range
        Vector3 playerDir = player.transform.position - this.transform.position;
        Vector3 forward = this.transform.up;
        float angle = Vector3.Angle(playerDir, forward);
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (angle < detectionAngle && distance < detectionDistance)
        {

            //check if player behind any obstacle
            int layerMask = (1 << 8 | 1 << 11);
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, playerDir, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
            }
            else
            {
                //In case part of the body is seen
                RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(playerDir.z + 10f, Vector3.forward) * playerDir, Mathf.Infinity, layerMask);
                if (hit2.collider != null)
                {
                    if (hit2.collider.gameObject.tag == "Player")
                    {
                        return true;
                    }
                }
                else
                {
                    RaycastHit2D hit3 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(playerDir.z - 10f, Vector3.forward) * playerDir, Mathf.Infinity, layerMask);
                    if (hit3.collider != null)
                    {
                        if (hit3.collider.gameObject.tag == "Player")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        else
            return false;
    }

    void Rotate()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);

        if (transform.rotation.eulerAngles.z > rotationAngle * 0.5 || transform.rotation.eulerAngles.z < (-rotationAngle * 0.5))
            rotationSpeed = -rotationSpeed;
    }

    public void CameraOff()
    {
        isActive = false;
        GetComponent<LineRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void CameraOn()
    {
        isActive = true;
        GetComponent<LineRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    }

    void EmitSound()
    {
        if(sound_counter > soundInterval)
        {
            GetComponent<EmitSound>().emitSound();
            sound_counter = 0.0f;
        }
        sound_counter += Time.deltaTime;
    }
}
