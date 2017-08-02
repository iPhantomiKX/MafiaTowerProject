using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawningAIData
{
    public enum SPAWN_ROOM_TYPE
    {
        ANY,
        MISC,
        OBJECTIVE,
    }

    public string name;
    public GameObject stateMachine;
    public int amount;
    public SPAWN_ROOM_TYPE spawnRoom;
}

public class LevelManager : MonoBehaviour
{

    public enum TileType
    {
        FLOOR,
        WALL_VERTICAL,
        WALL_HORIZONTAL,
        WALL_TOP_LEFT_CORNER,
        WALL_TOP_RIGHT_CORNER,
        WALL_BOTTOM_LEFT_CORNER,
        WALL_BOTTOM_RIGHT_CORNER,
        WALL_ENDING_LEFT,
        WALL_ENDING_RIGHT,
        WALL_ENDING_TOP,
        WALL_ENDING_BOTTOM,
        VENT,
        VENT_E, // VENT ENTRANCE/EXIT
        ROOM,
        DOOR,
        HACKABLE_DOOR,
        OBJECTIVE_ROOM,
        OBJECTIVE,
        OBSTACLE,
        TRAPS,
        ENTITY,
    };

    [Header("Level Dimension")]
    public int columns = 100;
    public int rows = 100;
    public float tilespacing = 0.275f;

    [Space]
    [Header("Room Modifications")]
    public int numberOfObjectiveRooms = 0;
    public int numberOfMiscRooms = 0;

    [Space]
    [Header("Number Of Collectibles")]
    public int numberOfCollectibles = 0;

    [Space]
    [Header("Room Dimensions")]
    public IntRange roomWidth = new IntRange(5, 10);
    public IntRange roomHeight = new IntRange(5, 10);

    [Space]
    [Header("Room Conditions")]
    public bool hackableDoorLevel = false;
    public bool RandomAmmoCollecitbles = false;
    public bool RandomHealthpackCollecitbles = false;
    public bool BossLevel = false;        //For Every 5 Levels in The Game, this bool becomes true

    //Able to on and off obstacles for the particular level, used for level progression
    [Space]
    [Header("Level Obstacles")]
    public bool GlassObstacle = false;
    public bool BlinkingTrapObstacle = false;
    public bool WaitTrapObstacle = false;
    public bool LaserAlarmObstacle = false;

    [Space]
    [Header("Quantity of Obstacles/Room")]
    public int numberOfObstaclesPerRoom = 1;

    [Space]
    [Header("Quantity of Obstacles For Each Obstacle")]
    public int numberOfWaitTraps = 0;
    public int numberOfLaserAlarms = 0;

    [Space]
    [Header("Tile Types")]
    public GameObject floorTile;
    public GameObject[] wallTile;
    public GameObject roomTile;
    public GameObject objectiveRoomTile;
    public GameObject[] doorTile;
    public GameObject[] ventTile;

    [Space]
    [Header("Player Spawn Platform")]
    public GameObject PlayerSpawnerPlatform;

    [Space]
    [Header("Exit Platform")]
    public GameObject NextLevelPlatform;

    [Space]
    [Header("Level Objectives")]
    public List<GameObject> Objectives;

    [Space]
    [Header("Enemy Object")]
    public List<SpawningAIData> SpawnList;

    [Space]
    [Header("Security Camera Object")]
    public GameObject SecurityCameraObject;

    [Space]
    [Header("Collectible Objects")]
    public List<GameObject> Obstacles;

    [Space]
    [Header("Collectible Objects")]
    public List<GameObject> Collectibles;

    private TileType[][] maptiles;
    private TileType[][] obstacletiles;
    private TileType[][] venttiles;

    private List<RoomScript> existingRooms;
    private RoomScript spawnRoom;
    private RoomScript exitRoom;
    private RoomScript[] objectiveRooms;
    private RoomScript[] miscRooms;

    private GameObject LevelLayout;
    private GameObject VentsLayout;
    private GameObject FloorsLayout;
    private GameObject WallsLayout;
    private GameObject VentsEntranceLayout;
    private GameObject ObjectivesRoomTileLayout;
    private GameObject DoorsTileLayout;
    private GameObject ObstacleLayout;

    private bool areaIsIntersecting;

    private short counter = 0;

    // Use this for initialization
    void Awake()
    {
        //Added by Randall 
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Vent"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Vent"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Vent"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("VIP"), LayerMask.NameToLayer("Vent"));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Vent_Player"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("VIP"), LayerMask.NameToLayer("Vent_Player"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Vent_Player"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Inspectables"), LayerMask.NameToLayer("Vent_Player"));
        //

        LevelLayout = new GameObject("LevelLayout");
        VentsLayout = new GameObject("VentsLayout");
        FloorsLayout = new GameObject("FloorsLayout");
        WallsLayout = new GameObject("WallsLayout");
        VentsEntranceLayout = new GameObject("VentsEntranceLayout");
        ObjectivesRoomTileLayout = new GameObject("ObjectivesRoomTileLayout");
        ObstacleLayout = new GameObject("ObstacleLayout");
        
        DoorsTileLayout = new GameObject("DoorsTileLayout");

        SetupTilesArray();

        CreateRooms();

        SetTilesValueForRooms();
        CreateCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();

        InstantiatePlayerPosition();
        InstantiateNextLevelPlatformPosition();
        InstantiateObjective();
        InstantiateSecurityObject();
        InstantiateCollectibles();
        if (GlassObstacle || BlinkingTrapObstacle || WaitTrapObstacle || LaserAlarmObstacle)
            InstantiateObstacle();
        InstantiateEnemyPosition();

        Debug.Log("Level Spawned");
    }

    void Update()
    {
        //HOTFIX Added by Randall - THIS IS QUITE BAD BUT UH IDK 
        if(counter++ == 1)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Vent"), LayerMask.NameToLayer("Vent"));
            VentsLayout.SetActive(false);
            //gameObject.SetActive(false);
        }
    }

    void SetupTilesArray()
    {
        maptiles = new TileType[columns][];
        venttiles = new TileType[columns][];
        obstacletiles = new TileType[columns][];
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i] = new TileType[rows];
            venttiles[i] = new TileType[rows];
            obstacletiles[i] = new TileType[rows];
        }
    }

    void CreateRooms()
    {
        existingRooms = new List<RoomScript>();
        spawnRoom = new RoomScript();
        exitRoom = new RoomScript();
        objectiveRooms = new RoomScript[numberOfObjectiveRooms];
        miscRooms = new RoomScript[numberOfMiscRooms];

        RecursiveFindEmptyPos(spawnRoom, existingRooms, RoomScript.RoomType.SPAWN);
        existingRooms.Add(spawnRoom);

        for (int i = 0; i < numberOfMiscRooms; i++)
        {
            miscRooms[i] = new RoomScript();
            RecursiveFindEmptyPos(miscRooms[i], existingRooms, RoomScript.RoomType.MISC);
            if (hackableDoorLevel)
            {
                miscRooms[i].doorType = Random.Range(0, 2);
            }
            existingRooms.Add(miscRooms[i]);
        }

        for (int i = 0; i < numberOfObjectiveRooms; i++)
        {
            objectiveRooms[i] = new RoomScript();
            RecursiveFindEmptyPos(objectiveRooms[i], existingRooms, RoomScript.RoomType.OBJECTIVES);
            if(hackableDoorLevel)
            {
                objectiveRooms[i].doorType = Random.Range(0, 2);
            }
            existingRooms.Add(objectiveRooms[i]);
        }

        RecursiveFindEmptyPos(exitRoom, existingRooms, RoomScript.RoomType.EXIT);
        if(hackableDoorLevel)
        {
            exitRoom.doorType = Random.Range(0, 2);
        }
        existingRooms.Add(exitRoom);
    }

    void CreateCorridors()
    {
        for (int i = 0; i < existingRooms.Count - 1; i++)
        {
            SetTilesForVent(existingRooms[i], existingRooms[i + 1]);
        }
    }

    void RecursiveFindEmptyPos(RoomScript room1, List<RoomScript> rooms, RoomScript.RoomType type)
    {
        //Init with type
        IntRange doorDir = new IntRange((int)RoomScript.DoorDirection.NORTH, (int)RoomScript.DoorDirection.MAX_DIRECTIONS);

        room1.Init(roomWidth, roomHeight, columns, rows, type, (RoomScript.DoorDirection)doorDir.Random);
        if (rooms != null)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                areaIsIntersecting = room1.RoomIntersecting(rooms[i]);
                if (areaIsIntersecting)
                {
                    RecursiveFindEmptyPos(room1, rooms, type);
                }
            }
        }
        else
        {
            return;
        }

        if (!areaIsIntersecting)
        {
            return;
        }
    }

    void SetTilesValueForRooms()
    {
        //Debug.Log(tiles.Length);
        for (int i = 0; i < existingRooms.Count; i++)
        {
            RoomScript currentRoom = existingRooms[i];
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xpos + j;

                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.ypos + k;
                    switch (currentRoom.roomType)
                    {
                        case RoomScript.RoomType.SPAWN:
                            {
                                maptiles[xCoord][yCoord] = TileType.ROOM;
                            }
                            break;
                        case RoomScript.RoomType.EXIT:
                            {
                                maptiles[xCoord][yCoord] = TileType.ROOM;
                            }
                            break;
                        case RoomScript.RoomType.OBJECTIVES:
                            {
                                maptiles[xCoord][yCoord] = TileType.OBJECTIVE_ROOM;
                            }
                            break;
                        case RoomScript.RoomType.MISC:
                            {
                                maptiles[xCoord][yCoord] = TileType.FLOOR;
                            }
                            break;
                    }

                    for (int idx = 0; idx < wallTile.Length; idx++ )
                    {
                        //left wall
                        maptiles[currentRoom.xpos][yCoord] = TileType.WALL_VERTICAL;
                        //right wall
                        maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][yCoord] = TileType.WALL_VERTICAL;
                        //bottom wall
                        maptiles[xCoord][currentRoom.ypos] = TileType.WALL_HORIZONTAL;
                        //top wall
                        maptiles[xCoord][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_HORIZONTAL;
                        //Top Left Corner
                        maptiles[currentRoom.xpos][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_TOP_LEFT_CORNER;
                        //Top Right Corner
                        maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_TOP_RIGHT_CORNER;
                        //Bottom Left Corner
                        maptiles[currentRoom.xpos][currentRoom.ypos] = TileType.WALL_BOTTOM_LEFT_CORNER;
                        //Bottom Right Corner
                        maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][currentRoom.ypos] = TileType.WALL_BOTTOM_RIGHT_CORNER;
                    }
                }
            }
            IntRange randdoorXPos = new IntRange(currentRoom.xpos + 2, currentRoom.xpos + currentRoom.roomWidth - 2);
            IntRange randdoorYPos = new IntRange(currentRoom.ypos + 2, currentRoom.ypos + currentRoom.roomHeight - 2);

            int doorXpos = randdoorXPos.Random;
            int doorYPos = randdoorYPos.Random;

            switch (currentRoom.doorDirection)
            {
                case RoomScript.DoorDirection.NORTH:
                    {
                        if (currentRoom.doorType == 0)
                        {
                            maptiles[doorXpos][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.DOOR;
                            maptiles[doorXpos + 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_ENDING_LEFT;
                            maptiles[doorXpos - 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_ENDING_RIGHT;
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[doorXpos][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.HACKABLE_DOOR;
                            maptiles[doorXpos + 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_ENDING_LEFT;
                            maptiles[doorXpos - 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL_ENDING_RIGHT;
                            Vector3 doorposition = new Vector3(tilespacing * (doorXpos), tilespacing * (currentRoom.ypos + currentRoom.roomHeight - 1), 0f);
                            GameObject hackDoor = Instantiate(doorTile[currentRoom.doorType], doorposition, Quaternion.Euler(0, 0, 90));
                            hackDoor.transform.parent = LevelLayout.transform;
                        }
                    }
                    break;
                case RoomScript.DoorDirection.SOUTH:
                    {
                        if (currentRoom.doorType == 0)
                        {
                            maptiles[doorXpos][currentRoom.ypos] = TileType.DOOR;
                            maptiles[doorXpos + 1][currentRoom.ypos] = TileType.WALL_ENDING_LEFT;
                            maptiles[doorXpos - 1][currentRoom.ypos] = TileType.WALL_ENDING_RIGHT;
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[doorXpos][currentRoom.ypos] = TileType.HACKABLE_DOOR;
                            maptiles[doorXpos + 1][currentRoom.ypos] = TileType.WALL_ENDING_LEFT;
                            maptiles[doorXpos - 1][currentRoom.ypos] = TileType.WALL_ENDING_RIGHT;
                            Vector3 doorposition = new Vector3(tilespacing * (doorXpos), tilespacing * currentRoom.ypos, 0f);
                            GameObject hackDoor = Instantiate(doorTile[currentRoom.doorType], doorposition, Quaternion.Euler(0, 0, 270));
                            hackDoor.transform.parent = LevelLayout.transform;
                        }
                    }
                    break;
                case RoomScript.DoorDirection.WEST:
                    {
                        //Door on Left Side
                        if (currentRoom.doorType == 0)
                        {
                            maptiles[currentRoom.xpos][doorYPos] = TileType.DOOR;
                            maptiles[currentRoom.xpos][doorYPos + 1] = TileType.WALL_ENDING_BOTTOM;
                            maptiles[currentRoom.xpos][doorYPos - 1] = TileType.WALL_ENDING_TOP;
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[currentRoom.xpos][doorYPos] = TileType.HACKABLE_DOOR;
                            maptiles[currentRoom.xpos][doorYPos + 1] = TileType.WALL_ENDING_BOTTOM;
                            maptiles[currentRoom.xpos][doorYPos - 1] = TileType.WALL_ENDING_TOP;
                            Vector3 doorposition = new Vector3(tilespacing * (currentRoom.xpos), tilespacing * doorYPos, 0f);
                            GameObject hackDoor = Instantiate(doorTile[currentRoom.doorType], doorposition, Quaternion.Euler(0, 0, 180));
                            hackDoor.transform.parent = LevelLayout.transform;
                        }
                    }
                    break;
                case RoomScript.DoorDirection.EAST:
                    {
                        //Door on Right Side
                        if (currentRoom.doorType == 0)
                        {
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos] = TileType.DOOR;
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos + 1] = TileType.WALL_ENDING_BOTTOM;
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos - 1] = TileType.WALL_ENDING_TOP;
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos] = TileType.HACKABLE_DOOR;
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos + 1] = TileType.WALL_ENDING_BOTTOM;
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos - 1] = TileType.WALL_ENDING_TOP;
                            Vector3 doorposition = new Vector3(tilespacing * (currentRoom.xpos + currentRoom.roomWidth - 1), tilespacing * doorYPos, 0f);
                            GameObject hackDoor = Instantiate(doorTile[currentRoom.doorType], doorposition, Quaternion.Euler(0, 0, 0));
                            hackDoor.transform.parent = LevelLayout.transform;
                        }
                    }
                    break;
            }
        }
    }

    void SetTilesForVent(RoomScript room1, RoomScript room2)
    {
        int room1CenterXVector = Mathf.RoundToInt(room1.xpos + (room1.roomWidth) / 2);
        int room1CenterYVector = Mathf.RoundToInt(room1.ypos + (room1.roomHeight) / 2);

        int room2CenterXVector = Mathf.RoundToInt(room2.xpos + (room2.roomWidth) / 2);
        int room2CenterYVector = Mathf.RoundToInt(room2.ypos + (room2.roomHeight) / 2);

        Vector2 firstVent_E = new Vector2();
        Vector2 secondVent_E = new Vector2();

        ////Check if Otherroom is on left side of room1
        //if (room2CenterXVector <= room1.xpos)
        if (room1CenterXVector >= room2CenterXVector)
        {
            venttiles[room1.xpos + 1][room1CenterYVector] = TileType.VENT_E;
            firstVent_E = new Vector2(room1.xpos + 1, room1CenterYVector);
        }
        //Check if Otherroom is on right side of room1
        else //if (room2CenterXVector >= room1.xpos + room1.roomWidth - 2)
        if (room1CenterXVector <= room2CenterXVector)
        {
            venttiles[(room1.xpos + room1.roomWidth - 2)][room1CenterYVector] = TileType.VENT_E;
            firstVent_E = new Vector2((room1.xpos + room1.roomWidth - 2), room1CenterYVector);
        }

        //Check if Otherroom is on the bottom side of room1
        //if (room2CenterYVector <= room1.ypos + 1)
        if (room1CenterYVector >= room2CenterYVector)        
        {
            venttiles[room2CenterXVector][(room2.ypos + room2.roomHeight - 2)] = TileType.VENT_E;
            secondVent_E = new Vector2(room2CenterXVector, (room2.ypos + room2.roomHeight - 2));
        }
        //Check if Otherroom is on the top side of room1
        else //if (room2CenterYVector >= room1.ypos + room1.roomHeight - 1)
        if (room1CenterYVector <= room2CenterYVector)        
        {
            venttiles[room2CenterXVector][(room2.ypos + 2)] = TileType.VENT_E;
            secondVent_E = new Vector2(room2CenterXVector, (room2.ypos + 2));
        }

        SetTilesForBetweenVents(firstVent_E, secondVent_E);
    }

	void SetTilesForBetweenVents(Vector2 firstPos, Vector2 secondPos)
	{
		// Horizontal first, then vertical
        Vector2 check = firstPos;

		while (check != secondPos)
		{
		    if (check.x != secondPos.x)
            {
                // Horizontal translate
                if (venttiles[(int)check.x][(int)check.y] != TileType.VENT_E)
                    venttiles[(int)check.x][(int)check.y] = TileType.VENT;

                if (firstPos.x <= secondPos.x)
                    check.x++;
                else
                    check.x--;
            }
            else if (check.x == secondPos.x)
            {
                // Vertical translate
                if (venttiles[(int)check.x][(int)check.y] != TileType.VENT_E)
                    venttiles[(int)check.x][(int)check.y] = TileType.VENT;


                if (firstPos.y <= secondPos.y)
                    check.y++;
                else
                    check.y--;
            }
		}
	}

    void InstantiatePlayerPosition()
    {
        int PlayerXPos = Mathf.RoundToInt(spawnRoom.xpos + (spawnRoom.roomWidth / 2));
        int PlayerYPos = Mathf.RoundToInt(spawnRoom.ypos + (spawnRoom.roomHeight / 2));

        Vector3 playerPos = new Vector3(tilespacing * PlayerXPos, tilespacing * PlayerYPos, -1f);
        Instantiate(PlayerSpawnerPlatform, playerPos, Quaternion.identity);
    }

    void InstantiateEnemyPosition()
    {
        int spawnIdx = 0;

        while (SpawnList[spawnIdx].amount == 0)
        {
            ++spawnIdx;
        }

        while (SpawnList[spawnIdx].amount > 0)
        {
            // Find room to spawn
            int randomRoom = Random.Range(0, existingRooms.Count);

            // Get what rooms to ignore
            List<RoomScript.RoomType> typesToIgnore = new List<RoomScript.RoomType>();
            switch (SpawnList[spawnIdx].spawnRoom)
            {
                case SpawningAIData.SPAWN_ROOM_TYPE.ANY:
                    typesToIgnore.Add(RoomScript.RoomType.SPAWN);
                    typesToIgnore.Add(RoomScript.RoomType.EXIT);
                    break;
                case SpawningAIData.SPAWN_ROOM_TYPE.MISC:
                    typesToIgnore.Add(RoomScript.RoomType.SPAWN);
                    typesToIgnore.Add(RoomScript.RoomType.EXIT);
                    typesToIgnore.Add(RoomScript.RoomType.OBJECTIVES);
                    break;
                case SpawningAIData.SPAWN_ROOM_TYPE.OBJECTIVE:
                    typesToIgnore.Add(RoomScript.RoomType.SPAWN);
                    typesToIgnore.Add(RoomScript.RoomType.EXIT);
                    typesToIgnore.Add(RoomScript.RoomType.MISC);
                    break;
            }

            while (typesToIgnore.Contains(existingRooms[randomRoom].roomType))
            {
                randomRoom = Random.Range(0, existingRooms.Count);
            }


            Vector3 spawnPos = new Vector3(tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].xpos + (existingRooms[randomRoom].roomWidth / 2)), tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].ypos + (existingRooms[randomRoom].roomHeight / 2)), 1f);
            GameObject spawnedSM = Instantiate(SpawnList[spawnIdx].stateMachine, spawnPos, Quaternion.identity);

            spawnedSM.GetComponentInChildren<Pathfinder>().theLevelManager = this;

            // Reduce amount, increase index if amount == 0
            SpawnList[spawnIdx].amount--;
            if (SpawnList[spawnIdx].amount <= 0)
            {
                spawnIdx++;

                // Return when spawn index is at end
                if (spawnIdx >= SpawnList.Count)
                {
                    return;
                }
            }
        }
    }

    void InstantiateSecurityObject()
    {
        foreach(var oR in objectiveRooms)
        {
            Vector3 SCPosition = new Vector3(tilespacing * (oR.xpos + 2), tilespacing * (oR.ypos + 2), 0f);
            GameObject SCObject = Instantiate(SecurityCameraObject, SCPosition, Quaternion.identity) as GameObject;
        }
    }

    void InstantiateCollectibles()
    {
        for (int i = 0; i < numberOfCollectibles; i++)
        {
            int randomMiscRoomNumber = Random.Range(0, miscRooms.Length);
            float RandomXPos = tilespacing * Random.Range(miscRooms[randomMiscRoomNumber].xpos + 1, miscRooms[randomMiscRoomNumber].xpos + miscRooms[randomMiscRoomNumber].roomWidth - 1);
            float RandomYPos = tilespacing * Random.Range(miscRooms[randomMiscRoomNumber].ypos + 1, miscRooms[randomMiscRoomNumber].ypos + miscRooms[randomMiscRoomNumber].roomHeight - 1);
            Vector3 RandomPosInRoom = new Vector3(RandomXPos, RandomYPos, 0f);

            //IF BOTH BOOLEANS ARE TRUE
            if (RandomHealthpackCollecitbles && RandomAmmoCollecitbles)
            {
                GameObject Collectible = Instantiate(Collectibles[Random.Range(0, Collectibles.Count)], RandomPosInRoom, Quaternion.identity) as GameObject;
            }

            //ELSE
            else
            {
                for (int idx = 0; idx < Collectibles.Count; idx++)
                {
                    switch (Collectibles[idx].name)
                    {
                        case "AmmoInspect":
                            {
                                if (RandomAmmoCollecitbles)
                                {
                                    GameObject AmmoCollectible = Instantiate(Collectibles[idx], RandomPosInRoom, Quaternion.identity) as GameObject;
                                }
                            }
                            break;
                        case "Medkit":
                            {
                                if (RandomHealthpackCollecitbles)
                                {
                                    GameObject HealthpackCollectible = Instantiate(Collectibles[idx], RandomPosInRoom, Quaternion.identity) as GameObject;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    void InstantiateNextLevelPlatformPosition()
    {
        int NextLevelPlatformXPos = Mathf.RoundToInt(exitRoom.xpos + (exitRoom.roomWidth / 2));
        int NextLevelPlatformYPos = Mathf.RoundToInt(exitRoom.ypos + (exitRoom.roomHeight / 2));

        Vector3 NextLevelPlatformPos = new Vector3(tilespacing * NextLevelPlatformXPos, tilespacing * NextLevelPlatformYPos, -1f);
        Instantiate(NextLevelPlatform, NextLevelPlatformPos, Quaternion.identity);
    }

    void InstantiateObjective()
    {
        for (int i = 0; i < objectiveRooms.Length; i++)
        {
            int ObjectiveXPos = Mathf.RoundToInt(objectiveRooms[i].xpos + (objectiveRooms[i].roomWidth / 2));
            int ObjectiveYPos = Mathf.RoundToInt(objectiveRooms[i].ypos + (objectiveRooms[i].roomHeight / 2));

            Vector3 ObjectivePos = new Vector3(tilespacing * ObjectiveXPos, tilespacing * ObjectiveYPos, -1f);
            GameObject go = Instantiate(Objectives[Random.Range(0, Collectibles.Count)], ObjectivePos, Quaternion.identity) as GameObject;

            if (go.tag == "Rescue")
            {
                go.GetComponent<Pathfinder>().theLevelManager = this;
            }
            else
                maptiles[ObjectiveXPos][ObjectiveYPos] = TileType.OBJECTIVE;
        }
    }

    void InstantiateObstacle()
    {
        for (int i = 0; i < objectiveRooms.Length; i++)
        {
            //int RandomObstacle = Random.Range(0, Obstacles.Count);
            int RandomObstacle = Random.Range(0, 3);
            switch (RandomObstacle)
            {
                case 0://GLASS OBSTACLE
                    {
                        for (int idx = 0; idx < Obstacles.Count; idx++)
                        {
                            if (Obstacles[idx].name == "Glass" && GlassObstacle == true)
                            {
                                int x = 0;
                                int y = 0;

                                int minX = Mathf.RoundToInt(objectiveRooms[i].xpos + (objectiveRooms[i].roomWidth / 2)) - 1;
                                int maxX = Mathf.RoundToInt(objectiveRooms[i].xpos + (objectiveRooms[i].roomWidth / 2)) + 1;

                                int minY = Mathf.RoundToInt(objectiveRooms[i].ypos + (objectiveRooms[i].roomHeight / 2)) - 1;
                                int maxY = Mathf.RoundToInt(objectiveRooms[i].ypos + (objectiveRooms[i].roomHeight / 2)) + 1;

                                while (minX + x <= maxX)
                                {
                                    obstacletiles[minX + x][minY] = TileType.OBSTACLE;
                                    obstacletiles[minX + x][maxY] = TileType.OBSTACLE;

                                    GameObject GlassObjectBottom = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX + x), tilespacing * (minY), 0), Quaternion.identity);
                                    GameObject GlassObjectTop = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX + x), tilespacing * (maxY), 0), Quaternion.identity);

                                    GlassObjectBottom.transform.parent = ObstacleLayout.transform;
                                    GlassObjectTop.transform.parent = ObstacleLayout.transform;

                                    x++;
                                }

                                while (minY + y <= maxY)
                                {
                                    obstacletiles[minX][minY + y] = TileType.OBSTACLE;
                                    obstacletiles[maxX][minY + y] = TileType.OBSTACLE;

                                    GameObject GlassObjectLeft = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX), tilespacing * (minY + y), 0), Quaternion.identity);
                                    GameObject GlassObjectRight = Instantiate(Obstacles[idx], new Vector3(tilespacing * (maxX), tilespacing * (minY + y), 0), Quaternion.identity);

                                    GlassObjectLeft.transform.parent = ObstacleLayout.transform;
                                    GlassObjectRight.transform.parent = ObstacleLayout.transform;

                                    y++;
                                }
                            }
                        }
                    }
                    break;
                case 1://WAIT TRAP OBSTACLE
                    {
                        for (int idx = 0; idx < Obstacles.Count; idx++)
                        {
                            if (Obstacles[idx].name == "WaitTrap" && WaitTrapObstacle == true)
                            {
                                for (int num = 0; num < numberOfWaitTraps; num++)
                                {
                                    int RandomXPos = Random.Range(objectiveRooms[i].xpos + 1, objectiveRooms[i].xpos + objectiveRooms[i].roomWidth - 2);
                                    int RandomYPos = Random.Range(objectiveRooms[i].ypos + 1, objectiveRooms[i].ypos + objectiveRooms[i].roomHeight - 2);

                                    obstacletiles[RandomXPos][RandomYPos] = TileType.TRAPS;
                                    Vector3 TrapPos = new Vector3(tilespacing * RandomXPos, tilespacing * RandomYPos, 0);
                                    GameObject WaitTrap = Instantiate(Obstacles[idx], TrapPos, Quaternion.identity);
                                    WaitTrap.transform.parent = ObstacleLayout.transform;
                                }
                            }
                        }
                    }
                    break;
                case 2://BLINKING TRAP OBSTACLE
                    {
                        for (int idx = 0; idx < Obstacles.Count; idx++)
                        {
                            if (Obstacles[idx].name == "BlinkingTrap" && BlinkingTrapObstacle == true)
                            {
                                switch (objectiveRooms[i].doorDirection)
                                {
                                    case RoomScript.DoorDirection.NORTH:
                                        {
                                            int numberOfBlinkingTraps = objectiveRooms[i].roomWidth - 2;
                                            for (int BlinkingTrapsIdx = 0; BlinkingTrapsIdx < numberOfBlinkingTraps; BlinkingTrapsIdx++)
                                            {
                                                obstacletiles[objectiveRooms[i].xpos + BlinkingTrapsIdx + 1][objectiveRooms[i].ypos + objectiveRooms[i].roomHeight - 2] = TileType.TRAPS;
                                                Vector3 BTPos = new Vector3(tilespacing * (objectiveRooms[i].xpos + BlinkingTrapsIdx + 1), tilespacing * (objectiveRooms[i].ypos + objectiveRooms[i].roomHeight - 2), 0);
                                                GameObject BlinkingTrap = Instantiate(Obstacles[idx], BTPos, Quaternion.identity);
                                                BlinkingTrap.transform.parent = ObstacleLayout.transform;
                                            }
                                        }
                                        break;
                                    case RoomScript.DoorDirection.SOUTH:
                                        {
                                            int numberOfBlinkingTraps = objectiveRooms[i].roomWidth - 2;
                                            for (int BlinkingTrapsIdx = 0; BlinkingTrapsIdx < numberOfBlinkingTraps; BlinkingTrapsIdx++)
                                            {
                                                obstacletiles[objectiveRooms[i].xpos + BlinkingTrapsIdx + 1][objectiveRooms[i].ypos + 1] = TileType.TRAPS;
                                                Vector3 BTPos = new Vector3(tilespacing * (objectiveRooms[i].xpos + BlinkingTrapsIdx + 1), tilespacing * (objectiveRooms[i].ypos + 1), 0);
                                                GameObject BlinkingTrap = Instantiate(Obstacles[idx], BTPos, Quaternion.identity);
                                                BlinkingTrap.transform.parent = ObstacleLayout.transform;
                                            }
                                        }
                                        break;
                                    case RoomScript.DoorDirection.EAST:
                                        {
                                            int numberOfBlinkingTraps = objectiveRooms[i].roomHeight - 2;
                                            for (int BlinkingTrapsIdx = 0; BlinkingTrapsIdx < numberOfBlinkingTraps; BlinkingTrapsIdx++)
                                            {
                                                obstacletiles[objectiveRooms[i].xpos + objectiveRooms[i].roomWidth - 2][objectiveRooms[i].ypos + BlinkingTrapsIdx + 1] = TileType.TRAPS;
                                                Vector3 BTPos = new Vector3(tilespacing * (objectiveRooms[i].xpos + objectiveRooms[i].roomWidth - 2), tilespacing * (objectiveRooms[i].ypos + BlinkingTrapsIdx + 1), 0);
                                                GameObject BlinkingTrap = Instantiate(Obstacles[idx], BTPos, Quaternion.identity);
                                                BlinkingTrap.transform.parent = ObstacleLayout.transform;
                                            }
                                        }
                                        break;
                                    case RoomScript.DoorDirection.WEST:
                                        {
                                            int numberOfBlinkingTraps = objectiveRooms[i].roomHeight - 2;
                                            for (int BlinkingTrapsIdx = 0; BlinkingTrapsIdx < numberOfBlinkingTraps; BlinkingTrapsIdx++)
                                            {
                                                obstacletiles[objectiveRooms[i].xpos + 1][objectiveRooms[i].ypos + BlinkingTrapsIdx + 1] = TileType.TRAPS;
                                                Vector3 BTPos = new Vector3(tilespacing * (objectiveRooms[i].xpos + 1), tilespacing * (objectiveRooms[i].ypos + BlinkingTrapsIdx + 1), 0);
                                                GameObject BlinkingTrap = Instantiate(Obstacles[idx], BTPos, Quaternion.identity);
                                                BlinkingTrap.transform.parent = ObstacleLayout.transform;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 3://LASER ALARM OBSTACLE
                    {
                        for (int idx = 0; idx < Obstacles.Count; idx++)
                        {
                            if (Obstacles[idx].name == "LaserAlarm" && LaserAlarmObstacle == true)
                            {

                            }
                        }
                    }
                    break;
            }
        }
    }

    void InstantiateTiles()
    {
        for (int i = 0; i < venttiles.Length; i++)
        {
            for (int j = 0; j < venttiles[i].Length; j++)
            {
                switch (venttiles[i][j])
                {
                    case TileType.VENT:
                        InstantiateFromArray(ventTile[0], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.VENT_E:
                        InstantiateFromArray(ventTile[1], tilespacing * i, tilespacing * j, 0f);
                        break;
                }
            }
        }
        //Amirul: I'll figure out a way to do this better
        for (int i = 0; i < maptiles.Length; i++)
        {
            for (int j = 0; j < maptiles[i].Length; j++)
            {
                switch (maptiles[i][j])
                {
                    case TileType.ROOM:
                        InstantiateFromArray(roomTile, tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.OBJECTIVE_ROOM:
                        InstantiateFromArray(objectiveRoomTile, tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_HORIZONTAL:
                        InstantiateFromArray(wallTile[3], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_VERTICAL:
                        InstantiateFromArray(wallTile[5], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_TOP_LEFT_CORNER:
                        InstantiateFromArray(wallTile[0], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_TOP_RIGHT_CORNER:
                        InstantiateFromArray(wallTile[1], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_BOTTOM_LEFT_CORNER:
                        InstantiateFromArray(wallTile[6], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_BOTTOM_RIGHT_CORNER:
                        InstantiateFromArray(wallTile[7], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_ENDING_RIGHT:
                        InstantiateFromArray(wallTile[4], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_ENDING_LEFT:
                        InstantiateFromArray(wallTile[8], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_ENDING_TOP:
                        InstantiateFromArray(wallTile[2], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.WALL_ENDING_BOTTOM:
                        InstantiateFromArray(wallTile[9], tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.FLOOR:
                        InstantiateFromArray(floorTile, tilespacing * i, tilespacing * j, 0f);
                        break;
                    case TileType.DOOR:
                        InstantiateFromArray(doorTile[0], tilespacing * i, tilespacing * j, 0f);
                        break;
                }
            }
        }
    }

    void InstantiateOuterWalls()
    {
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        InstantiateVerticalOuterWalls(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWalls(rightEdgeX, bottomEdgeY, topEdgeY);

        InstantiateHorizontalOuterWalls(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWalls(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }

    void InstantiateVerticalOuterWalls(float xCoord, float startingY, float endingY)
    {
        float currentY = startingY + 1;
        InstantiateFromArray(wallTile[6], tilespacing * -1, tilespacing * startingY, 0f);
        InstantiateFromArray(wallTile[7], tilespacing * columns, tilespacing * startingY, 0f);
        InstantiateFromArray(wallTile[0], tilespacing * -1, tilespacing * rows, 0f);
        InstantiateFromArray(wallTile[1], tilespacing * columns, tilespacing * rows, 0f);
        while (currentY <= endingY - 1)
        {
            InstantiateFromArray(wallTile[5], tilespacing * xCoord, tilespacing * currentY, 0f);

            currentY++;
        }
    }

    void InstantiateHorizontalOuterWalls(float startingX, float endingX, float yCoord)
    {
        float currentX = startingX;

        while (currentX <= endingX)
        {
            InstantiateFromArray(wallTile[3], tilespacing * currentX, tilespacing * yCoord, 0f);

            currentX++;
        }
    }

    GameObject InstantiateFromArray(GameObject prefabs, float xCoord, float yCoord, float zCoord)
    {
        Vector3 position = new Vector3(xCoord, yCoord, zCoord);
        GameObject tileInstance = Instantiate(prefabs, position, Quaternion.identity) as GameObject;
        
        if (prefabs == ventTile[0])
        {
            tileInstance.transform.parent = VentsLayout.transform;
            VentsLayout.transform.parent = LevelLayout.transform;
        }
        else if (prefabs == ventTile[1])
        {
            tileInstance.transform.parent = VentsEntranceLayout.transform;
            VentsEntranceLayout.transform.parent = LevelLayout.transform;

			tileInstance.GetComponent<SpriteRenderer> ().sortingOrder = 1;
        }
        else if(prefabs == floorTile)
        {
            tileInstance.transform.parent = FloorsLayout.transform;
            FloorsLayout.transform.parent = LevelLayout.transform;
        }
        else if (prefabs == objectiveRoomTile)
        {
            tileInstance.transform.parent = ObjectivesRoomTileLayout.transform;
            ObjectivesRoomTileLayout.transform.parent = LevelLayout.transform;
        }
        else if (prefabs == doorTile[0])
        {
            tileInstance.transform.parent = DoorsTileLayout.transform;
            DoorsTileLayout.transform.parent = LevelLayout.transform;
        }
        else
        {
            tileInstance.transform.parent = LevelLayout.transform;
        }
        for(int i = 0; i < wallTile.Length; i++)
        {    
            if (prefabs == wallTile[i])
            {
                tileInstance.transform.parent = WallsLayout.transform;
                WallsLayout.transform.parent = LevelLayout.transform;
            }
        }
        return tileInstance;
    }

    // Gets the cost of a tile/grid based on the TileType
    public int GetGridCost(int x, int y)
    {
        switch (obstacletiles[x][y])
        {
            case TileType.OBSTACLE: return -1;
            case TileType.TRAPS: return 1;
        }

        switch (maptiles[x][y])
        {
            case TileType.FLOOR: return 1;
            case TileType.HACKABLE_DOOR: return 5;
            //case TileType.WALL_HORIZONTAL: return -1;
            //case TileType.WALL_VERTICAL: return -1;
            //case TileType.WALL_TOP_LEFT_CORNER: return -1;
            //case TileType.WALL_TOP_RIGHT_CORNER: return -1;
            //case TileType.WALL_BOTTOM_LEFT_CORNER: return -1;
            //case TileType.WALL_BOTTOM_RIGHT_CORNER: return -1;
            //case TileType.WALL_ENDING_BOTTOM: return -1;
            //case TileType.WALL_ENDING_RIGHT: return -1;
            //case TileType.WALL_ENDING_LEFT: return -1;
            //case TileType.WALL_ENDING_TOP: return -1;
            //case TileType.VENT_E: return -1;
            case TileType.OBJECTIVE: return -1;
            case TileType.ENTITY: return -1;

        }

        if ((int)maptiles[x][y] >= (int)TileType.WALL_VERTICAL && (int)maptiles[x][y] <= (int)TileType.WALL_ENDING_BOTTOM)
            return -1;

        return 1;
    }

    // Gets vector3 position using x and y as indexes
    public Vector3 GetVec3Pos(int x, int y)
    {
        return new Vector3(x * tilespacing, y * tilespacing, 0);
    }

    // Gets the grid position of a tile based on a vector3 position
    public Vector2 GetGridPos(Vector3 checkPos)
    {
        for (int x = 0; x < columns; ++x)
        {
            for (int y = 0; y < rows; ++y)
            {
                double offset = 0.5 * tilespacing;
                Vector3 tempVec3 = GetVec3Pos(x, y);

                if (checkPos.x <= tempVec3.x + offset &&
                    checkPos.x >= tempVec3.x - offset &&
                    checkPos.y <= tempVec3.y + offset &&
                    checkPos.y >= tempVec3.y - offset)
                {
                    return new Vector2(x, y);
                }
            }
        }

        return new Vector2(-1, -1);
    }

    // Gets TileType with indexes
    public TileType GetTileType(int x, int y)
    {
        return maptiles[x][y];
    }

    // Get a random room from miscRooms
    public RoomScript GetRandomRoom()
    {
        return miscRooms[Random.Range(0, miscRooms.Length - 1)];
    }

	// Get a random room from objtRooms
	public RoomScript GetObjtRooms()
	{
		return objectiveRooms[Random.Range(0, objectiveRooms.Length - 1)];
	}

    // Set a TileType on the given position
    // Also returns the grid position of the object
    public Vector2 AddToArray(Vector3 position, TileType toSet)
    {
        Vector2 gridPos = GetGridPos(position);

        maptiles[(int)gridPos.x][(int)gridPos.y] = toSet;
        return gridPos;
    }

    // Sets the tile at the given position to floor
    public void RemoveFromArray(Vector2 position)
    {
        maptiles[(int)position.x][(int)position.y] = TileType.FLOOR;
    }

    // Get if player can see thru this tile (used for fog of war)
    public bool GetCanSeeThru(int x, int y)
    {
        if ((int)maptiles[x][y] >= (int)TileType.WALL_VERTICAL && (int)maptiles[x][y] <= (int)TileType.WALL_ENDING_BOTTOM)
            return false;
        else
            return true;
    }

    public void MirgratePos(Vector2 oldPos, Vector2 newPos)
    {
        maptiles[(int)oldPos.x][(int)oldPos.y] = TileType.FLOOR;
        maptiles[(int)newPos.x][(int)newPos.y] = TileType.ENTITY;
    }
}
