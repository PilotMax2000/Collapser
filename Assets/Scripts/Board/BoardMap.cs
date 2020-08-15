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

        private Cell[,] _map;
        private Node[,] _graph;

        private void GenerateMap()
        {
            _map = new Cell[_sizeX,_sizeY];
            
            //TODO:remove after map functionality
            int mapLoaderCounter = 0;
                
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    _map[x,y] = new Cell(x,y);
                    _map[x,y].SetNewBlock(_mapToLoad[mapLoaderCounter]);
                    mapLoaderCounter++;
                }
            }
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
                    _graph[x,y] = new Node(x,y);
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

        //TODO: remove after adding proper taiing
        private void OnClick(BlockColor color, int x, int y)
        {
            
        }

        public List<Block> SearchColorLink(Block block)
        {
            List<Block> foundBlocks = new List<Block>();
            List<Block> toSearchIn = new List<Block>();

            var currentBlock = block;
            
            foundBlocks.Add(currentBlock);
            foreach (var node in _graph[currentBlock.Cell.X, currentBlock.Cell.Y].Neighbours)
            {
                if (_map[node.X, node.Y].Block.BlockParams.Color == currentBlock.BlockParams.Color
                    && toSearchIn.Contains(_map[node.X, node.Y].Block) == false)
                {
                    toSearchIn.Add(_map[node.X, node.Y].Block);
                }
            }

            while (toSearchIn.Count > 0)
            {
                currentBlock = toSearchIn[toSearchIn.Count - 1];
                foundBlocks.Add(currentBlock);
                foreach (var node in _graph[currentBlock.Cell.X, currentBlock.Cell.Y].Neighbours)
                {
                    if (_map[node.X, node.Y].Block.BlockParams.Color == currentBlock.BlockParams.Color
                        && toSearchIn.Contains(_map[node.X, node.Y].Block) == false
                        && foundBlocks.Contains(_map[node.X, node.Y].Block) == false)
                    {
                        toSearchIn.Add(_map[node.X, node.Y].Block);
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
            var block = fromCell.Block;
            fromCell.RemoveBlock();
            toCell.SetBlock(block);
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

        private void GenerateBlocks()
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
            GenerateMap();
            LogMap();
            GeneratePathfindingGraph();
            RemoveBlocks(SearchColorLink(_map[0,0].Block));
            GravitationSimulationShift();
            LogMap();
            GenerateBlocks();
            LogMap();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

