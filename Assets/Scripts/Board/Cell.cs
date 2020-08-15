using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Collapser
{
    public class Cell
    {
        private int _x;
        private int _y;
        private bool _isEmpty = true;
        private Block _block;
        public Block Block => _block;
        public int X => _x;
        public int Y => _y;
        public bool IsEmpty => _isEmpty;

        public Cell(int xPos, int yPos)
        {
            _x = xPos;
            _y = yPos;
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
