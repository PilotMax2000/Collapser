using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Collapser
{
    public class Cell
    {
        private Vector2Int _boardPos;
        private bool _isEmpty;
        private Block _block;
        private Action<Cell> _onClick;
        public Block Block => _block;
        public bool IsEmpty => _isEmpty;
        public Vector2Int BoardPos => _boardPos;
        public int BoardX => _boardPos.x;
        public int BoardY => _boardPos.y;
        private BoardsBridge _boardsBridge;

        public Cell(Vector2Int boardPos, BoardsBridge boardsBridge)
        {
            _boardPos = boardPos;
            _isEmpty = true;
            _boardsBridge = boardsBridge;
        }
        
        // public Cell(int xPos, int yPos, Block block)
        // {
        //     _x = xPos;
        //     _y = yPos;
        //     _block = block;
        // }

        public void SetBlock(Block block)
        {
            _block = block;
            _block.BindBlockToNewCell(this);
            if (_block != null)
            {
                _isEmpty = false;
            }
        }
        
        public void SetNewBlock(BlockParams blockParams)
        {
            _block = new Block(blockParams, this);
            if (_block != null)
            {
                _isEmpty = false;
            }

            if (_boardsBridge.VisualBoard == null)
            {
                return;
            }
            //_boardsBridge.SendVisualBoardAction(() => _boardsBridge.VisualBoard.SetNewBlock(this));
        }

        public void RemoveBlock()
        {
            _block = null;
            _isEmpty = true;
            _boardsBridge.SendVisualBoardAction(_boardsBridge.GetVisualCell(this).RemoveBlock);
        }

        public void UnbindBlock()
        {
            _block.BindBlockToNewCell(null);
            _block = null;
            _isEmpty = true;
        }
    }

}
