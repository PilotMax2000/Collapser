using System;
using UnityEngine;

namespace Collapser
{
    public class Cell
    {
        public Block Block { get; private set; }
        public bool IsEmpty { get; private set; }
        public Vector2Int BoardPos => _boardPos;
        public int BoardX => _boardPos.x;
        public int BoardY => _boardPos.y;
        
        private readonly BoardsBridge _boardsBridge;
        private Vector2Int _boardPos;
        private Action<Cell> _onClick;

        public Cell(Vector2Int boardPos, BoardsBridge boardsBridge)
        {
            _boardPos = boardPos;
            IsEmpty = true;
            _boardsBridge = boardsBridge;
        }

        public void SetBlock(Block block)
        {
            Block = block;
            Block.BindBlockToNewCell(this);
            if (Block != null)
            {
                IsEmpty = false;
            }
        }
        
        public void SetNewBlock(BlockParams blockParams, bool onlyLogicSetup = true)
        {
            Block = new Block(blockParams, this);
            if (Block != null)
            {
                IsEmpty = false;
            }

            if (onlyLogicSetup)
            {
                return;
            }
            _boardsBridge.VisualActionSetNewBlock(_boardPos, Block);
        }

        public void RemoveBlock()
        {
            Block = null;
            IsEmpty = true;
            
            _boardsBridge.VisualActionRemoveBlock(_boardPos);
        }

        public void UnbindBlock()
        {
            Block.BindBlockToNewCell(null);
            Block = null;
            IsEmpty = true;
        }
    }

}
