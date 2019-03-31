using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState {open, undecided, closed}

public enum RoomType {none, normal, start, end}

public class Room {

    public DoorState up, down, left, right;

    public RoomType type = RoomType.none;

    public Room(RoomType _type = RoomType.none, DoorState _up = DoorState.undecided,
                DoorState _down = DoorState.undecided, DoorState _left = DoorState.undecided,
                DoorState _right = DoorState.undecided) {

        type = _type;
        up = _up;
        down = _down;
        left = _left;
        right = _right;
    }

}
