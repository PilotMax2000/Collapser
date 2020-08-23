using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public static class BoardSearch
    {
        public static Node[,] GeneratePathfindingGraph(LevelData levelData, Cell[,] map)
        {
            Node[,] graph = new Node[levelData.SizeX, levelData.SizeY];

            DoForEachCell((cell, x, y) => { graph[x, y] = new Node(new Vector2Int(x, y)); }, levelData, map);

            DoForEachCell((cell, x, y) =>
            {
                if (x > 0)
                    graph[x, y].Neighbours.Add(graph[x - 1, y]);
                if (x < levelData.SizeX - 1)
                    graph[x, y].Neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].Neighbours.Add(graph[x, y - 1]);
                if (y < levelData.SizeY - 1)
                    graph[x, y].Neighbours.Add(graph[x, y + 1]);
            }, levelData, map);

            Debug.Log($"Pathfinding graph was loaded successfully, size {levelData.SizeX}x{levelData.SizeY}");
            return graph;
        }

        public static Cell GetCell(int x, int y, LevelData levelData, Cell[,] map, bool showLogs = true)
        {
            if (x >= 0 && x < levelData.SizeX)
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

        public static void DoForEachCell(Action<Cell, int, int> actionForCell, LevelData levelData, Cell[,] map)
        {
            for (int x = 0; x < levelData.SizeX; x++)
            {
                for (int y = 0; y < levelData.SizeY; y++)
                {
                    actionForCell(GetCell(x, y, levelData, map), x, y);
                }
            }
        }

        public static Vector2Int GetPosDueToTargetOffset(Vector2Int boardPos, CellPos cellPos)
        {
            Vector2Int res = boardPos;
            Vector2Int addPos = Vector2Int.zero;

            switch (cellPos)
            {
                case CellPos.Center:
                    addPos = Vector2Int.zero;
                    break;
                case CellPos.Top:
                    addPos = Vector2Int.up;
                    break;
                case CellPos.Right:
                    addPos = Vector2Int.right;
                    break;
                case CellPos.Bottom:
                    addPos = Vector2Int.down;
                    break;
                case CellPos.Left:
                    addPos = Vector2Int.left;
                    break;
                case CellPos.TopLeft:
                    addPos = new Vector2Int(-1, 1);
                    break;
                case CellPos.TopRight:
                    addPos = new Vector2Int(1, 1);
                    break;
                case CellPos.BottomLeft:
                    addPos = new Vector2Int(-1, -1);
                    break;
                case CellPos.BottomRight:
                    addPos = new Vector2Int(1, -1);
                    break;
            }

            res += addPos;
            return res;
        }
        
        public static List<Block> GetTargetBlocksForRemoval(Cell cell,OnClickDestruction onDest, LevelData levelData, Cell[,] map)
        {
            List<Block> resBlocks = new List<Block>();
            if (onDest.TargetAllBoard)
            {
                DoForEachCell((c, x,y) =>
                {
                    BlockColor col = c.Block.BlockParams.Color;
                    if (col != null && col == onDest.OverrideTargetColor)
                    {
                        resBlocks.Add(c.Block);
                    }
                },levelData, map);
            }

            if (onDest.TargetBlocks == null || onDest.TargetBlocks.Count <= 0)
            {
                return resBlocks;
            }
            
            foreach (var cellPos in onDest.TargetBlocks)
            {
                var pos = GetPosDueToTargetOffset(cell.BoardPos, cellPos);
                var cellWithTarget = GetCell(pos.x, pos.y,
                    levelData, map, false);
                if (cellWithTarget != null && resBlocks.Contains(cellWithTarget.Block) == false)
                {
                    resBlocks.Add(cellWithTarget.Block);
                }
            }
            return resBlocks;
        }
    }
    public enum CellPos {Center, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft}
}


