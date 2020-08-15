using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    public class Block
    {
        private int _x;
        private int _y;
        private Cell _cell;
        private BlockParams _blockParams;
        public BlockParams BlockParams => _blockParams;
        public Cell Cell => _cell;

        public Block(BlockParams blockParams, Cell parentCell)
        {
            SetBlockParams(blockParams, parentCell);
        }

        public void SetBlockParams(BlockParams blockParams, Cell parentCell)
        {
            _blockParams = blockParams;
            _cell = parentCell;
        }

        private void OnRemove()
        {
            
        }
    }
}
