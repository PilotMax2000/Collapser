using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "OnClickDestructionParams", menuName = "BlockData/OnClickDestr")]
    public class OnClickDestruction : ScriptableObject
    {
        public List<CellPos> TargetBlocks;
        public bool TargetAllBoard;
        public bool LinkOtherBlocksByColor;
        [Range(0,10)] public int MinLinkingNumber;
        public BlockColor OverrideTargetColor;
        
        public Vector2Int GetBoardPosDueToTargetOffset(Vector2Int boardPos, CellPos cellPos)
        {
            Vector2Int res = boardPos;
            Vector2Int addPos = Vector2Int.zero;
            
            switch (cellPos)
            {
                case CellPos.Center:
                    addPos = Vector2Int.zero;
                    break;
                case CellPos.Top:
                    addPos = Vector2Int.up;
                    break;
                case CellPos.Right:
                    addPos = Vector2Int.right;
                    break;
                case CellPos.Bottom:
                    addPos = Vector2Int.down;
                    break;
                case CellPos.Left:
                    addPos = Vector2Int.left;
                    break;
                case CellPos.TopLeft:
                    addPos = new Vector2Int(-1,1);
                    break;
                case CellPos.TopRight:
                    addPos = new Vector2Int(1,1);
                    break;
                case CellPos.BottomLeft:
                    addPos = new Vector2Int(-1,-1);
                    break;
                case CellPos.BottomRight:
                    addPos = new Vector2Int(1,-1);
                    break;
            }

            res += addPos;
            return res;
        }

    }
    

    public enum CellPos {Center, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft}
}
