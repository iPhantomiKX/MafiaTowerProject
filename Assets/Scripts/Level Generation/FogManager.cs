using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public GameObject fogPrefab;
    public int fogRange = 3; // TO DO - KEEP HERE OR IN PLAYER

    private LevelManager levelManagerRef;
    private PlayerController playerRef;

    GameObject fogLayout;
    GameObject[][] fogMap;

    // Use this for initialization
	void Start () {
        playerRef = FindObjectOfType<PlayerController>();
        levelManagerRef = FindObjectOfType<LevelManager>();

        // Generate fogMap
        int SizeX = levelManagerRef.columns;
        int SizeY = levelManagerRef.rows;

        fogMap = new GameObject[SizeX][];
        fogLayout = new GameObject("FogLayout");

        // Fill up fogMap
        for (int i = 0; i < SizeX; i++)
        {
            fogMap[i] = new GameObject[SizeY];
            for (int j = 0; j < SizeY; j++)
            {
                fogMap[i][j] = Instantiate(fogPrefab, levelManagerRef.GetVec3Pos(i, j), Quaternion.identity, fogLayout.transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        DoFogChecks();
	}

    void DoFogChecks()
    {
        // Get Player GridPos

        //
    }
}
