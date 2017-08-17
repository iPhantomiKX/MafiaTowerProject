using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraConsole : MonoBehaviour {

    public GameObject[] securityCameraList;
    public Text cameraIndexDisplay;

    private int camera_index = 0;
    private GameObject camera_offline_screen;
    private PanelManager pm;

    // Use this for initialization
    void Awake()
    {
    }

    void Start()
    {
        pm = GameObject.Find("Canvas").GetComponent<PanelManager>();
        securityCameraList = GameObject.FindGameObjectsWithTag("SecurityCamera");
        camera_offline_screen = transform.GetChild(0).gameObject; //A bit hardcoded but uh should be fine as long as the first child is the offline screen
    }

    void MoveToCamera(int indexInArray)
    {
        if (indexInArray < 0 || indexInArray >= securityCameraList.Length) return;

        camera_index = indexInArray;
        Camera.main.transform.position = securityCameraList[camera_index].transform.position + new Vector3(0, 0, -1.0f);
        cameraIndexDisplay.text = "Camera " + (camera_index + 1) + " / " + securityCameraList.Length;

        if (securityCameraList[camera_index].GetComponent<SecurityCamera>().IsOn())
            camera_offline_screen.SetActive(false);
        else
            camera_offline_screen.SetActive(true);
    }

    public void NextCamera()
    {
        if (camera_index == securityCameraList.Length - 1) camera_index = 0;
        else ++camera_index;

        MoveToCamera(camera_index);
    }

    public void PrevCamera()
    {
        if (camera_index == 0) camera_index = securityCameraList.Length -1;
        else --camera_index;

        MoveToCamera(camera_index);
    }

    public void TurnOffCurrentCamera()
    {
        securityCameraList[camera_index].GetComponent<SecurityCamera>().CameraOff();
        camera_offline_screen.SetActive(true);
    }

    public void TurnOnCurrentCamera()
    {
        securityCameraList[camera_index].GetComponent<SecurityCamera>().CameraOn();
        camera_offline_screen.SetActive(securityCameraList[camera_index].GetComponent<SecurityCamera>().IsDestroyed());
    }

    public void TurnOffAllCameras()
    {
        foreach (GameObject sc in securityCameraList)
            sc.GetComponent<SecurityCamera>().CameraOff();

        camera_offline_screen.SetActive(true);
    }

    public void TurnOnAllCameras()
    {
        foreach (GameObject sc in securityCameraList)
            sc.GetComponent<SecurityCamera>().CameraOn();

        camera_offline_screen.SetActive(securityCameraList[camera_index].GetComponent<SecurityCamera>().IsDestroyed());
    }

    public void ClosePanel()
    {
        Camera.main.GetComponent<PlayerCamera>().free = false;
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().enabled = true;

        pm.ActivatePanels(new[] { "PlayerUI", "ObjectiveUI" });

        pm.DeactivatePanel("SecurityConsole");

        //gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        Camera.main.GetComponent<PlayerCamera>().free = true;
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().enabled = false;
        MoveToCamera(camera_index);
    }
}
