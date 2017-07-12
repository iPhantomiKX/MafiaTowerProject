using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    enum TileType
    {
        FLOOR,
        WALL,
        VENT,
        VENT_E, // VENT ENTRANCE/EXIT
        ROOM,
        DOOR,
        OBJECTIVE_ROOM,
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
    [Header("Room Dimensions")]
    public IntRange roomWidth = new IntRange(5, 10);
    public IntRange roomHeight = new IntRange(5, 10);

    [Space]
    [Header("Tile Types")]
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject roomTile;
    public GameObject objectiveRoomTile;
    public GameObject doorTile;
    public GameObject[] ventTile;

    public GameObject PlayerObject;

    private TileType[][] tiles;
    private TileType[][] groundtiles;
    private RoomScript[] rooms;

    private List<RoomScript> existingRooms;
    private RoomScript spawnRoom;
    private RoomScript exitRoom;
    private RoomScript[] objectiveRooms;
    private RoomScript[] miscRooms;
    //privaet Corridor[] corridors;
    private GameObject LevelLayout;

    private bool areaIsIntersecting;

	// Use this for initialization
	void Start () {
        LevelLayout = new GameObject("LevelLayout");

        SetupTilesArray();

        CreateRooms();

        SetTilesValueForRooms();
        CreateCorridors();

        SetPlayerPosition();

        InstantiateTiles();
        InstantiateOuterWalls();
	}

    void SetupTilesArray()
    {
        tiles = new TileType[columns][];
        groundtiles = new TileType[columns][];
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[rows];
            groundtiles[i] = new TileType[rows];
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
            existingRooms.Add(miscRooms[i]);
        }

        for (int i = 0; i < numberOfObjectiveRooms; i++)
        {
            objectiveRooms[i] = new RoomScript();
            IntRange objectiveType = new IntRange((int)RoomScript.RoomType.HOSTAGE, (int)RoomScript.RoomType.ITEM);
            RecursiveFindEmptyPos(objectiveRooms[i], existingRooms, (RoomScript.RoomType)objectiveType.Random);
            existingRooms.Add(objectiveRooms[i]);
        }

        RecursiveFindEmptyPos(exitRoom, existingRooms, RoomScript.RoomType.EXIT);
        existingRooms.Add(exitRoom);
    }

    void CreateCorridors()
    {
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
        if(rooms != null)
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
        for(int i = 0; i < existingRooms.Count; i++)
        {
            RoomScript currentRoom = existingRooms[i];

            for(int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xpos + j;
                
                for(int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.ypos + k;
                    switch(currentRoom.roomType)
                    {
                        case RoomScript.RoomType.SPAWN:
                            {
                                tiles[xCoord][yCoord] = TileType.ROOM;
                            }
                            break;
                        case RoomScript.RoomType.EXIT:
                            {
                                tiles[xCoord][yCoord] = TileType.WALL;
                            }
                            break;
                        case RoomScript.RoomType.HOSTAGE:
                            {
                                tiles[xCoord][yCoord] = TileType.OBJECTIVE_ROOM;
                            }
                            break;
                        case RoomScript.RoomType.ITEM:
                            {
                                tiles[xCoord][yCoord] = TileType.OBJECTIVE_ROOM;
                            }
                            break;
                        case RoomScript.RoomType.MISC:
                            {
                                tiles[xCoord][yCoord] = TileType.FLOOR;
                            }
                            break;
                    }
                    //left wall
                    tiles[currentRoom.xpos][yCoord] = TileType.WALL;
                    //right wall
                    tiles[currentRoom.xpos + currentRoom.roomWidth - 1][yCoord] = TileType.WALL;
                    //bottom wall
                    tiles[xCoord][currentRoom.ypos] = TileType.WALL;
                    //top wall
                    tiles[xCoord][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.WALL;
                }
            }
            IntRange randdoorXPos = new IntRange(currentRoom.xpos + 2, currentRoom.xpos + currentRoom.roomWidth - 2);
            IntRange randdoorYPos = new IntRange(currentRoom.ypos + 2, currentRoom.ypos + currentRoom.roomHeight - 2);

            int doorXpos = randdoorXPos.Random;
            int doorYPos = randdoorYPos.Random;

            switch(currentRoom.doorDirection)
            {
                case RoomScript.DoorDirection.NORTH:
                    {
                        tiles[doorXpos][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.DOOR;
                        tiles[doorXpos + 1][currentRoom.ypos + currentRoom.roomHeight - 1] = TileType.DOOR;
                    }
                    break;
                case RoomScript.DoorDirection.SOUTH:
                    {
                        tiles[doorXpos][currentRoom.ypos] = TileType.DOOR;
                        tiles[doorXpos + 1][currentRoom.ypos] = TileType.DOOR;
                    }
                    break;
                case RoomScript.DoorDirection.WEST:
                    {
                        //Door on Left Side
                        tiles[currentRoom.xpos][doorYPos] = TileType.DOOR;
                        tiles[currentRoom.xpos][doorYPos + 1] = TileType.DOOR;
                    }
                    break;
                case RoomScript.DoorDirection.EAST:
                    {
                        //Door on Right Side
                        tiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos] = TileType.DOOR;
                        tiles[currentRoom.xpos + currentRoom.roomWidth - 1][doorYPos + 1] = TileType.DOOR;
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
        if (room2CenterXVector < room1.xpos)
        {
            ResultantXVector = room2CenterXVector - room1.xpos;
            while (x != ResultantXVector)
            {
                int xCoord = room1.xpos + x;
                groundtiles[xCoord][room1CenterYVector] = TileType.VENT;
                groundtiles[room1.xpos][room1CenterYVector] = TileType.VENT_E;
                groundtiles[room2CenterXVector][room1CenterYVector] = TileType.VENT_E;
                x--;
            }
        }
        //Check if Otherroom is on right side of room1
        else if (room2CenterXVector > room1.xpos + room1.roomWidth - 2)
        {
            ResultantXVector = room2CenterXVector - (room1.xpos + room1.roomWidth - 2);
            while (x != ResultantXVector)
            {
                int xCoord = (room1.xpos + room1.roomWidth - 2) + x;
                groundtiles[xCoord][room1CenterYVector] = TileType.VENT;
                groundtiles[(room1.xpos + room1.roomWidth - 2)][room1CenterYVector] = TileType.VENT_E;
                groundtiles[room2CenterXVector][room1CenterYVector] = TileType.VENT_E;
                x++;
            }
        }

        //Check if Otherroom is on the bottom side of room1
        if(room2CenterYVector < room1.ypos)
        {
            ResultantYVector = (room2.ypos + room2.roomHeight - 2) - room1CenterYVector;
            while(y != ResultantYVector)
            {
                int yCoord = room1CenterYVector + y;
                groundtiles[room2CenterXVector][yCoord] = TileType.VENT;
                groundtiles[room2CenterXVector][(room2.ypos + room2.roomHeight - 2)] = TileType.VENT_E;
                groundtiles[room2CenterXVector][room1CenterYVector] = TileType.VENT_E;
                y--;
            }
        }
        //Check if Otherroom is on the top side of room1
        else if (room2CenterYVector > room1.ypos + room1.roomHeight - 1)
        {
            ResultantYVector = (room2.ypos + 2) - room1CenterYVector;
            while (y != ResultantYVector)
            {
                int yCoord = room1CenterYVector + y;
                groundtiles[room2CenterXVector][yCoord] = TileType.VENT;
                groundtiles[room2CenterXVector][(room2.ypos + 2)] = TileType.VENT_E;
                groundtiles[room2CenterXVector][room1CenterYVector] = TileType.VENT_E;
                y++;
            }
        }
    }

    void SetPlayerPosition()
    {
        int PlayerXPos = Random.Range(spawnRoom.xpos + 1, spawnRoom.xpos + spawnRoom.roomWidth - 1);
        int PlayerYPos = Random.Range(spawnRoom.ypos + 1, spawnRoom.ypos + spawnRoom.roomHeight - 1);

        Vector3 playerPos = new Vector3(tilespacing * PlayerXPos, tilespacing * PlayerYPos, -1);
        Instantiate(PlayerObject, playerPos, Quaternion.identity);
    }

    void InstantiateTiles()
    {
        for (int i = 0; i < groundtiles.Length; i++)
        {
            for (int j = 0; j < groundtiles[i].Length; j++)
            {
                switch (groundtiles[i][j])
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
        for(int i = 0; i < tiles.Length; i++)
        {
            for(int j = 0; j < tiles[i].Length; j++)
            {
                switch (tiles[i][j])
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
                        InstantiateFromArray(doorTile, tilespacing * i, tilespacing * j, 0f);
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

        tileInstance.transform.parent = LevelLayout.transform;

        return tileInstance;
    }

    // Gets the cost of a tile/grid based on the TileType
    public int GetGridCost(int x, int y)
    {
        switch(tiles[x][y])
        {
            case TileType.FLOOR: return 1;
            case TileType.WALL: return -1;

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

                if (checkPos.x < tempVec3.x + offset &&
                    checkPos.x > tempVec3.x - offset &&
                    checkPos.y < tempVec3.x + offset &&
                    checkPos.y > tempVec3.x - offset)
                {
                    return new Vector2(x, y);
                }
            }
        }

        return new Vector2(-1, -1);
    }
}
