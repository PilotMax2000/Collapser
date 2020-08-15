using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "BlockColorsDB", menuName = "BlockData/CreateColorsDB")]
    public class BlockColorsData : ScriptableObject
    {
        public List<BlockColor> Colors;
    }
}
