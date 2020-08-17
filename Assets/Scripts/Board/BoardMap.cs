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

        //[SerializeField] private VisualBoard _visualBoard;

        private Cell[,] _map;
        private Node[,] _graph;

        public Cell[,] Map => _map;

        private void Awake()
        {
            GenerateMap();
            GeneratePathfindingGraph();
            LogMap();
            _boardsBridge.InitLogicBoard(this);
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
        }

        public List<Block> SearchColorLink(Block block)
        {
            List<Block> foundBlocks = new List<Block>();
            List<Block> toSearchIn = new List<Block>();

            var currentBlock = block;
            
            foundBlocks.Add(currentBlock);
            foreach (var node in _graph[currentBlock.Cell.BoardX, currentBlock.Cell.BoardY].Neighbours)
            {
                if (GetCell(node.Pos).IsEmpty)
                {
                    continue;
                }
                if (GetCell(node.Pos).Block.BlockParams.Color == currentBlock.BlockParams.Color
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
                    if (GetCell(node.Pos).Block.BlockParams.Color == currentBlock.BlockParams.Color
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
            //-1 because we want to shift all existing elements before the generation of new one
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

        private void SwapBlockFromTo(Cell fromCell, Cell toCell)
        {
            //TODO: Fix bug with> 2 empty cells in a row!!!
            var block = fromCell.Block;
            fromCell.UnbindBlock();
            toCell.SetBlock(block);
            _boardsBridge.SendVisualBoardAction(() => _boardsBridge.VisualBoard.SwapBlockFromTo
            (_boardsBridge.GetVisualCell(fromCell),
                _boardsBridge.GetVisualCell(toCell)));
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
                        _map[x,y].SetNewBlock(_toSpawn.GetRandomBlock());
                    }
                }
            }
        }
        

        // Start is called before the first frame update
        void Start()
        {
           // GenerateMap();
          //  GeneratePathfindingGraph();
            //_visualBoard.GenerateBoard(_map);
            
          //  LogMap();
          //  GeneratePathfindingGraph();
          //  RemoveBlocks(SearchColorLink(_map[0,0].Block));
           // GravitationSimulationShift();
           // LogMap();
            //GenerateBlocks();
            //LogMap();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

