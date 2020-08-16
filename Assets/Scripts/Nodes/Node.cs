using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public int PosX => _pos.x;
    public int PosY => _pos.y;

    public List<Node> Neighbours;

    public Node(Vector2Int pos )
    {
        Neighbours = new List<Node>();
        _pos = pos;
    }
}
