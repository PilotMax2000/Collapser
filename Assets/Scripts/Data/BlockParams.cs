using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Collapser
{
    [CreateAssetMenu(fileName = "NewBlock", menuName = "BlockData/CreateBlock", order = 0)]
    public class BlockParams : ScriptableObject
    {
        public BlockColor Color;
    }
}