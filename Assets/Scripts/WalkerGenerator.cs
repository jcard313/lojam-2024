using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY
    }

    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public int MapWidth = 30;
    public int MapHeight = 30;

    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];
        
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
                {
                    gridHandler[x, y] = Grid.EMPTY;
                }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject currWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        tileMap.SetTile(TileCenter, GroundTiles[UnityEngine.Random.Range(0, GroundTiles.Length)]);
        Walkers.Add(currWalker);

        TileCount++;

        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject currwalkter in Walkers)
            {
                Vector3Int currPos = new Vector3Int((int)currwalkter.Position.x, (int)currwalkter.Position.y, 0);

                if (gridHandler[currPos.x, currPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(currPos,  GroundTiles[UnityEngine.Random.Range(0, GroundTiles.Length)]);
                    TileCount++;
                    gridHandler[currPos.x, currPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }

            }

            // Walker Methods

            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        StartCoroutine(CreateWalls());
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;

        for (int i = 0; i< updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }
    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;

        for (int i = 0; i< updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }
    void UpdatePosition()
    {
        for (int i = 0; i< Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                WalkerObject FoundWalker = Walkers[i];
                FoundWalker.Position += FoundWalker.Direction;
                FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0)-2);
                FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1)-2);
                Walkers[i] = FoundWalker;
            }
        }
    }
    void ChanceToRedirect()
    {
        for (int i = 0; i< Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject currWalker = Walkers[i];
                currWalker.Direction = GetDirection();
                Walkers[i] = currWalker;
            }
        }
    }

    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++) 
        {
             for (int y = 0; y < gridHandler.GetLength (1) - 1; y++)
             {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedwall = false;
                    
                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        tileMap. SetTile(new Vector3Int(x + 1, y, 0), WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)]);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedwall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        tileMap. SetTile(new Vector3Int(x - 1, y, 0), WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)]);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedwall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        tileMap. SetTile(new Vector3Int(x, y + 1, 0), WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)]);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedwall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        tileMap. SetTile(new Vector3Int(x, y - 1, 0), WallTiles[UnityEngine.Random.Range(0, WallTiles.Length)]);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedwall = true;
                    }

                    if (hasCreatedwall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }

                }
              

                }
             }

        }
       
}