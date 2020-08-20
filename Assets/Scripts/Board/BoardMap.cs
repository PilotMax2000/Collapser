using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class BoardMap : MonoBehaviour
    {
        [SerializeField] private int _sizeX;
        [SerializeField] private int _sizeY;

        [SerializeField] private List<BlockParams> _mapToLoad = new List<BlockParams>(9);
        [SerializeField] private BlocksToSpawn _toSpawn;

        [Header("Bridge")] 
        [SerializeField] private BoardsBridge _boardsBridge;

        private Cell[,] _map;
        private Node[,] _graph;

        public Cell[,] Map => _map;

        private void Awake()
        {
            _boardsBridge.InitLogicBoard(this);
        }

        public void Init()
        {
            GenerateMap();
            GeneratePathfindingGraph();
            Debug.Log("Logic board was successfuly generated.");
            LogMap();
        }
        
        void Start()
        {
            _boardsBridge.GenerateVisualMap();
        }

        private void GenerateMap()
        {
            _map = new Cell[_sizeX,_sizeY];
            
            //TODO:remove after map functionality
            int mapLoaderCounter = 0;
                
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    _map[x,y] = new Cell(new Vector2Int(x,y), _boardsBridge);
                    _map[x,y].SetNewBlock(_mapToLoad[mapLoaderCounter]);
                    mapLoaderCounter++;
                }
            }
            Debug.Log($"Map was generated successfully for logical board, size {_sizeX}x{_sizeY}");
        }

        public void OnClickReaction(Cell clickedCell)
        {
            //TODO:OnClickDestructioin parse
            var listToDestruction = GetBlocksForDestruction(clickedCell);
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

        private List<Block> GetBlocksForDestruction(Cell cell)
        {
            if (cell.Block == null || cell.Block.BlockParams == null)
            {
                Debug.LogError($"Cell {cell.BoardX},{cell.BoardY} has no block or block params!");
                return null;
            }
            
            
            var onDest = cell.Block.BlockParams.OnClickDestruction;
            
            //Generate list of target blocks to targets

            if (onDest.LinkOtherBlocksByColor && onDest.TargetBlocks != null && onDest.TargetBlocks.Count > 0)
            {
                var target0 = GetCell(onDest.GetBoardPosDueToTargetOffset(cell.BoardPos, onDest.TargetBlocks[0]));
                
                if (target0 == null)
                {
                    Debug.LogError($"Target0 color was not fount on board, pos {target0.BoardX},{target0.BoardY}");
                    return null;
                }
                
                if (target0.Block == null || target0.Block.BlockParams == null ||
                    target0.Block.BlockParams.Color == null)
                {
                    Debug.LogError($"Target0 color was not fount, pos {target0.BoardX},{target0.BoardY}");
                    return null;
                }
                
                //Currently for linking only use info of first element
                var linkedBlocks = onDest.OverrideFirstTargetColor == null
                    ? SearchColorLink(target0.Block) 
                    : SearchColorLink(target0.Block, onDest.OverrideFirstTargetColor);

                if (linkedBlocks != null && linkedBlocks.Count >= onDest.MinLinkingNumber)
                {
                    return linkedBlocks;
                }
                Debug.Log("Not enought blocks for destruction. Will return null");
                return null;

            }
            
            List<Block> resBlocks = new List<Block>();
            if (onDest.TargetBlocks != null && onDest.TargetBlocks.Count > 0)
            {
                foreach (var cellPos in onDest.TargetBlocks)
                {
                    var cellWithTarget = GetCell(onDest.GetBoardPosDueToTargetOffset(cell.BoardPos, cellPos));
                    if (cellWithTarget != null)
                    {
                        resBlocks.Add(cellWithTarget.Block);
                    }
                }
                return resBlocks;
            }
            return null;
        }
        
        

        //TODO: separete in different functions 
        public void RemoveBlocksWithSameColor(Cell cell)
        {
            List<Block> resBlocks = SearchColorLink(cell.Block);
            if (resBlocks != null && resBlocks.Count >= 2)
            {
                RemoveBlocks(resBlocks);
                //TODO:Send events for destruction
                //TODO:Move to visual events
                //_visualBoard.UpdateBoard();
            }
            GravitationSimulationShift();
            LogMap();
            GenerateNewBlocks();
            LogMap();
            
        }

        public Cell GetCell(Vector2Int pos)
        {
            if(pos.x >= 0 && pos.x < _sizeX)
            {
                if (pos.y >= 0 && pos.y < _sizeY)
                {
                    return _map[pos.x, pos.y];
                }
            }
            Debug.LogError($"Cell for {pos} was not found in board map!");
            return null;
        }
        
        //TODO: remove, only for debug purpose
        private void LogMap()
        {
            Debug.Log("===========Reading Map============");
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    if (_map[x, y].IsEmpty)
                    {
                        Debug.Log($"[{x},{y}], Empty");
                    }
                    else
                    {
                        Debug.Log($"[{x},{y}] Color: {_map[x,y].Block.BlockParams.name}");
                    }
                   
                }
            }
            Debug.Log("===========Finished Reading Map============");
        }
        
        void GeneratePathfindingGraph() {
            // Initialize the array
            _graph = new Node[_sizeX,_sizeY];

            // Initialize a Node for each spot in the array
            for(int x=0; x < _sizeX; x++) {
                for(int y=0; y < _sizeY; y++) {
                    _graph[x,y] = new Node(new Vector2Int(x,y));
                }
            }

            // Now that all the nodes exist, calculate their neighbours
            for(int x=0; x < _sizeX; x++) {
                for(int y=0; y < _sizeY; y++) {

                    // This is the 4-way connection version:
				if(x > 0)
					_graph[x,y].Neighbours.Add( _graph[x-1, y] );
				if(x < _sizeX-1)
					_graph[x,y].Neighbours.Add( _graph[x+1, y] );
				if(y > 0)
					_graph[x,y].Neighbours.Add( _graph[x, y-1] );
				if(y < _sizeY-1)
					_graph[x,y].Neighbours.Add( _graph[x, y+1] );
                }
            }
            Debug.Log($"Pathfinding graph was loaded successfully, size {_sizeX}x{_sizeY}");
        }

        public List<Block> SearchColorLink(Block block, BlockColor useOtherColor = null)
        {
            List<Block> foundBlocks = new List<Block>();
            List<Block> toSearchIn = new List<Block>();
            BlockColor currentColor = useOtherColor == null ? block.BlockParams.Color : useOtherColor;

            var currentBlock = block;
            
            foundBlocks.Add(currentBlock);
            foreach (var node in _graph[currentBlock.Cell.BoardX, currentBlock.Cell.BoardY].Neighbours)
            {
                if (GetCell(node.Pos).IsEmpty)
                {
                    continue;
                }
                if (GetCell(node.Pos).Block.BlockParams.Color == currentColor
                    && toSearchIn.Contains(GetCell(node.Pos).Block) == false)
                {
                    toSearchIn.Add(GetCell(node.Pos).Block);
                }
            }

            while (toSearchIn.Count > 0)
            {
                currentBlock = toSearchIn[toSearchIn.Count - 1];
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

                toSearchIn.Remove(currentBlock);
            }
            Debug.Log(foundBlocks);
            return foundBlocks;
        }

        private void RemoveBlocks(List<Block> blocksToRemove)
        {
            if (blocksToRemove != null && blocksToRemove.Count > 0)
            {
                foreach (var block in blocksToRemove)
                {
                    block.Cell.RemoveBlock();
                }
            }
        }

        private void GravitationSimulationShift()
        {
            for (int x = 0; x < _sizeX; x++)
            {
                int lastFixedBlock = -1;
                int currentIndex = 0;
                while (currentIndex < _sizeY - 1)
                {
                    if (_map[x, currentIndex].IsEmpty == false)
                    {
                        lastFixedBlock = currentIndex;
                        currentIndex++;
                        continue;
                    }
                    
                    if (_map[x, currentIndex].IsEmpty && _map[x, currentIndex+1].IsEmpty == false)
                    {
                        if (currentIndex - 1 != lastFixedBlock)
                        {
                            SwapBlockFromTo(_map[x, currentIndex+1], _map[x, lastFixedBlock+1]);
                            lastFixedBlock++;
                        }
                        else
                        {
                            SwapBlockFromTo(_map[x, currentIndex+1], _map[x, currentIndex]);
                            lastFixedBlock = currentIndex;
                        }
                        currentIndex++;
                    }
                    else if (_map[x, currentIndex].IsEmpty && _map[x, currentIndex+1].IsEmpty)
                    {
                        currentIndex++;
                    }
                }
            }
        }

        private void SwapBlockFromTo(Cell fromCell, Cell toCell)
        {
            var block = fromCell.Block;
            fromCell.UnbindBlock();
            toCell.SetBlock(block);
            _boardsBridge.VisualActionSwapBlocks(fromCell.BoardPos, toCell.BoardPos);
        }

        private void DoForEachCell(Action<int,int> test)
        {
            for (int y = 0; y < _sizeY-1; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    //TODO:fix this - will work only if EVERY cell has block (no empty/dead cells on level)
                    if (_map[x, y].IsEmpty)
                    {
                        SwapBlockFromTo(_map[x, y+1], _map[x, y]);
                    }
                }
            }
        }

        private void GenerateNewBlocks()
        {
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    if (_map[x, y].IsEmpty)
                    {
                        _map[x,y].SetNewBlock(_toSpawn.GetRandomBlock(), false);
                    }
                }
            }
        }
    }
}

