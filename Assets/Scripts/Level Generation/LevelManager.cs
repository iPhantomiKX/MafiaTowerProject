using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public enum TileType
    {
        FLOOR,
        WALL,
        VENT,
        VENT_E, // VENT ENTRANCE/EXIT
        ROOM,
        DOOR,
        HACKABLE_DOOR,
        OBJECTIVE_ROOM,
        OBJECTIVE,
    };

    [Header("Level Dimension")]
    public int columns = 100;
    public int rows = 100;
    public float tilespacing = 0.275f;
    public bool hackableDoorLevel = false;

    [Space]
    [Header("Room Modifications")]
    public int numberOfObjectiveRooms = 0;
    public int numberOfMiscRooms = 0;

    [Space]
    [Header("Room Dimensions")]
    public IntRange roomWidth = new IntRange(5, 10);
    public IntRange roomHeight = new IntRange(5, 10);

    [Space]
    [Header("Tile Types")]
    public GameObject floorTile;
    public GameObject wallTile;
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
    [Header("Hostage Object")]
    public GameObject HostageObject;

    [Space]
    [Header("Find Item Object")]
    public GameObject FindItemObject;

    [Space]
    [Header("Enemy Object")]
    public GameObject EnemyObject;

    private TileType[][] maptiles;
    private TileType[][] venttiles;
    private RoomScript[] rooms;

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
        //

        LevelLayout = new GameObject("LevelLayout");
        VentsLayout = new GameObject("VentsLayout");
        FloorsLayout = new GameObject("FloorsLayout");
        WallsLayout = new GameObject("WallsLayout");
        VentsEntranceLayout = new GameObject("VentsEntranceLayout");
        ObjectivesRoomTileLayout = new GameObject("ObjectivesRoomTileLayout");
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
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();
        //InstantiateEnemyPosition();

        Debug.Log("Level Spawned");
        //Debug.Log(columns);
    }

    void Update()
    {
        //HOTFIX Added by Randall - THIS IS QUITE BAD BUT UH IDK 
        if(counter++ == 1)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Vent"), LayerMask.NameToLayer("Vent"));
            VentsLayout.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void SetupTilesArray()
    {
        maptiles = new TileType[columns][];
        venttiles = new TileType[columns][];
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i] = new TileType[rows];
            venttiles[i] = new TileType[rows];
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
            IntRange objectiveType = new IntRange((int)RoomScript.RoomType.HOSTAGE, (int)RoomScript.RoomType.MISC);
            RecursiveFindEmptyPos(objectiveRooms[i], existingRooms, (RoomScript.RoomType)objectiveType.Random);
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
        //SetTilesForVent(spawnRoom, exitRoom);
        for (int i = 0; i < existingRooms.Count; i++)
        {
            if (i == existingRooms.Count - 1)
            {
                break;
            }
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
                        case RoomScript.RoomType.HOSTAGE:
                            {
                                maptiles[xCoord][yCoord] = TileType.OBJECTIVE_ROOM;
                            }
                            break;
                        case RoomScript.RoomType.ITEM:
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
                    //left wall
                    maptiles[currentRoom.xpos][yCoord] = TileType.WALL;
                    //right wall
                    maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][yCoord] = TileType.WALL;
                    //bottom wall
                    maptiles[xCoord][currentRoom.ypos] = TileType.WALL;
                    //top wall
                    maptiles[xCoord][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL;
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
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[doorXpos][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.HACKABLE_DOOR;
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
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[doorXpos][currentRoom.ypos] = TileType.HACKABLE_DOOR;
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
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[currentRoom.xpos][doorYPos] = TileType.HACKABLE_DOOR;
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
                        }
                        else if (currentRoom.doorType == 1)
                        {
                            maptiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos] = TileType.HACKABLE_DOOR;
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

        int x = 0;
        int y = 0;

        int ResultantXVector = 0;
        int ResultantYVector = 0;

        ////Check if Otherroom is on left side of room1
        if(room2CenterXVector < room1.xpos + 1)
        {
            ResultantXVector = (room2.xpos + room2.roomWidth - 2) - (room1.xpos + 1);
            venttiles[(room1.xpos + 1)][room1CenterYVector] = TileType.VENT_E;
            while (x != ResultantXVector)
            {
                int xCoord = (room1.xpos) + x;
                venttiles[xCoord][room1CenterYVector] = TileType.VENT;
                x--;
                if (xCoord >= columns || xCoord <= 0)
                    break;
            }
            if (room1CenterYVector == room2CenterYVector)
            {
                venttiles[(room2.xpos + room2.roomWidth - 2)][room1CenterYVector] = TileType.VENT_E;
            }

            //if room2 is bottom of room1
            if (room2CenterYVector < room1.ypos)
            {
                ResultantYVector = room2CenterYVector - room1CenterYVector;
                venttiles[(room2.xpos + room2.roomWidth - 2)][room2CenterYVector] = TileType.VENT_E;
                while (y != ResultantYVector)
                {
                    int yCoord = (room1CenterYVector) + y;
                    venttiles[(room2.xpos + room2.roomWidth - 2)][yCoord] = TileType.VENT;
                    y--;
                }
            }
            //if room2 is top of room1
            else if (room2CenterYVector > room1.ypos + room1.roomHeight)
            {
                ResultantYVector = room2CenterYVector - room1CenterYVector;
                venttiles[(room2.xpos + room2.roomWidth - 2)][room2CenterYVector] = TileType.VENT_E;
                while (y != ResultantYVector)
                {
                    int yCoord = (room1CenterYVector) + y;
                    venttiles[(room2.xpos + room2.roomWidth - 2)][yCoord] = TileType.VENT;
                    y++;
                }
            }
        }

        //Check if Otherroom is on right side of room1
        else if (room2CenterXVector > room1.xpos + room1.roomWidth - 2)
        {
            ResultantXVector = (room2.xpos + 1) - (room1.xpos + room1.roomWidth - 2);
            venttiles[(room1.xpos + room1.roomWidth - 2)][room1CenterYVector] = TileType.VENT_E;
            while (x != ResultantXVector)
            {
                int xCoord = (room1.xpos + room1.roomWidth - 2) + x;
                venttiles[xCoord][room1CenterYVector] = TileType.VENT;
                x++;
                if (xCoord >= columns || xCoord <= 0)
                    break;
            }
            if (room1CenterYVector == room2CenterYVector)
            {
                venttiles[(room2.xpos + 1)][room1CenterYVector] = TileType.VENT_E;
            }

            //if room2 is bottom of room1
            if (room2CenterYVector < room1.ypos)
            {
                ResultantYVector = room2CenterYVector - room1CenterYVector;
                venttiles[(room2.xpos + 1)][room2CenterYVector] = TileType.VENT_E;
                while (y != ResultantYVector)
                {
                    int yCoord = (room1CenterYVector) + y;
                    venttiles[(room2.xpos + 1)][yCoord] = TileType.VENT;
                    y--;
                }
            }
            //if room2 is top of room1
            else if (room2CenterYVector > room1.ypos + room1.roomHeight)
            {
                ResultantYVector = room2CenterYVector - room1CenterYVector;
                venttiles[(room2.xpos + 1)][room2CenterYVector] = TileType.VENT_E;
                while (y != ResultantYVector)
                {
                    int yCoord = (room1CenterYVector) + y;
                    venttiles[(room2.xpos)][yCoord] = TileType.VENT;
                    y++;
                }
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
        int randomRoom = Random.Range(0, existingRooms.Count);
        while (existingRooms[randomRoom].roomType != RoomScript.RoomType.MISC)
        {
            randomRoom = Random.Range(0, existingRooms.Count);
        }
    
        if (existingRooms[randomRoom].roomType == RoomScript.RoomType.MISC)
        {
            Vector3 enemyPos = new Vector3(tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].xpos + (existingRooms[randomRoom].roomWidth / 2)), tilespacing * Mathf.RoundToInt(existingRooms[randomRoom].ypos + (existingRooms[randomRoom].roomHeight / 2)), 1f);
            GameObject enemy = Instantiate(EnemyObject, enemyPos, Quaternion.identity);
            //enemy.GetComponent<EnemyController>().player = GameObject.FindGameObjectWithTag("Player");    // Not used anymore - Don

            enemy.GetComponentInChildren<Pathfinder>().theLevelManager = this;
            Debug.Log(existingRooms[randomRoom].roomType);
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
        for (int i = 0; i < existingRooms.Count; i++)
        {
            if (existingRooms[i].roomType == RoomScript.RoomType.HOSTAGE)
            {
                int ObjectiveXPos = Mathf.RoundToInt(existingRooms[i].xpos + (existingRooms[i].roomWidth / 2));
                int ObjectiveYPos = Mathf.RoundToInt(existingRooms[i].ypos + (existingRooms[i].roomHeight / 2));

                Vector3 ObjectivePos = new Vector3(tilespacing * ObjectiveXPos, tilespacing * ObjectiveYPos, -1f);
                GameObject go = Instantiate(HostageObject, ObjectivePos, Quaternion.identity) as GameObject;

                go.GetComponent<Pathfinder>().theLevelManager = this;
                maptiles[ObjectiveXPos][ObjectiveYPos] = TileType.OBJECTIVE;
            }

            else if (existingRooms[i].roomType == RoomScript.RoomType.ITEM)
            {
                int ObjectiveXPos = Mathf.RoundToInt(existingRooms[i].xpos + (existingRooms[i].roomWidth / 2));
                int ObjectiveYPos = Mathf.RoundToInt(existingRooms[i].ypos + (existingRooms[i].roomHeight / 2));

                Vector3 ObjectivePos = new Vector3(tilespacing * ObjectiveXPos, tilespacing * ObjectiveYPos, -1f);
                Instantiate(FindItemObject, ObjectivePos, Quaternion.identity);

                maptiles[ObjectiveXPos][ObjectiveYPos] = TileType.OBJECTIVE;
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
                    case TileType.WALL:
                        InstantiateFromArray(wallTile, tilespacing * i, tilespacing * j, 0f);
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
        float currentY = startingY;

        while (currentY <= endingY)
        {
            InstantiateFromArray(wallTile, tilespacing * xCoord, tilespacing * currentY, 0f);

            currentY++;
        }
    }

    void InstantiateHorizontalOuterWalls(float startingX, float endingX, float yCoord)
    {
        float currentX = startingX;

        while (currentX <= endingX)
        {
            InstantiateFromArray(wallTile, tilespacing * currentX, tilespacing * yCoord, 0f);

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
        }
        else if(prefabs == floorTile)
        {
            tileInstance.transform.parent = FloorsLayout.transform;
            FloorsLayout.transform.parent = LevelLayout.transform;
        }
        else if (prefabs == wallTile)
        {
            tileInstance.transform.parent = WallsLayout.transform;
            WallsLayout.transform.parent = LevelLayout.transform;
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

        return tileInstance;
    }

    // Gets the cost of a tile/grid based on the TileType
    public int GetGridCost(int x, int y)
    {
        switch (maptiles[x][y])
        {
            case TileType.FLOOR: return 1;
            case TileType.HACKABLE_DOOR: return 5;
            case TileType.WALL: return -1;
            case TileType.VENT_E: return -1;
            case TileType.OBJECTIVE: return -1;

            default: return 1;
        }
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

    public TileType GetTileType(int x, int y)
    {
        return maptiles[x][y];
    }

    public RoomScript GetRandomRoom()
    {
        return miscRooms[Random.Range(0, miscRooms.Length - 1)];
    }
}
