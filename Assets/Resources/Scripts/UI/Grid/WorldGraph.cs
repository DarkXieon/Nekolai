using System.Linq;
using System.Text;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(WorldGraph))]
public class WorldGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUI.Button(EditorGUILayout.GetControlRect(), new GUIContent("Bake World Graph")))
        {
            WorldGraph graph = (WorldGraph)target;

            graph.CreateWorldGraph();

            Debug.Log("Finished Baking");

            EditorUtility.SetDirty(target);
        }
    }
}

#endif

public class WorldGraph : MonoBehaviour
{
    [System.Serializable]
    private class VisualizationSettings
    {
        public bool VisualizeGraph;
        public Color EmptySpaceColor;
        public Color NormalTerrainColor;
        public Color JumpMarkerColor;
        public Color FallMarkerColor; 
    }

    public static WorldGraph Singleton;

    [SerializeField] private VisualizationSettings visualizationSettings;

    public float CellWidth;
    public float CellHeight;
    public LayerMask TerrainMask;

    public int Rows;
    public int Collumns;
    [HideInInspector] public Node[] Nodes;
    
    private void Awake()
    {
        CreateWorldGraph();

        Singleton = this;
    }

    private void OnDrawGizmos()
    {
        if(visualizationSettings.VisualizeGraph)
        {
            var sortedTest = Nodes.OrderBy(node => (int)node.NodeType).ToArray();

            for(int i = 0; i < sortedTest.Length; i++)
            {
                switch (sortedTest[i].NodeType)
                {
                    case NodeType.Empty:
                        Gizmos.color = visualizationSettings.EmptySpaceColor;
                        break;
                    case NodeType.Terrain:
                        Gizmos.color = visualizationSettings.NormalTerrainColor;
                        break;
                    case NodeType.JumpBlock:
                        Gizmos.color = visualizationSettings.JumpMarkerColor;
                        break;
                    case NodeType.FallBlock:
                        Gizmos.color = visualizationSettings.FallMarkerColor;
                        break;
                }

                Gizmos.DrawWireCube(sortedTest[i].CellBounds.center, sortedTest[i].CellBounds.size);
            }
        }
    }

    public void CreateWorldGraph()
    {
        var worldObjects = FindObjectsOfType<BoxCollider2D>()
            .Where(renderer => renderer.tag == "Terrain")
            .ToArray();

        float leftBound = worldObjects.Min(renderer => renderer.transform.position.x);
        float rightBound = worldObjects.Max(renderer => renderer.transform.position.x);
        float lowerBound = worldObjects.Min(renderer => renderer.transform.position.y);
        float upperBound = worldObjects.Max(renderer => renderer.transform.position.y);

        Bounds worldBounds = new Bounds(new Vector3((leftBound + rightBound) / 2f, (upperBound + lowerBound) / 2f), new Vector3(rightBound - leftBound, upperBound - lowerBound));

        Rows = Mathf.FloorToInt(worldBounds.size.y / 3f) + 1;
        Collumns = Mathf.FloorToInt(worldBounds.size.x / 7f) + 1;
        int[,] worldGrid = new int[Rows, Collumns];
        Nodes = new Node[Collumns * Rows];

        PopulateGridAndNodes(worldGrid, worldBounds);
        PopulateAdjecentNodes(worldGrid);
    }

    public Node FindCreatedNodeAt(float x, float y)
    {
        return Nodes.SingleOrDefault(node => node.CellBounds.Contains(new Vector3(x, y)));
    }

    private void PopulateGridAndNodes(int[,] worldGrid, Bounds worldBounds)
    {
        for(int row = 0; row < Rows; row++)
        {
            float y = worldBounds.center.y + worldBounds.extents.y - row * CellHeight;

            for(int collumn = 0; collumn < Collumns; collumn++)
            {
                float x = worldBounds.center.x - worldBounds.extents.x + collumn * CellWidth;

                GameObject nodeGameObject;
                NodeType nodeType = GetNodeTypeAt(x, y, out nodeGameObject);
                Bounds nodeBounds = new Bounds(new Vector3(x, y), new Vector3(CellWidth, CellHeight));
                int nodeIndex = row * Collumns + collumn;
                Node node = new Node(nodeGameObject, nodeBounds, nodeType, nodeIndex, row, collumn);

                worldGrid[row, collumn] = (int)nodeType;
                Nodes[nodeIndex] = node;
            }
        }
    }

    private NodeType GetNodeTypeAt(float x, float y, out GameObject nodeGameObject)
    {
        Collider2D[] foundColliders = new Collider2D[3];
        int foundNum = Physics2D.OverlapPoint(new Vector2(x, y), new ContactFilter2D() { layerMask = TerrainMask, minDepth = -10f, maxDepth = 10f, useTriggers = true }, foundColliders);

        foundColliders = foundColliders
            .Where(collider => collider != null && collider.tag == "Terrain")
            .ToArray();
        foundNum = foundColliders.Length;

        NodeType nodeType = NodeType.Empty;

        if (foundNum > 0)
        {
            bool hasJumpMarker = foundColliders.Any(collider => collider.GetComponent<PathfindingJumpMarker>());
            bool hasFallMarker = foundColliders.Any(collider => collider.GetComponent<PathfindingFallMarker>());

            if (hasFallMarker && hasJumpMarker)
            {
                Debug.LogError(string.Format("You should not have both a jump and fall marker on the same spot. Please redo. At position ({0}, {1})", x, y));

                nodeGameObject = null;
            }
            else if (hasJumpMarker)
            {
                nodeType = NodeType.JumpBlock;

                nodeGameObject = foundColliders
                    .Select(collider => collider.gameObject)
                    .Single(gameObject => gameObject.GetComponent<PathfindingJumpMarker>() != null);
            }
            else if (hasFallMarker)
            {
                nodeType = NodeType.FallBlock;

                nodeGameObject = foundColliders
                    .Select(collider => collider.gameObject)
                    .Single(gameObject => gameObject.GetComponent<PathfindingFallMarker>() != null);
            }
            else
            {
                if(foundColliders.Length > 1)
                {
                    Debug.LogWarning(string.Format("Currently has more than one found collider here for some reason. There is overlapping at ({0}, {1})", x, y));
                }
                
                nodeType = NodeType.Terrain;

                nodeGameObject = foundColliders[0].gameObject;
            }
        }
        else
        {
            nodeGameObject = null;
        }

        return nodeType;
    }

    private void PopulateAdjecentNodes(int[,] worldGrid)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Collumns; column++)
            {
                var node = Nodes[row * Collumns + column];
                
                if (row > 0)
                {
                    var adjecent = Nodes[(row - 1) * Collumns + column];

                    if(DeterminePassability(adjecent))
                        node.AdjecentNodes.Add(adjecent.Index);
                }
                if (row < Rows - 1)
                {
                    var adjecent = Nodes[(row + 1) * Collumns + column];

                    if (DeterminePassability(adjecent))
                        node.AdjecentNodes.Add(adjecent.Index);
                }
                if (column > 0)
                {
                    var adjecent = Nodes[row * Collumns + column - 1];

                    if (DeterminePassability(adjecent))
                        node.AdjecentNodes.Add(adjecent.Index);
                }
                if (column < Collumns - 1)
                {
                    var adjecent = Nodes[row * Collumns + column + 1];

                    if (DeterminePassability(adjecent))
                        node.AdjecentNodes.Add(adjecent.Index);
                }

                if (node.NodeType == NodeType.JumpBlock)
                {
                    Vector3 connectingPosition = node.CorrospondingGameObject.GetComponent<PathfindingJumpMarker>().ConnectingPlatform.bounds.center;

                    var adjecent = FindCreatedNodeAt(connectingPosition.x, connectingPosition.y);

                    Debug.Assert(DeterminePassability(adjecent));

                    node.AdjecentNodes.Add(adjecent.Index);
                }
                else if(node.NodeType == NodeType.FallBlock)
                {
                    Vector3 connectingPosition = node.CorrospondingGameObject.GetComponent<PathfindingFallMarker>().ConnectingPlatform.bounds.center;

                    var adjecent = FindCreatedNodeAt(connectingPosition.x, connectingPosition.y);

                    Debug.Assert(DeterminePassability(adjecent));

                    node.AdjecentNodes.Add(adjecent.Index);
                }
            }
        }
    }

    private bool DeterminePassability(Node currentNode)
    {
        var nodeAbove1 = currentNode.Row > 0
            ? Nodes[currentNode.Index - Collumns]
            : null;
        var nodeAbove2 = currentNode.Row > 1
            ? Nodes[currentNode.Index - Collumns * 2]
            : null;
        var nodeAbove3 = currentNode.Row > 2
            ? Nodes[currentNode.Index - Collumns * 3]
            : null;

        bool passedFirstCheck = nodeAbove1 == null || nodeAbove1.NodeType == NodeType.Empty;
        bool passedSecondCheck = nodeAbove2 == null || nodeAbove2.NodeType == NodeType.Empty;
        bool passedThirdCheck = nodeAbove3 == null || nodeAbove3.NodeType == NodeType.Empty;

        return currentNode.NodeType != NodeType.Empty && passedFirstCheck && passedSecondCheck && passedThirdCheck;
    }
    
    private void DisplayWorldGrid(int[,] worldGrid)
    {
        var numberOfRows = worldGrid.GetLength(0);

        var numberOfColumns = worldGrid.GetLength(1);

        StringBuilder builder = new StringBuilder();

        for (int row = 0; row < numberOfRows; row++)
        {
            for (int column = 0; column < numberOfColumns; column++)
            {
                builder.Append(worldGrid[row, column] + " ");
            }

            builder.AppendLine();
        }

        Debug.Log(builder.ToString());
    }
}