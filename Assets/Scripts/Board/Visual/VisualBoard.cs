using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class VisualBoard : MonoBehaviour
    {
        [SerializeField] private int _sizeX;
        [SerializeField] private int _sizeY;

        [Header("Prefabs")] 
        [SerializeField] private VisualCell _cellPref;
        [SerializeField] private VisualBlock _blockPref;

        private VisualCell[,] _visualCells;
        private Cell[,] _cells;
        
        [Header("Bridge")] 
        [SerializeField] private BoardsBridge _boardsBridge;

        private void Awake()
        {
            _boardsBridge.InitVisualBoard(this);
        }

        private void Start()
        {
            GenerateBoard(_boardsBridge.BoardMap.Map);
        }

        public void SetCells(Cell[,] cells)
        {
            _cells = cells;
        }

        public void GenerateBoard(Cell[,] boardCells)
        {
            _sizeX = boardCells.GetLength(0);
            _sizeY = boardCells.GetLength(1);
            _visualCells = new VisualCell[_sizeX,_sizeY];
            
            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    //Create Cell
                    VisualCell cell = Instantiate(_cellPref, transform);
                    _visualCells[x, y] = cell;
                    cell.Init(boardCells[x,y], _boardsBridge);
                    var cellTransform = cell.transform;
                    cellTransform.position = new Vector3(x,y,0);
                    
                    VisualBlock block = Instantiate(_blockPref, cellTransform);
                    block.Init(boardCells[x,y].Block);
                    
                    cell.SetBlock(block);
                    //Create Block
                }
            }
            
        }

        public void SetNewBlock(Cell cell)
        {
            VisualBlock block = Instantiate(_blockPref, _visualCells[cell.BoardX, cell.BoardY].transform);
            block.Init(cell.Block);
            _visualCells[cell.BoardX, cell.BoardY].SetBlock(block);
        }
        
        public VisualCell GetCell(Vector2Int pos)
        {
            if(pos.x >= 0 && pos.x < _sizeX)
            {
                if (pos.y >= 0 && pos.y < _sizeY)
                {
                    return _visualCells[pos.x, pos.y];
                }
            }
            Debug.LogError($"Cell for {pos} was not found in board map!");
            return null;
        }

        public void GravitationShift()
        {
            
        }
        
        //TODO: Animate this!!!
        public void SwapBlockFromTo(VisualCell from, VisualCell to)
        {
            var block = from.VisualBlock;
            from.UnbindBlock();
            to.SetBlock(block);
        }

        //TODO: Add to game event system
        public void UpdateBoard()
        {
        
        }

        private void CreateBlock()
        {
        
        }
    }

}

