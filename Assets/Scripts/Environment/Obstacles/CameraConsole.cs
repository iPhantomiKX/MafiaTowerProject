using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraConsole : MonoBehaviour {

    public GameObject[] securityCameraList;
    public Text cameraIndexDisplay;
    private int camera_index = 0;

    // Use this for initialization
    void Start() {
        securityCameraList = GameObject.FindGameObjectsWithTag("SecurityCamera");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && Input.GetKeyDown("e"))
            InitPanel();
    }

    void MoveToCamera(int indexInArray)
    {
        if (indexInArray < 0 || indexInArray >= securityCameraList.Length) return;

        camera_index = indexInArray;
        Camera.main.transform.localPosition = securityCameraList[camera_index].transform.localPosition + new Vector3(0, 10.0f, 0);
        cameraIndexDisplay.text = "Camera " + (camera_index + 1) + " / " + securityCameraList.Length;
    }

    public void NextCamera()
    {
        if (camera_index == securityCameraList.Length -1) camera_index = 0;
        else ++camera_index;

        Camera.main.transform.localPosition = securityCameraList[camera_index].transform.localPosition + new Vector3(0, 10.0f, 0);
        cameraIndexDisplay.text = "Camera " +  (camera_index+1) + " / " + securityCameraList.Length;
    }

    public void PrevCamera()
    {
        if (camera_index == 0) camera_index = securityCameraList.Length -1;
        else --camera_index;

        Camera.main.transform.localPosition = securityCameraList[camera_index].transform.localPosition + new Vector3(0, 10.0f, 0);
        cameraIndexDisplay.text = "Camera " + (camera_index + 1) + " / " + securityCameraList.Length;
    }

    public void TurnOffCurrentCamera()
    {
        securityCameraList[camera_index].GetComponent<SecurityCamera>().CameraOff();
    }

    public void TurnOffAllCameras()
    {
        foreach (GameObject sc in securityCameraList)
            sc.GetComponent<SecurityCamera>().CameraOff();
    }

    public void TurnOnAllCameras()
    {
        foreach (GameObject sc in securityCameraList)
            sc.GetComponent<SecurityCamera>().CameraOn();
    }

    public void ClosePanel()
    {
        Camera.main.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition + new Vector3(0, 10.0f, 0);
        gameObject.SetActive(false);
    }

    public void InitPanel()
    {
        
    }
}
