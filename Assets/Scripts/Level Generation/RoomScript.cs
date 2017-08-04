using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript{

    public enum RoomType
    {
        SPAWN = 0,
        EXIT,
        OBJECTIVES,
        MISC,
        POWER,
        SECURITYCONSOLE,
        MAX_ROOMS,
    };

    public RoomType roomType;

    public enum DoorDirection
    {
        NORTH = 0,
        SOUTH,
        EAST,
        WEST,
        MAX_DIRECTIONS,
    };

    public DoorDirection doorDirection;
    public int doorType;

    public int xpos;
    public int ypos;
    public int roomWidth;
    public int roomHeight;

    public bool isIntersecting = true;

    public void Init(IntRange widthRange, IntRange heightRange, int columns, int rows, RoomType type, DoorDirection direction)
    {
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        xpos = Random.Range(1, Mathf.RoundToInt(columns - roomWidth) - 1);
        ypos = Random.Range(1, Mathf.RoundToInt(rows - roomHeight) - 1);

        roomType = type;

        doorDirection = direction;
    }

    public bool RoomIntersecting(RoomScript otherRoom)
    {
        int xpos2 = xpos + roomWidth;
        int ypos2 = ypos + roomHeight;

        int OtherRoomXpos2 = otherRoom.xpos + otherRoom.roomWidth;
        int OtherRoomYpos2 = otherRoom.ypos + otherRoom.roomHeight;

        if (xpos <= OtherRoomXpos2 && xpos2 >= otherRoom.xpos &&
            ypos <= OtherRoomYpos2 && ypos2 >= otherRoom.ypos)
        {
            isIntersecting = true;
        }
        else
            isIntersecting = false;

        return isIntersecting;
    }

	
}
