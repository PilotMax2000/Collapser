using System;
using System.Collections;
using System.Collections.Generic;
using Collapser;
using UnityEngine;

public static class BoardSearch 
{
    public static Node[,] GeneratePathfindingGraph(LevelData levelData, Cell[,] map) 
    {
        Node[,] graph = new Node[levelData.SizeX,levelData.SizeY];

        DoForEachCell((cell, x, y) =>
        {
            graph[x,y] = new Node(new Vector2Int(x,y));
        }, levelData, map);
            
        DoForEachCell((cell, x, y) =>
        {
            if(x > 0)
                graph[x,y].Neighbours.Add( graph[x-1, y] );
            if(x < levelData.SizeX-1)
                graph[x,y].Neighbours.Add( graph[x+1, y] );
            if(y > 0)
                graph[x,y].Neighbours.Add( graph[x, y-1] );
            if(y < levelData.SizeY-1)
                graph[x,y].Neighbours.Add( graph[x, y+1] );
        }, levelData, map);
            
        Debug.Log($"Pathfinding graph was loaded successfully, size {levelData.SizeX}x{levelData.SizeY}");
        return graph;
    }
    
    public static Cell GetCell(int x, int y, LevelData levelData, Cell[,] map, bool showLogs = true)
    {
        if(x >= 0 && x < levelData.SizeX)
        {
            if (y >= 0 && y < levelData.SizeY)
            {
                return map[x, y];
            }
        }

        if (showLogs)
        {
            Debug.LogError($"Cell for {x},{y} was not found in board map!");
        }
        return null;
    }
    
    public static void DoForEachCell(Action<Cell,int,int> actionForCell, LevelData levelData, Cell[,] map)
    {
        for (int x = 0; x < levelData.SizeX; x++)
        {
            for (int y = 0; y < levelData.SizeY; y++)
            {
                actionForCell(GetCell(x,y,levelData,map),x,y);
            }
        }
    }
}
