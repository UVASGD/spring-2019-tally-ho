using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState {open, closed}

public enum RoomType {none, normal, start, end}

public class Room {

    public DoorState up, down, left, right;

    public RoomType type = RoomType.none;

    public int x, y;

    public bool reached = false;

    public Room(RoomType _type = RoomType.none) {

        type = _type;
        up = DoorState.closed;
        down = DoorState.closed;
        left = DoorState.closed;
        right = DoorState.closed;
    }

    public Room(RoomType _type = RoomType.none, int _x = 0, int _y = 0)
    {

        type = _type;
        x = _x;
        y = _y;
        up = DoorState.closed;
        down = DoorState.closed;
        left = DoorState.closed;
        right = DoorState.closed;
    }

}
