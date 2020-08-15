using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int _x;
    private int _y;
    public int X => _x;
    public int Y => _y;

    public List<Node> Neighbours;

    public Node(int x, int y)
    {
        Neighbours = new List<Node>();
        _x = x;
        _y = y;
    }
}
