using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class Board : MonoBehaviour
    {
        [Header("Loading levels")] 
        [SerializeField] private LevelData _levelData;

        [Header("Bridge")] 
        [SerializeField] private BoardsBridge _boardsBridge;
        public Cell[,] Map { get; private set; }
        
        private Node[,] _graph;
        

        private void Awake()
        {
            _boardsBridge.InitLogicBoard(this);
        }

        public void Init()
        {
            GenerateLevelMap();
            GeneratePathfindingGraph();
            Debug.Log("Logic board was successfully generated.");
            LogMap();
        }
        
        private void Start()
        {
            _boardsBridge.GenerateVisualMap();
        }

        private void GenerateLevelMap()
        {
            Map = new Cell[_levelData.SizeX,_levelData.SizeY];
            int mapLoaderCounter = 0;
            
            for (int y = 0; y < _levelData.SizeY; y++)
            {
                for (int x = 0; x < _levelData.SizeX; x++)
                {
                    Map[x,y] = new Cell(new Vector2Int(x,y), _boardsBridge);
                    Map[x,y].SetNewBlock(_levelData.StartMapToLoad[mapLoaderCounter]);
                    mapLoaderCounter++;
                }
            }
            Debug.Log($"Map was generated successfully for logical board, size {_levelData.SizeX}x{_levelData.SizeY}");
        }
        

        public void OnClickReaction(Cell clickedCell)
        {
            var listToDestruction = GetBlocksForRemoval(clickedCell);
            if (listToDestruction == null || listToDestruction.Count <= 0)
            {
                return;
            }
            RemoveBlocks(listToDestruction);
            GravitationSimulationShift();
            LogMap();
            GenerateNewBlocks();
            LogMap();
            
            _boardsBridge.RunVisualActions();
        }

        private List<Block> GetBlocksForRemoval(Cell cell)
        {
            if (cell.Block == null || cell.Block.BlockParams == null)
            {
                Debug.LogError($"Cell {cell.BoardX},{cell.BoardY} has no block or block params!");
                return null;
            }
            
            var onDest = cell.Block.BlockParams.OnClickDestruction;

            if (onDest.LinkOtherBlocksByColor)
            {
                return GetColoredLinedBlocks(cell);
            }

            return BoardSearch.GetTargetBlocksForRemoval(cell, onDest, _levelData, Map);
        }

        private List<Block> GetColoredLinedBlocks(Cell cell)
        {
            var onDest = cell.Block.BlockParams.OnClickDestruction;
            if (onDest.TargetBlocks != null && onDest.TargetBlocks.Count <= 0)
            {
                Debug.Log("No target was selected for color linking");
                return null;
            }
            
            var target0 = GetCell(BoardSearch.GetPosDueToTargetOffset(cell.BoardPos, onDest.TargetBlocks[0]));
                
            if (target0 == null)
            {
                Debug.LogError($"Target0 was not fount on board, pos {cell.BoardPos}");
                return null;
            }
                
            if (target0.IsEmpty || target0.Block.BlockParams.Color == null)
            {
                Debug.LogError($"Target0 color was not fount, pos {target0.BoardX},{target0.BoardY}");
                return null;
            }
                
            //Currently for linking only use info of first element
            var linkedBlocks = onDest.OverrideTargetColor == null
                ? SearchColorLink(target0.Block) 
                : SearchColorLink(target0.Block, onDest.OverrideTargetColor);

            if (linkedBlocks != null && linkedBlocks.Count >= onDest.MinLinkingNumber)
            {
                return linkedBlocks;
            }
            Debug.Log("Not enough blocks for destruction. Will return null");
            return null;
        }

        public Cell GetCell(Vector2Int pos, bool showLogs = true)
        {
            return GetCell(pos.x, pos.y, showLogs);
        }

        private Cell GetCell(int x, int y, bool showLogs = true)
        {
            return BoardSearch.GetCell(x, y, _levelData, Map, showLogs);
        }

        private void LogMap()
        {
            Debug.Log("===========Reading Map============");
            DoForEachCell((cell, x, y) =>
            {
                string log = cell.IsEmpty ? $"[{x},{y}], Empty" : $"[{x},{y}] Color: {cell.Block.BlockParams.name}";
                Debug.Log(log);
            });
            Debug.Log("===========Finished Reading Map============");
        }

        
        private void GeneratePathfindingGraph() 
        {
            _graph = BoardSearch.GeneratePathfindingGraph(_levelData, Map);
        }

        private List<Block> SearchColorLink(Block block, BlockColor useOtherColor = null)
        {
            List<Block> foundBlocks = new List<Block>();
            List<Block> toSearchIn = new List<Block>();
            BlockColor currentColor = useOtherColor == null ? block.BlockParams.Color : useOtherColor;

            Block currentBlock = block;
            FindProperColorNeighbourBlocks(foundBlocks, currentBlock, currentColor, toSearchIn);

            while (toSearchIn.Count > 0)
            {
                currentBlock = toSearchIn[toSearchIn.Count - 1];
                FindProperColorNeighbourBlocks(foundBlocks, currentBlock, currentColor, toSearchIn);

                toSearchIn.Remove(currentBlock);
            }
            return foundBlocks;
        }

        private void FindProperColorNeighbourBlocks(List<Block> foundBlocks, Block currentBlock, BlockColor currentColor,
            List<Block> toSearchIn)
        {
            foundBlocks.Add(currentBlock);
            foreach (var node in _graph[currentBlock.Cell.BoardX, currentBlock.Cell.BoardY].Neighbours)
            {
                if (GetCell(node.Pos).IsEmpty)
                {
                    continue;
                }

                if (GetCell(node.Pos).Block.BlockParams.Color == currentColor
                    && toSearchIn.Contains(GetCell(node.Pos).Block) == false 
                    && foundBlocks.Contains(GetCell(node.Pos).Block) == false)
                {
                    toSearchIn.Add(GetCell(node.Pos).Block);
                }
            }
        }

        private static void RemoveBlocks(List<Block> blocksToRemove)
        {
            if (blocksToRemove == null || blocksToRemove.Count <= 0)
            {
                return;
            }
            foreach (var block in blocksToRemove)
            {
                block.Cell.RemoveBlock();
            }
        }

        private void GravitationSimulationShift()
        {
            for (int currentXIndex = 0; currentXIndex < _levelData.SizeX; currentXIndex++)
            {
                int lastFixedYIndex = -1;
                int currentYIndex = 0;
                while (currentYIndex < _levelData.SizeY - 1)
                {
                    currentYIndex = MoveFromTopToBottom(currentXIndex, currentYIndex, ref lastFixedYIndex);
                }
            }
        }

        private int MoveFromTopToBottom(int currentXIndex, int currentYIndex, ref int lastFixedYIndex)
        {
            if (GetCell(currentXIndex, currentYIndex).IsEmpty == false)
            {
                lastFixedYIndex = currentYIndex;
                currentYIndex++;
                return currentYIndex;
            }

            if (GetCell(currentXIndex, currentYIndex + 1).IsEmpty)
            {
                return ++currentYIndex;
            }
            
            bool previousAndLastFixedYAreDifferent = currentYIndex - 1 != lastFixedYIndex;
            int cellForSwappingTo = previousAndLastFixedYAreDifferent ? lastFixedYIndex + 1 : currentYIndex;
            SwapBlockFromTo(GetCell(currentXIndex, currentYIndex + 1), GetCell(currentXIndex, cellForSwappingTo));
            lastFixedYIndex = previousAndLastFixedYAreDifferent ? ++lastFixedYIndex : currentYIndex;

            return ++currentYIndex;
        }

        private void SwapBlockFromTo(Cell fromCell, Cell toCell)
        {
            var block = fromCell.Block;
            fromCell.UnbindBlock();
            toCell.SetBlock(block);
            _boardsBridge.VisualActionSwapBlocks(fromCell.BoardPos, toCell.BoardPos);
        }

        private void GenerateNewBlocks()
        {
            DoForEachCell((cell) =>
            {
                if (cell.IsEmpty == false)
                {
                    return;
                }
                cell.SetNewBlock(_levelData.BlocksToSpawn.GetRandomBlock(), false);
            });
        }

        private void DoForEachCell(Action<Cell> actionForCell)
        {
            DoForEachCell((cell, x, y) => actionForCell(cell));
        }
        
        private void DoForEachCell(Action<Cell,int,int> actionForCell)
        {
            BoardSearch.DoForEachCell(actionForCell, _levelData, Map);
        }
    }
}

