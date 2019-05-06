using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int levelwidth, levelheight;
    public GameObject DefaultRoom;
    public List<GameObject> RoomPrefabs;
    public GameObject Player;
    public GameObject boss;
    public float roomWidth, roomHeight;
    public float randomRoomChance;

    private void Awake() {
        List<Room> rooms = GenerateLayout(levelwidth, levelheight, 0);
        roomWidth = DefaultRoom.transform.Find("Background").localScale.x - DefaultRoom.transform.Find("WallTopRight").localScale.y;
        roomHeight = DefaultRoom.transform.Find("Background").localScale.y - DefaultRoom.transform.Find("WallTopRight").localScale.y;
        PutRoomsInScene(rooms);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void printGrid(Room[,] grid) {
        string buffer = "";
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                buffer += "[" + (int)(grid[x, y].type) + "]";
            }
            buffer += "\n";
        }

        Debug.Log(buffer);
    }

    public List<Room> GenerateLayout(int width, int height, int minRooms) {

        Room[,] grid = new Room[width, height];
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j] = new Room(_x: i, _y: j);
            }
        }
        //add random rooms
        RandomRooms(grid, randomRoomChance);


        int startIndex = Random.Range(0, height);

        grid[0, startIndex].type = RoomType.start;

        int endIndex = Random.Range(0, height);


        //create shortest path for initial sequence of rooms
        int x = 0;
        int y = startIndex;
        while (x < width - 1) {
            //increment room position
            if (y == endIndex) {
                //if same level as end room, always go right
                x++;
                grid[x, y].type = RoomType.normal;
            } else if (y > endIndex) {
                //if below end room, randomly decide between up and right
                int coinflip = Random.Range(0, 3);
                if (coinflip != 0) {
                    x++;
                    grid[x, y].type = RoomType.normal;
                } else {
                    y--;
                    grid[x, y].type = RoomType.normal;
                }
            } else {
                //if above end room, randomly decide between down and right
                int coinflip = Random.Range(0, 3);
                if (coinflip != 0) {
                    x++;
                    grid[x, y].type = RoomType.normal;
                } else {
                    y++;
                    grid[x, y].type = RoomType.normal;
                }
            }
        }
        //if it went all the way to the right, move vertically until reaching end room
        while (y != endIndex) {
            if (y < endIndex) {
                y++;
                grid[x, y].type = RoomType.normal;
            } else {
                y--;
                grid[x, y].type = RoomType.normal;
            }
        }
        grid[width - 1, endIndex].type = RoomType.end;

        //BFS from start room
        List<Room> floorRooms = new List<Room>();//contains all rooms in the floor
                                                 //floorRooms.Add(grid[0, startIndex]);
        Queue<Room> q = new Queue<Room>();
        q.Enqueue(grid[0, startIndex]);
        while (q.Count > 0) {
            Room nextRoom = q.Dequeue();
            floorRooms.Add(nextRoom);
            foreach (Room r in GetNeighbors(nextRoom, grid)) {
                if (r.type != RoomType.none && !r.reached) {
                    r.reached = true;
                    q.Enqueue(r);
                }
            }
        }


        //open appropriate doors
        foreach (Room r in floorRooms) {
            if (r.x > 0 && grid[r.x - 1, r.y].type != RoomType.none) {
                r.left = DoorState.open;
                grid[r.x - 1, r.y].right = DoorState.open;
            }
            if (r.y > 0 && grid[r.x, r.y - 1].type != RoomType.none) {
                r.up = DoorState.open;
                grid[r.x, r.y - 1].down = DoorState.open;
            }
            if (r.x < grid.GetLength(0) - 1 && grid[r.x + 1, r.y].type != RoomType.none) {
                r.right = DoorState.open;
                grid[r.x + 1, r.y].left = DoorState.open;
            }
            if (r.y < grid.GetLength(1) - 1 && grid[r.x, r.y + 1].type != RoomType.none) {
                r.down = DoorState.open;
                grid[r.x, r.y + 1].up = DoorState.open;
            }
        }
        return floorRooms;
    }

    List<Room> GetNeighbors(Room r, Room[,] grid) {
        List<Room> result = new List<Room>();
        if (r.x > 0) {
            result.Add(grid[r.x - 1, r.y]);
        }
        if (r.y > 0) {
            result.Add(grid[r.x, r.y - 1]);
        }
        if (r.x < grid.GetLength(0) - 1) {
            result.Add(grid[r.x + 1, r.y]);
        }
        if (r.y < grid.GetLength(1) - 1) {
            result.Add(grid[r.x, r.y + 1]);
        }
        return result;
    }

    void PutRoomsInScene(List<Room> desiredRooms) {
        foreach (Room r in desiredRooms) {
            if (r.type != RoomType.none) {
                addRoomAtPosition(r.x, r.y, r);
            }
        }
    }

    //Adds Rooms and Doors
    void addRoomAtPosition(int i, int j, Room room) {
        Vector3 pos = new Vector3(transform.position.x + i * roomWidth, transform.position.y - j * roomHeight, transform.position.z);
        GameObject RoomToAdd = RoomPrefabs[Random.Range(0, RoomPrefabs.Count)];
        if (room.type == RoomType.start) {
            RoomToAdd = DefaultRoom;
            GameObject newPlayer = Instantiate(Player, pos, Quaternion.identity, null);
            newPlayer.name = "Cornelius";
        } else if (room.type == RoomType.end) {
            GameObject finalBoss = Instantiate(boss, pos, Quaternion.identity, null);
            finalBoss.name = "Level 99 Chicken";
        }
        GameObject newRoom = Instantiate(RoomToAdd, pos, Quaternion.identity, transform);
        if (room.up == DoorState.open) {
            Destroy(newRoom.transform.Find("WallTop").gameObject);
        } else {
            Destroy(newRoom.transform.Find("WallTopLeft").gameObject);
            Destroy(newRoom.transform.Find("WallTopRight").gameObject);
            Destroy(newRoom.transform.Find("ExitLadder").gameObject);
        }
        if (room.down == DoorState.open) {
            Destroy(newRoom.transform.Find("WallBottom").gameObject);
        } else {
            Destroy(newRoom.transform.Find("WallBottomLeft").gameObject);
            Destroy(newRoom.transform.Find("WallBottomRight").gameObject);
        }
        if (room.left == DoorState.open) {
            Destroy(newRoom.transform.Find("WallLeft").gameObject);
        } else {
            Destroy(newRoom.transform.Find("WallLeftTop").gameObject);
            Destroy(newRoom.transform.Find("WallLeftBottom").gameObject);
        }
        if (room.right == DoorState.open) {
            Destroy(newRoom.transform.Find("WallRight").gameObject);
        } else {
            Destroy(newRoom.transform.Find("WallRightTop").gameObject);
            Destroy(newRoom.transform.Find("WallRightBottom").gameObject);
        }
    }

    //Runs Before Generate Layout
    void RandomRooms(Room[,] grid, float chance) {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                float coinflip = Random.Range(0f, 1f);
                if (coinflip < chance) {
                    grid[i, j].type = RoomType.normal;
                }
            }
        }
    }
}
