using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Collapser
{
    public class Cell
    {
        private Vector2Int _boardPos;
        private bool _isEmpty = true;
        private Block _block;
        public Block Block => _block;
        public bool IsEmpty => _isEmpty;
        public Vector2Int BoardPos => _boardPos;
        public int BoardX => _boardPos.x;
        public int BoardY => _boardPos.y;

        public Cell(Vector2Int boardPos)
        {
            _boardPos = boardPos;
            _isEmpty = true;
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
        }

        public void RemoveBlock()
        {
            _block = null;
            _isEmpty = true;
        }
        
    }

}
