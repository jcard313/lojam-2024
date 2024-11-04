using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class BoardManager : MonoBehaviour
{

   public class CellData
   {
      public bool Passable;
   }

    private CellData[,] m_BoardData;

    private Tilemap m_Tilemap;
    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public int MinRoomSize = 5;
    public int MaxRoomSize = 7;
    public int RoomCount = 8;
    private List<RectInt> rooms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_BoardData = new CellData[Width, Height];
        rooms = new List<RectInt>();

        InitializeBoard();
        GenerateRooms();
   }

    void InitializeBoard() {

        for (int y = 0; y < Height; ++y)
            {
                for(int x = 0; x < Width; ++x)
                {
                    Tile tile = WallTiles[Random.Range(0, WallTiles.Length)];  // fill the board with wall tiles (impassable)
                    m_BoardData[x, y] = new CellData { Passable = false };               
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
   }

    void GenerateRooms() {
    
        Debug.Log("Generating Rooms " + RoomCount);

        for (int i = 0; i < RoomCount; i++)
            {
                int roomWidth = Random.Range(MinRoomSize, MaxRoomSize);
                int roomHeight = Random.Range(MinRoomSize, MaxRoomSize);
                int roomXPos = Random.Range(1, Width - roomWidth - 1);
                int roomYPos = Random.Range(1, Width - roomHeight - 1);
                Debug.Log("Inside GenerateRooms for loop");

                RectInt newRoom = new RectInt(roomXPos, roomYPos, roomWidth, roomHeight);

                // Check if the new room overlaps with existing rooms
                bool overlaps = false;
                foreach (RectInt room in rooms)
                {
                    if (newRoom.Overlaps(room))
                    {
                        overlaps = true;
                        break;
                    }
                }
                if (overlaps) {
                    i--;
                    continue;
                }

                rooms.Add(newRoom);
                CreateRoom(roomXPos, roomYPos, roomWidth, roomHeight);

                // Connect to previous room if it's not the first one
                if (rooms.Count > 1)
                {
                    Vector2Int prevRoomCenter = GetRoomCenter(rooms[rooms.Count - 2]);
                    Vector2Int newRoomCenter = GetRoomCenter(newRoom);
                    CreateCorridor(prevRoomCenter, newRoomCenter);
                }

            }
   }

    void CreateRoom(int x, int y, int width, int height) {
        Debug.Log("Creating Room");

        for (int i = y; i < height+y; i++)
            {
                for(int j = x; j < width+x; j++)
                {
                    Tile tile = GroundTiles[Random.Range(0, GroundTiles.Length)];  // fill the room with ground tiles (walkabke)
                    m_BoardData[i, j].Passable = true;              
                    m_Tilemap.SetTile(new Vector3Int(i, j, 0), tile);
                }
            }
   }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        int currentX = start.x;
        int currentY = start.y;

        while (currentX != end.x)
        {
            Tile tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
            m_BoardData[currentX, currentY].Passable = true;
            m_Tilemap.SetTile(new Vector3Int(currentX, currentY, 0), tile);
            currentX += (end.x > start.x) ? 1 : -1;
        }

        while (currentY != end.y)
        {
            Tile tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
            m_BoardData[currentX, currentY].Passable = true;
            m_Tilemap.SetTile(new Vector3Int(currentX, currentY, 0), tile);
            currentY += (end.y > start.y) ? 1 : -1;
        }
    }

    Vector2Int GetRoomCenter(RectInt room)
    {
        int centerX = room.xMin + room.width / 2;
        int centerY = room.yMin + room.height / 2;
        return new Vector2Int(centerX, centerY);
    }
}
