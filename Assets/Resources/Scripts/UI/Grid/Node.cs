using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum NodeType
{
    Empty = 0,
    Terrain = 1,
    JumpBlock = 2,
    FallBlock = 3
}

[System.Serializable]
public class Node
{
    public PathfindingJumpMarker JumpMarker => CorrospondingGameObject.GetComponent<PathfindingJumpMarker>();

    public PathfindingFallMarker FallMarker => CorrospondingGameObject.GetComponent<PathfindingFallMarker>();

    public List<int> AdjecentNodes; //{ get; private set; }

    public GameObject CorrospondingGameObject; //{ get; private set; }

    public NodeType NodeType; //{ get; private set; }

    public Bounds CellBounds; //{ get; private set; }
    
    public int Index; //{ get; private set; }
    
    public int Row; // { get; private set; }

    public int Column; // { get; private set; }

    public Node(GameObject corrospondingGameobject, Bounds bounds, NodeType nodeType, int index, int row, int column)
    {
        AdjecentNodes = new List<int>();

        CorrospondingGameObject = corrospondingGameobject;

        CellBounds = bounds;

        NodeType = nodeType;

        Index = index;

        Row = row;

        Column = column;
    }
}