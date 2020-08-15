using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "BlockColor", menuName = "BlockData/CreateColor", order = 1)]
    public class BlockColor : ScriptableObject
    {
        public Color Color;
    }
}
