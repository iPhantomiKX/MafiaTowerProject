using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUtilities : MonoBehaviour {//Needs to be a sinlgeton

    public string NameOfPauseScene = "PauseScene";

    private GraphicRaycaster paused_graphic_raycaster;

	public void Pause()
    { 
        //Makes any GUI in the game scene uninteractable
        paused_graphic_raycaster = GameObject.Find("ObjtUI").GetComponent<GraphicRaycaster>();
        paused_graphic_raycaster.enabled = false;

        //Freeze player control
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().enabled = false;

        //Pause and change scene
        Time.timeScale = 0.0f;
        SceneManager.LoadScene(NameOfPauseScene, LoadSceneMode.Additive);

        //Freeze enemy behaeviours
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enem in enemies)
            enem.GetComponent<MeleeEnemy>().enabled = false;
    }

    public void Resume()
    {
        //Resume gametime and unload pause scene
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync(NameOfPauseScene);

        //Unfreeze player control
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().enabled = true;

        //Unfreeze enemy behaeviours
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enem in enemies)
            enem.GetComponent<MeleeEnemy>().enabled = true;

        //UI in game scene interactable again
        paused_graphic_raycaster = GameObject.Find("ObjtUI").GetComponent<GraphicRaycaster>();
        paused_graphic_raycaster.enabled = true;
    }
}
