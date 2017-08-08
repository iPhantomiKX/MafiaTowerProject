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

        SwitchOff();
    }
	
	// Update is called once per frame
	void Update () {
        DoFogChecks();
	}

    void DoFogChecks()
    {
        // Get Player GridPos
        Vector2 playerGridPos = levelManagerRef.GetGridPos(playerRef.transform.position);

        // Recursive and switch off the relevant fogtiles
        SwitchOffNearFog(playerGridPos, fogRange);
    }

    public void SwitchOffNearFog(Vector2 startPos, int range)
    {
        List<Vector3> checkedList = new List<Vector3>();

        RecursiveSwitchTileOff(startPos, range, checkedList, true);
    }

    void RecursiveSwitchTileOff(Vector2 checkPos, int tilesLeft, List<Vector3> checkedPos, bool firstLoop = false)
    {
        int x = (int)checkPos.x;
        int y = (int)checkPos.y;

		if (CheckIfInBounds (checkPos)) 
		{
            if (tilesLeft == 0)
                fogMap[x][y].GetComponent<FogTile>().SetFogLevel(FogTile.FOG_LEVEL.BORDER);
            else
                fogMap[x][y].GetComponent<FogTile>().SetFogLevel(FogTile.FOG_LEVEL.SEEN);

            checkedPos.Add (checkPos);
		
	        if (tilesLeft > 0)
	        {
	            Vector2 newPos = checkPos + new Vector2(0, 1);
                if (firstLoop)
                {
                    List<Vector3> newList = new List<Vector3>();
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, newList);
                    }
                }
                else
                {
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, checkedPos);
                    }
                }

	            newPos = checkPos + new Vector2(0, -1);
                if (firstLoop)
                {
                    List<Vector3> newList = new List<Vector3>();
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, newList);
                    }
                }
                else
                {
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, checkedPos);
                    }
                }

	            newPos = checkPos + new Vector2(1, 0);
                if (firstLoop)
                {
                    List<Vector3> newList = new List<Vector3>();
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, newList);
                    }
                }
                else
                {
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, checkedPos);
                    }
                }

	            newPos = checkPos + new Vector2(-1, 0);
                if (firstLoop)
                {
                    List<Vector3> newList = new List<Vector3>();
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, newList);
                    }
                }
                else
                {
                    if (!checkedPos.Contains(newPos) && CheckIfCanSeeThru(checkPos))
                    {
                        RecursiveSwitchTileOff(newPos, tilesLeft - 1, checkedPos);
                    }
                }
        	}
		}
	}

    bool CheckIfInBounds(Vector2 checkPos)
    {        
        int x = (int)checkPos.x;
        int y = (int)checkPos.y;
        
		if (x >= 0 && x < fogMap.Length && y >= 0 && y < fogMap[0].Length)
        {
            return true;
        }

        return false;
    }

	bool CheckIfCanSeeThru(Vector2 checkPos)
	{
		int x = (int)checkPos.x;
		int y = (int)checkPos.y;		

		if (levelManagerRef.GetCanSeeThru(x, y))
		{
			return true;
		}

		return false;
	}

    public void SwitchOn()
    {
        for (int i = 0; i < fogMap.Length; ++i)
        {
            for (int j = 0; j < fogMap[0].Length; ++j)
            {
                fogMap[i][j].GetComponent<FogTile>().SetFogLevel(FogTile.FOG_LEVEL.UNEXPLORED);
            }
        }

        fogLayout.SetActive(true);

        // Affect AI vision
        BaseSM[] allSM = GameObject.FindObjectsOfType<BaseSM>();
        foreach (BaseSM sm in allSM)
        {
            sm.visionRange /= 2;
            sm.angleFOV /= 2;
        }

    }

    public void SwitchOff()
    {
        for (int i = 0; i < fogMap.Length; ++i)
        {
            for (int j = 0; j < fogMap[0].Length; ++j)
            {
                fogMap[i][j].GetComponent<FogTile>().SetFogLevel(FogTile.FOG_LEVEL.UNEXPLORED);
            }
        }

        fogLayout.SetActive(false);
        
        // Affect AI vision
        BaseSM[] allSM = GameObject.FindObjectsOfType<BaseSM>();
        foreach (BaseSM sm in allSM)
        {
            sm.visionRange *= 2;
            sm.angleFOV *= 2;
        }
    }
}

/*
    public List<BaseCharacter> GetCharactersInRange(Vector3 checkPos, int range)
    {
        List<BaseCharacter> returnList = new List<BaseCharacter>();
        List<Vector3> checkedList = new List<Vector3>();

        returnList = RecursiveFindCharacter(checkPos, range, checkedList);

        return returnList;
    }

    List<BaseCharacter> RecursiveFindCharacter(Vector3 checkPos, int tilesLeft, List<Vector3> checkedPos)
    {
        List<BaseCharacter> returnList = new List<BaseCharacter>();

        if (GetCharacterInTile(checkPos) != null)
        {
            returnList.Add(GetCharacterInTile(checkPos));
            checkedPos.Add(checkPos);
        }

        if (tilesLeft > 0)
        {
            Vector3 newPos = checkPos - new Vector3(0, 1, 0);
            List<BaseCharacter> tempList1 = new List<BaseCharacter>();
            if (!checkedPos.Contains(newPos))
            {
                tempList1 = RecursiveFindCharacter(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos - new Vector3(0, -1, 0);
            List<BaseCharacter> tempList2 = new List<BaseCharacter>();
            if (!checkedPos.Contains(newPos))
            {
                tempList2 = RecursiveFindCharacter(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos - new Vector3(1, 0, 0);
            List<BaseCharacter> tempList3 = new List<BaseCharacter>();
            if (!checkedPos.Contains(newPos))
            {
                tempList3 = RecursiveFindCharacter(newPos, tilesLeft - 1, checkedPos);
            }

            newPos = checkPos - new Vector3(-1, 0, 0);
            List<BaseCharacter> tempList4 = new List<BaseCharacter>();
            if (!checkedPos.Contains(newPos))
            {
                tempList4 = RecursiveFindCharacter(newPos, tilesLeft - 1, checkedPos);
            }

            foreach (BaseCharacter aCharacter in tempList1)
            {
                returnList.Add(aCharacter);
            }

            foreach (BaseCharacter aCharacter in tempList2)
            {
                returnList.Add(aCharacter);
            }

            foreach (BaseCharacter aCharacter in tempList3)
            {
                returnList.Add(aCharacter);
            }

            foreach (BaseCharacter aCharacter in tempList4)
            {
                returnList.Add(aCharacter);
            }
        }

        return returnList;
    }
 */
