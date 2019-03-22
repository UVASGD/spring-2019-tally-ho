using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int levelwidth, levelheight;
    public GameObject RoomPrefab;

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < 10; i++) {
            printGrid(GenerateLayout(levelwidth, levelheight, 0));
        }
        print("width: " + RoomPrefab.transform.Find("Background").localScale.x);
        print("cell width: " + (RoomPrefab.transform.Find("Background").localScale.x - RoomPrefab.transform.Find("WallTopRight").localScale.y));
        print("cell height: " + (RoomPrefab.transform.Find("Background").localScale.y - RoomPrefab.transform.Find("WallTopRight").localScale.y));
    }

    // Update is called once per frame
    void Update() {
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

    public Room[,] GenerateLayout(int width, int height, int minRooms) {

        Room[,] grid = new Room[width, height];
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j] = new Room();
            }
        }
        List<Room> startingPathRooms = new List<Room>();//contains all rooms in the initial path including start and end

        int startIndex = Random.Range(0, height);

        grid[0, startIndex] = new Room(_type: RoomType.start);
        startingPathRooms.Add(grid[0, startIndex]);

        int endIndex = Random.Range(0, height);

        grid[width - 1, endIndex] = new Room(_type: RoomType.end);

        //create shortest path for initial sequence of rooms
        int x = 0;
        int y = startIndex;
        while (x < width - 1) {
            //increment room position
            if (y == endIndex) {
                //if same level as end room, always go right
                x++;
                grid[x - 1, y].right = DoorState.open;
                grid[x, y] = new Room(_type: RoomType.normal, _left: DoorState.open);
                startingPathRooms.Add(grid[x, y]);
            } else if (y > endIndex) {
                //if below end room, randomly decide between up and right
                int coinflip = Random.Range(0, 3);
                if (coinflip != 0) {
                    x++;
                    grid[x - 1, y].right = DoorState.open;
                    grid[x, y] = new Room(_type: RoomType.normal, _left: DoorState.open);
                    startingPathRooms.Add(grid[x, y]);
                } else {
                    y--;
                    grid[x, y + 1].up = DoorState.open;
                    grid[x, y] = new Room(_type: RoomType.normal, _down: DoorState.open);
                    startingPathRooms.Add(grid[x, y]);
                }
            } else {
                //if above end room, randomly decide between down and right
                int coinflip = Random.Range(0, 3);
                if (coinflip != 0) {
                    x++;
                    grid[x - 1, y].right = DoorState.open;
                    grid[x, y] = new Room(_type: RoomType.normal, _left: DoorState.open);
                    startingPathRooms.Add(grid[x, y]);
                } else {
                    y++;
                    grid[x, y - 1].down = DoorState.open;
                    grid[x, y] = new Room(_type: RoomType.normal, _up: DoorState.open);
                    startingPathRooms.Add(grid[x, y]);
                }
            }
        }
        //if it went all the way to the right, move vertically until reaching end room
        while (y != endIndex) {
            if (y < endIndex) {
                y++;
                grid[x, y - 1].down = DoorState.open;
                grid[x, y] = new Room(_type: RoomType.normal, _up: DoorState.open);
                startingPathRooms.Add(grid[x, y]);
            } else {
                y--;
                grid[x, y + 1].up = DoorState.open;
                grid[x, y] = new Room(_type: RoomType.normal, _down: DoorState.open);
                startingPathRooms.Add(grid[x, y]);
            }
        }
        grid[width - 1, endIndex].type = RoomType.end;

        //add random rooms


        return grid;
    }

    void putRoomsInScene(Room[,] grid) {
        float roomWidth = RoomPrefab.transform.Find("Background").localScale.x - RoomPrefab.transform.Find("WallTopRight").localScale.y;
        float roomHeight = RoomPrefab.transform.Find("Background").localScale.y - RoomPrefab.transform.Find("WallTopRight").localScale.y;

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                switch (grid[i, j].type) {
                    case RoomType.none:

                        break;
                    case RoomType.normal:
                        Vector3 pos = new Vector3(transform.position.x + i * roomWidth, transform.position.y + j * roomHeight, transform.position.z);
                        GameObject newRoom = Instantiate(RoomPrefab, pos, Quaternion.identity, transform);
                        break;
                    case RoomType.start:
                        pos = new Vector3(transform.position.x + i * roomWidth, transform.position.y + j * roomHeight, transform.position.z);
                        newRoom = Instantiate(RoomPrefab, pos, Quaternion.identity, transform);
                        break;
                    case RoomType.end:
                        pos = new Vector3(transform.position.x + i * roomWidth, transform.position.y + j * roomHeight, transform.position.z);
                        newRoom = Instantiate(RoomPrefab, pos, Quaternion.identity, transform);
                        break;
                }
            }
        }
    }


}
