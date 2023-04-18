using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PathNode.cs
//Justin Logan
//A00066979
//04-18-2023


[System.Serializable]
public class PathNode
{
    public Vector2 position; // data
    public PathNode next;
    public PathNode prev;

    // constructor
    public PathNode(Vector2 position, PathNode next, PathNode prev)
    {
        this.position = position;
        this.next = next;
        this.prev = prev;
    }
}
