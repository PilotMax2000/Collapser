using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        
        [Header("Settings")] 
        [SerializeField] private BoolVariable _isBlockingInput;
        [SerializeField] private FloatVariable _globalAnimationDuration;

        private void Awake()
        {
            _boardsBridge.InitVisualBoard(this);
        }

        public void BlockPlayersInput(bool blockInput)
        {
            _isBlockingInput.Value = blockInput;
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
                    cell.Init(boardCells[x,y]);
                    var cellTransform = cell.transform;
                    cellTransform.position = new Vector3(x,y,0);
                    
                    VisualBlock block = Instantiate(_blockPref, cellTransform);
                    block.Init(boardCells[x,y].Block);
                    
                    cell.SetBlock(block, false);
                    //Create Block
                }
            }
            
            transform.localPosition = new Vector2(-(_sizeX-1)/2.0f, -(_sizeY-1)/2.0f);
            BlockPlayersInput(false);
            Debug.Log($"Visual map was generated, size{_sizeX}x{_sizeY}");
        }
        
        public void SetNewBlock(Vector2Int boardPos, Block block)
        {
            var visualCell = GetVisualCell(boardPos);
            VisualBlock visualBlock = Instantiate(_blockPref, visualCell.transform);
            visualBlock.Init(block);
            visualCell.SetBlock(visualBlock);
            visualBlock.transform.localScale = Vector3.zero;
            visualBlock.transform.DOScale(Vector3.one, _globalAnimationDuration.Value);
        }
        
        public VisualCell GetVisualCell(Vector2Int pos)
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
        
        public void SwapBlockFromTo(VisualCell from, VisualCell to)
        {
            var block = from.VisualBlock;
            from.UnbindBlock();
            to.SetBlock(block);
        }
    }

}

