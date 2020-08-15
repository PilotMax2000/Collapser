using System.Collections.Generic;
using Collapser;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksToSpawn", menuName = "Levels/BlocksToSpawn")]
public class BlocksToSpawn : ScriptableObject
{
    public List<BlockParams> ToSpawn;

    public BlockParams GetRandomBlock()
    {
        return ToSpawn[Random.Range(0, ToSpawn.Count)];
    }
}
