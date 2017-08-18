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
        INTERACTABLES,
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
    public int numberOfSubObjectives = 0;
    public bool SubObjectivesLevel = false;
    public bool hackableDoorLevel = false;
    public bool RandomAmmoCollecitbles = false;
    public bool RandomHealthpackCollecitbles = false;
    public bool FogOfWar = false;
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
    public List<GameObject> MainObjectives;
    public List<GameObject> SubObjectives;
    public GameObject BossObjective;

    [Space]
    [Header("Enemy Object")]
    public List<SpawningAIData> SpawnList;
    public GameObject BossSpawner;

    [Space]
    [Header("Security Camera Object")]
    public GameObject SecurityCameraObject;

    [Space]
    [Header("Obstacles Objects")]
    public List<GameObject> Obstacles;

    [Space]
    [Header("Collectible Objects")]
    public List<GameObject> Collectibles;

    [Space]
    [Header("Interactables Objects")]
    public List<GameObject> Interactables;

    private TileType[][] maptiles;
    private TileType[][] obstacletiles;
    private TileType[][] venttiles;
    private TileType[][] interactabletiles;

    private List<RoomScript> existingRooms;
    private RoomScript spawnRoom;
    private RoomScript exitRoom;
    private RoomScript[] objectiveRooms;
    private RoomScript[] miscRooms;
    private RoomScript powerRoom;
    private RoomScript securityRoom;

    private GameObject LevelLayout;
    private GameObject VentsLayout;
    private GameObject FloorsLayout;
    private GameObject WallsLayout;
    private GameObject VentsEntranceLayout;
    private GameObject ObjectivesRoomTileLayout;
    private GameObject DoorsTileLayout;
    private GameObject ObstacleLayout;

    private List<GameObject> MainGameObjectiveList = new List<GameObject>();
    private List<GameObject> SubGameObjectiveList = new List<GameObject>();

    private bool areaIsIntersecting;

    private short counter = 0;

    // Use this for initialization
    void Awake()
    {
        GetCurrentStage();
        Debug.Log("CurrentLevel: " + PersistentData.m_Instance.CurrentLevel);
        Debug.Log("CurrentStage: " + GetCurrentStage());
        LevelGeneration(GetCurrentStage());

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
        InstantiateCollectibles();
        InstantiateInteractables();
        if (BossLevel)
        {
            InstantiateBoss();
            InstantiateBossObjective();
        }
        else
        {
            InstantiateSecurityObject();
            InstantiateMainObjective();
            if (SubObjectivesLevel)
                InstantiateSubObjective();
            if (GlassObstacle || BlinkingTrapObstacle || WaitTrapObstacle || LaserAlarmObstacle)
                InstantiateObstacle();
            InstantiateEnemyPosition();
        }

        //Added by Randall - To set the black image for the Vent Entrances
        GameObject bi = Instantiate(Resources.Load("BlackImage")) as GameObject;
        bi.SetActive(false);
        VentInspect[] via = VentsEntranceLayout.GetComponentsInChildren<VentInspect>();
        foreach (VentInspect vi in via)
            vi.black_image = bi;

        Debug.Log("Level Spawned");
    }

    public int GetCurrentStage()
    {
        int stageNumber = 1;
        while (PersistentData.m_Instance.CurrentLevel > stageNumber * 3)
        {
            stageNumber += 1;
        }
        
        // added by don
        PersistentData.m_Instance.NumTraitsPassDown = stageNumber * 2;

        return stageNumber;
    }

    void LevelGeneration(int stageNumber)
    {
        if (PersistentData.m_Instance.CurrentLevel == stageNumber * 3)
            BossLevel = true;
        else
            BossLevel = false;

        //Level Values
        columns = 25 + (10 * stageNumber);
        if (columns >= 50)
        {
            columns = 50;
        }
        rows = 25 + (10 * stageNumber);
        if (rows >= 50)
        {
            rows = 50;
        }


        //Room Datas
        numberOfMiscRooms = 2 + (2 * stageNumber);
        numberOfObjectiveRooms = 1 * stageNumber;
        roomWidth = new IntRange(3 + (2 * stageNumber), 5 + (3 * stageNumber));
        if(roomWidth.m_Min >= 7)
        {
            roomWidth.m_Min = 7;
        }
        if(roomWidth.m_Max >= 11)
        {
            roomWidth.m_Max = 11;
        }
        roomHeight = new IntRange(3 + (2 * stageNumber), 5 + (3 * stageNumber));
        if (roomHeight.m_Min >= 7)
        {
            roomHeight.m_Min = 7;
        }
        if (roomHeight.m_Max >= 11)
        {
            roomHeight.m_Max = 11;
        }

        //Collectibles
        RandomAmmoCollecitbles = true;
        RandomHealthpackCollecitbles = true;
        numberOfCollectibles = 5 * stageNumber;

        //Obstacles
        switch (BossLevel)
        {
            case true:
                {
                    columns = 20 + (15 * stageNumber);
                    if (columns >= 50)
                    {
                        columns = 50;
                    }
                    rows = 20 + (15 * stageNumber);
                    if (columns >= 50)
                    {
                        columns = 50;
                    }

                    numberOfMiscRooms = 2 + (2 * stageNumber);
                    roomWidth = new IntRange(3 + (2 * stageNumber), 4 + (3 * stageNumber));
                    roomHeight = new IntRange(3 + (2 * stageNumber), 4 + (3 * stageNumber));
                }
                break;
            case false:
                {
                    GlassObstacle = true;
                    BlinkingTrapObstacle = true;
                    WaitTrapObstacle = true;
                    LaserAlarmObstacle = true;
                    numberOfObstaclesPerRoom = 1 * stageNumber;

                    //Enemies Spawn
                    for (int i = 0; i < SpawnList.Count; i++)
                    {
                        switch (SpawnList[i].name)
                        {
                            case "Civilian":
                                SpawnList[i].amount = 3 * stageNumber;
                                break;
                            case "MeleeEnemy":
                                {
                                    SpawnList[i].amount = 1 * stageNumber;
                                    SpawnList[i].stateMachine.GetComponentInChildren<EnemySM>().role = (EnemySM.ENEMY_ROLE)Random.Range((int)EnemySM.ENEMY_ROLE.GUARD, (int)EnemySM.ENEMY_ROLE.WANDER);
                                }
                                break;
                            case "RangeEnemy":
                                {
                                    SpawnList[i].amount = 1 * stageNumber;
                                    SpawnList[i].stateMachine.GetComponentInChildren<EnemySM>().role = (EnemySM.ENEMY_ROLE)Random.Range((int)EnemySM.ENEMY_ROLE.GUARD, (int)EnemySM.ENEMY_ROLE.WANDER);
                                }
                                break;
                        }
                    }
                }
                break;
        }

        for (int idx = 0; idx < PersistentData.m_Instance.PlayerTraits.Count; idx++ )
        {
            if (PersistentData.m_Instance.PlayerTraits[idx].GetComponent<Trait_Hacking>())
            {
                hackableDoorLevel = true;
            }
        }

        switch (stageNumber)
        {
            case 1:
                {
                    //JUST A DEFAULT LEVEL
                    if(BossLevel)
                    {
                        SubObjectivesLevel = true;
                    }
                }
                break;
            case 2:
                {
                    numberOfSubObjectives = stageNumber - 1;
                    SubObjectivesLevel = true;
                }
                break;
            case 3:
                {
                    numberOfSubObjectives = stageNumber - 1;
                    FogOfWar = true;
                }
                break;
        }
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
        interactabletiles = new TileType[columns][];
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i] = new TileType[rows];
            venttiles[i] = new TileType[rows];
            obstacletiles[i] = new TileType[rows];
            interactabletiles[i] = new TileType[rows];
        }
    }

    void CreateRooms()
    {
        existingRooms = new List<RoomScript>();
        spawnRoom = new RoomScript();
        exitRoom = new RoomScript();
        powerRoom = new RoomScript();
        securityRoom = new RoomScript();
        objectiveRooms = new RoomScript[numberOfObjectiveRooms];
        miscRooms = new RoomScript[numberOfMiscRooms];

        RecursiveFindEmptyPos(spawnRoom, existingRooms, RoomScript.RoomType.SPAWN);
        existingRooms.Add(spawnRoom);

        if (FogOfWar)
        {
            RecursiveFindEmptyPos(powerRoom, existingRooms, RoomScript.RoomType.POWER);
            if (hackableDoorLevel)
            {
                powerRoom.doorType = Random.Range(0, 2);
            }
            existingRooms.Add(powerRoom);
        }

        RecursiveFindEmptyPos(securityRoom, existingRooms, RoomScript.RoomType.SECURITYCONSOLE);
        if (hackableDoorLevel)
        {
            securityRoom.doorType = Random.Range(0, 2);
        }
        existingRooms.Add(securityRoom);

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
            if (hackableDoorLevel)
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
                        case RoomScript.RoomType.POWER:
                            {
                                maptiles[xCoord][yCoord] = TileType.ROOM;
                            }
                            break;
                        case RoomScript.RoomType.SECURITYCONSOLE:
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
                    typesToIgnore.Add(RoomScript.RoomType.POWER);
                    typesToIgnore.Add(RoomScript.RoomType.SECURITYCONSOLE);
                    break;
                case SpawningAIData.SPAWN_ROOM_TYPE.MISC:
                    typesToIgnore.Add(RoomScript.RoomType.SPAWN);
                    typesToIgnore.Add(RoomScript.RoomType.EXIT);
                    typesToIgnore.Add(RoomScript.RoomType.OBJECTIVES);
                    typesToIgnore.Add(RoomScript.RoomType.POWER);
                    typesToIgnore.Add(RoomScript.RoomType.SECURITYCONSOLE);
                    break;
                case SpawningAIData.SPAWN_ROOM_TYPE.OBJECTIVE:
                    typesToIgnore.Add(RoomScript.RoomType.SPAWN);
                    typesToIgnore.Add(RoomScript.RoomType.EXIT);
                    typesToIgnore.Add(RoomScript.RoomType.MISC);
                    typesToIgnore.Add(RoomScript.RoomType.POWER);
                    typesToIgnore.Add(RoomScript.RoomType.SECURITYCONSOLE);
                    break;
            }

            while (typesToIgnore.Contains(existingRooms[randomRoom].roomType))
            {
                randomRoom = Random.Range(0, existingRooms.Count);
            }


            Vector3 spawnPos = new Vector3(tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].xpos + (existingRooms[randomRoom].roomWidth / 2)), tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].ypos + (existingRooms[randomRoom].roomHeight / 2)), 1f);
            GameObject spawnedSM = Instantiate(SpawnList[spawnIdx].stateMachine, spawnPos, Quaternion.identity);

            spawnedSM.GetComponentInChildren<Pathfinder>().theLevelManager = this;
			spawnedSM.GetComponentInChildren<BaseSM> ().SpawnPoint = spawnPos;
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

    void InstantiateBoss()
    {
        int BossSpawnRoom = Random.Range(0, objectiveRooms.Length);

        int BossXPos = Mathf.RoundToInt(objectiveRooms[BossSpawnRoom].xpos + (objectiveRooms[BossSpawnRoom].roomWidth / 2));
        int BossYPos = Mathf.RoundToInt(objectiveRooms[BossSpawnRoom].ypos + (objectiveRooms[BossSpawnRoom].roomHeight / 2));

        Vector3 bossSpawnerPos = new Vector3(tilespacing * BossXPos, tilespacing * BossYPos, 0);
        GameObject go = Instantiate(BossSpawner, bossSpawnerPos, Quaternion.identity);

        go.GetComponent<BossGenerator>().levelManagerRef = this;
    }

    void InstantiateSecurityObject()
    {
        foreach(var oR in objectiveRooms)
        {
            Vector3 SCPosition = new Vector3(tilespacing * (oR.xpos + oR.roomWidth - 2), tilespacing * (oR.ypos + 1), 0f);
            GameObject SCObject = Instantiate(SecurityCameraObject, SCPosition, Quaternion.identity) as GameObject;
        }
    }

    void InstantiateCollectibles()
    {
        for (int i = 0; i < numberOfCollectibles; i++)
        {
            Vector3 RandomPosInRoom;
            int randomMiscRoomNumber = Random.Range(0, miscRooms.Length);
            int RandomXPos = Random.Range(miscRooms[randomMiscRoomNumber].xpos + 1, miscRooms[randomMiscRoomNumber].xpos + miscRooms[randomMiscRoomNumber].roomWidth - 1);
            int RandomYPos = Random.Range(miscRooms[randomMiscRoomNumber].ypos + 1, miscRooms[randomMiscRoomNumber].ypos + miscRooms[randomMiscRoomNumber].roomHeight - 1);
            while(venttiles[RandomXPos][RandomYPos] == TileType.VENT_E)
            {
                RandomXPos = Random.Range(miscRooms[randomMiscRoomNumber].xpos + 1, miscRooms[randomMiscRoomNumber].xpos + miscRooms[randomMiscRoomNumber].roomWidth - 1);
                RandomYPos = Random.Range(miscRooms[randomMiscRoomNumber].ypos + 1, miscRooms[randomMiscRoomNumber].ypos + miscRooms[randomMiscRoomNumber].roomHeight - 1);
                if (venttiles[RandomXPos][RandomYPos] != TileType.VENT_E)
                    break;
            }
            RandomPosInRoom = new Vector3(tilespacing * RandomXPos, tilespacing * RandomYPos, -1.0f);

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

    void InstantiateInteractables()
    {
        for(int i = 0; i < Interactables.Count; i++)
        {
            switch(Interactables[i].name)
            {
                case "PowerSwitch":
                    {
                        if(FogOfWar)
                        {
                            int XPos = powerRoom.xpos + Mathf.RoundToInt(powerRoom.roomWidth / 2);
                            int YPos = powerRoom.ypos + Mathf.RoundToInt(powerRoom.roomHeight / 2);
                            interactabletiles[XPos][YPos] = TileType.INTERACTABLES;
                            Vector3 PowerPos = new Vector3(tilespacing * XPos, tilespacing * YPos, 0);
                            GameObject Power = Instantiate(Interactables[i], PowerPos, Quaternion.identity);

                            //Instantiating For Power Switch
                            for (int idx = 0; idx < Obstacles.Count; idx++)
                            {
                                if (Obstacles[idx].name == "Glass")
                                {
                                    int x = 0;
                                    int y = 0;

                                    int minX = XPos - 1;
                                    int maxX = XPos + 1;

                                    int minY = YPos - 1;
                                    int maxY = YPos + 1;

                                    for (x = 0; x <= (maxX - minX); x++)
                                    {
                                        for (y = 0; y <= (maxY - minY); y++)
                                        {
                                            if ((int)venttiles[minX + x][minY + y] == (int)TileType.VENT_E)
                                            {
                                                obstacletiles[minX + x][minY + y] = TileType.VENT_E;
                                            }
                                            else if ((int)interactabletiles[minX + x][minY + y] == (int)TileType.INTERACTABLES)
                                            {
                                                obstacletiles[minX + x][minY + y] = TileType.FLOOR;
                                                Instantiate(floorTile, new Vector3(tilespacing * (minX + x), tilespacing * (minY + y), 0), Quaternion.identity);
                                            }
                                            else
                                            {
                                                obstacletiles[minX + x][minY + y] = TileType.OBSTACLE;
                                            }

                                            if ((int)obstacletiles[minX + x][minY + y] == (int)TileType.OBSTACLE)
                                            {
                                                GameObject GlassObstacle = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX + x), tilespacing * (minY + y), 0), Quaternion.identity);
                                                GlassObstacle.transform.parent = ObstacleLayout.transform;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "SecurityCameraConsole":
                    {
                        int XPos = securityRoom.xpos + Mathf.RoundToInt(securityRoom.roomWidth / 2);
                        int YPos = securityRoom.ypos + Mathf.RoundToInt(securityRoom.roomHeight / 2);
                        interactabletiles[XPos][YPos] = TileType.INTERACTABLES;
                        Vector3 ConsolePos = new Vector3(tilespacing * XPos, tilespacing * YPos, 0);
                        GameObject Console = Instantiate(Interactables[i], ConsolePos, Quaternion.identity);
                    }
                    break;
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

    void InstantiateMainObjective()
    {
        //Instantaitable Objectives
        for (int i = 0; i < objectiveRooms.Length; i++)
        {
            int ObjectiveXPos = Mathf.RoundToInt(objectiveRooms[i].xpos + (objectiveRooms[i].roomWidth / 2));
            int ObjectiveYPos = Mathf.RoundToInt(objectiveRooms[i].ypos + (objectiveRooms[i].roomHeight / 2));

            Vector3 ObjectivePos = new Vector3(tilespacing * ObjectiveXPos, tilespacing * ObjectiveYPos, -1f);
            GameObject go = Instantiate(MainObjectives[Random.Range(0, MainObjectives.Count)], ObjectivePos, Quaternion.identity) as GameObject;
            MainGameObjectiveList.Add(go);
            if (go.tag == "Rescue")
            {
                go.GetComponentInChildren<Pathfinder>().theLevelManager = this;
                go.GetComponentInChildren<BaseSM>().SpawnPoint = ObjectivePos;
                maptiles[ObjectiveXPos][ObjectiveYPos] = TileType.OBJECTIVE;
            }
            else
                maptiles[ObjectiveXPos][ObjectiveYPos] = TileType.OBJECTIVE;
            
        }
    }

    void InstantiateSubObjective()
    {
        if (SubObjectivesLevel)
        {
            for (int i = 0; i < numberOfSubObjectives; i++)
            {
                int randomSubObjectives = Random.Range(0, SubObjectives.Count);
                GameObject sgo = Instantiate(SubObjectives[randomSubObjectives]) as GameObject;
                for(int idx = 0; idx < MainGameObjectiveList.Count; idx++)
                {
                    for(int idx2 = 0; idx2 < SubGameObjectiveList.Count; idx2++)
                    {
                        if(SubGameObjectiveList[idx2].GetComponent<PlayerActionLimitObjt>() && MainGameObjectiveList[idx].GetComponent<DefeatEnemy>())
                        {
                            InstantiateSubObjective();
                        }
                        else
                        {
                            while(sgo == SubGameObjectiveList[idx2])
                            {
                                InstantiateSubObjective();
                            }
                            if(sgo != SubGameObjectiveList[idx2])
                            {
                                SubGameObjectiveList.Add(sgo);
                            }
                        }
                    }
                }

            }
        }
    }

    void InstantiateBossObjective()
    {
        GameObject bgo = Instantiate(BossObjective) as GameObject;
    }

    void InstantiateObstacle()
    {
        for (int i = 0; i < objectiveRooms.Length; i++)
        {
            for(int obsNum = 0; obsNum < numberOfObstaclesPerRoom; obsNum++)
            {
                int RandomObstacle = Random.Range(0, Obstacles.Count);
                InstantiateObstacleID(RandomObstacle, objectiveRooms[i]);
                int temp = RandomObstacle;
                while (RandomObstacle == temp)
                {
                    RandomObstacle = Random.Range(0, Obstacles.Count);
                }
            }
        }
    }

    void InstantiateObstacleID(int ObstacleID, RoomScript ObjectiveRoom)
    {
        switch (ObstacleID)
        {
            case 0://GLASS OBSTACLE
                {
                    for (int idx = 0; idx < Obstacles.Count; idx++)
                    {
                        if (Obstacles[idx].name == "Glass" && GlassObstacle == true)
                        {
                            int x = 0;
                            int y = 0;

                            int minX = Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) - 1;
                            int maxX = Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) + 1;

                            int minY = Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)) - 1;
                            int maxY = Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)) + 1;

                            for(x = 0; x <= (maxX - minX); x++)
                            {
                                for(y = 0; y <= (maxY - minY); y++)
                                {
                                    if ((minX + x) != Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) || (minY + y) != Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)))
                                    {
                                        obstacletiles[minX + x][minY + y] = TileType.OBSTACLE;
                                        GameObject GlassObject = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX + x), tilespacing * (minY + y), 0), Quaternion.identity);
                                        GlassObject.transform.parent = ObstacleLayout.transform;
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 1://WAIT TRAP OBSTACLE
                {
                    int numberOfWaitTraps = Random.Range(1, 5);
                    for (int idx = 0; idx < Obstacles.Count; idx++)
                    {
                        if (Obstacles[idx].name == "WaitTrap" && WaitTrapObstacle == true)
                        {
                            for (int num = 0; num < numberOfWaitTraps; num++)
                            {
                                int RandomXPos = Random.Range(ObjectiveRoom.xpos + 1, ObjectiveRoom.xpos + ObjectiveRoom.roomWidth - 2);
                                int RandomYPos = Random.Range(ObjectiveRoom.ypos + 1, ObjectiveRoom.ypos + ObjectiveRoom.roomHeight - 2);

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
                            int x = 0;
                            int y = 0;

                            int minX = Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) - 1;
                            int maxX = Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) + 1;

                            int minY = Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)) - 1;
                            int maxY = Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)) + 1;

                            for (x = 0; x <= (maxX - minX); x++)
                            {
                                for (y = 0; y <= (maxY - minY); y++)
                                {
                                    if (minX + x != Mathf.RoundToInt(ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth / 2)) || minY + y != Mathf.RoundToInt(ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight / 2)))
                                    {
                                        obstacletiles[minX + x][minY + y] = TileType.OBSTACLE;
                                        GameObject BlinkingTrapObject = Instantiate(Obstacles[idx], new Vector3(tilespacing * (minX + x), tilespacing * (minY + y), 0), Quaternion.identity);
                                        BlinkingTrapObject.transform.parent = ObstacleLayout.transform;
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 3://LASER ALARM OBSTACLE
                {
                    for (int idx = 0; idx < Obstacles.Count; idx++)
                    {
                        if (Obstacles[idx].name == "LaserAlarmObstacle" && LaserAlarmObstacle == true)
                        {
                            switch(ObjectiveRoom.doorDirection)
                            {
                                case RoomScript.DoorDirection.NORTH:
                                    {
                                        float fromX = (tilespacing * ObjectiveRoom.xpos);
                                        float toX = (tilespacing * ObjectiveRoom.xpos) + ((tilespacing * ObjectiveRoom.roomWidth) - 1);

                                        //Debug.Log(tilespacing * (ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth * 0.5f)));
                                        Debug.Log(tilespacing * ((ObjectiveRoom.xpos + ObjectiveRoom.roomWidth) * 0.5f));

                                        Vector3 MidXPos = new Vector3(((ObjectiveRoom.xpos + ObjectiveRoom.roomWidth * 0.5f) * tilespacing) - 0.15f, tilespacing * (ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight - 2)), 0);

                                        GameObject LaserAlarm = Instantiate(Obstacles[idx], MidXPos, Quaternion.Euler(0, 0, 90));
                                        LaserAlarm.GetComponentInChildren<LaserAlarm>().gameObject.transform.localScale = new Vector3(1, 2 * (toX - fromX), 1);

                                        LaserAlarm.transform.parent = ObstacleLayout.transform;
                                    }
                                    break;
                                case RoomScript.DoorDirection.SOUTH:
                                    {
                                        float fromX = (tilespacing * ObjectiveRoom.xpos);
                                        float toX = (tilespacing * ObjectiveRoom.xpos) + ((tilespacing * ObjectiveRoom.roomWidth) - 1);

                                        //Debug.Log(tilespacing * (ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth * 0.5f)));

                                        Vector3 MidXPos = new Vector3(((ObjectiveRoom.xpos + ObjectiveRoom.roomWidth * 0.5f) * tilespacing) - 0.15f, tilespacing * (ObjectiveRoom.ypos + 1), 0);

                                        GameObject LaserAlarm = Instantiate(Obstacles[idx], MidXPos, Quaternion.Euler(0, 0, 90));
                                        LaserAlarm.GetComponentInChildren<LaserAlarm>().gameObject.transform.localScale = new Vector3(1, 2 * (toX - fromX), 1);

                                        LaserAlarm.transform.parent = ObstacleLayout.transform;
                                    }
                                    break;
                                case RoomScript.DoorDirection.EAST:
                                    {
                                        float fromY = (tilespacing * ObjectiveRoom.ypos);
                                        float toY = (tilespacing * ObjectiveRoom.ypos) + ((tilespacing * ObjectiveRoom.roomHeight) - 1);

                                        //Debug.Log(tilespacing * (ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight * 0.5f)));

                                        Vector3 MidYPos = new Vector3(tilespacing * (ObjectiveRoom.xpos + (ObjectiveRoom.roomWidth - 2)), ((ObjectiveRoom.ypos + ObjectiveRoom.roomHeight * 0.5f) * tilespacing) - 0.15f, 0);

                                        GameObject LaserAlarm = Instantiate(Obstacles[idx], MidYPos, Quaternion.Euler(0, 0, 0));
                                        LaserAlarm.GetComponentInChildren<LaserAlarm>().gameObject.transform.localScale = new Vector3(1, 2 * (toY - fromY), 1);

                                        LaserAlarm.transform.parent = ObstacleLayout.transform;
                                    }
                                    break;
                                case RoomScript.DoorDirection.WEST:
                                    {
                                        float fromY = (tilespacing * ObjectiveRoom.ypos);
                                        float toY = (tilespacing * ObjectiveRoom.ypos) + ((tilespacing * ObjectiveRoom.roomHeight) - 1);

                                        //Debug.Log(tilespacing * (ObjectiveRoom.ypos + (ObjectiveRoom.roomHeight * 0.5f)));

                                        Vector3 MidYPos = new Vector3(tilespacing * (ObjectiveRoom.xpos + 1), ((ObjectiveRoom.ypos + ObjectiveRoom.roomHeight * 0.5f) * tilespacing) - 0.15f, 0);

                                        GameObject LaserAlarm = Instantiate(Obstacles[idx], MidYPos, Quaternion.Euler(0, 0, 0));
                                        LaserAlarm.GetComponentInChildren<LaserAlarm>().gameObject.transform.localScale = new Vector3(1, 2 * (toY - fromY), 1);

                                        LaserAlarm.transform.parent = ObstacleLayout.transform;
                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
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

        switch (interactabletiles[x][y])
        {
            case TileType.INTERACTABLES: return -1;
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

    // Gets the array of misc rooms
    public RoomScript[] GetAllMiscRooms()
    {
        return miscRooms;
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
