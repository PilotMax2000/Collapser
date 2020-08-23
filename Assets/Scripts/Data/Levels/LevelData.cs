using System.Collections.Generic;
using UnityEngine;

namespace Collapser
{
    [CreateAssetMenu(fileName = "Level_0", menuName = "Levels/CreateNewLevel")]
    public class LevelData : ScriptableObject
    {
        public int SizeX;
        public int SizeY;
        public List<BlockParams> StartMapToLoad;
        public BlocksToSpawn BlocksToSpawn;
    }


}
