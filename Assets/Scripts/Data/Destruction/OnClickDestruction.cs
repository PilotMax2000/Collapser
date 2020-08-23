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
        [Range(0, 10)] public int MinLinkingNumber;
        public BlockColor OverrideTargetColor;
    }
}
