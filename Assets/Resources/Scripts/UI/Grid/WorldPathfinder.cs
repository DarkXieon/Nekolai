using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class WorldPathfinder
{
    public bool SearchFinished { get; private set; }
    public Node NextNode { get { return path.Pop(); } }

    private int stepIterations;
    private int iterations;
    private int[] previousNodes;
    private Node startNode;
    private Node goalNode;
    private List<Node> reachable;
    private List<Node> explored;
    private List<Node> allNodes;
    public Stack<Node> path;

    public WorldPathfinder(int iterationsPerStep)
    {
        allNodes = WorldGraph.Singleton.Nodes.ToList();
        stepIterations = iterationsPerStep;

        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].Index = i;
        }
    }

    public bool StartSearch(Vector3 startAt, Vector3 endAt)
    {
        iterations = 0;
        SearchFinished = false;

        explored = new List<Node>();
        path = new Stack<Node>();

        previousNodes = new int[allNodes.Count];

        for (int i = 0; i < allNodes.Count; i++)
        {
            previousNodes[i] = -1;
        }

        startNode = FindTerrainClosestTo(startAt);
        goalNode = FindTerrainClosestTo(endAt);

        reachable = new List<Node>();
        reachable.Add(startNode);

        return startNode != null && goalNode != null;
    }

    public void SearchStep()
    {
        int endIterations = iterations + stepIterations;

        while(iterations < endIterations && !SearchFinished)
        {
            Step();
        }
    }

    private void Step()
    {
        if(path.Count == 0 && reachable.Count > 0)
        {
            Node chosenNode = ChooseNode();

            if (chosenNode != goalNode)
            {
                reachable.Remove(chosenNode);
                explored.Add(chosenNode);

                for(int i = 0; i < chosenNode.AdjecentNodes.Count; i++)
                {
                    AddAdjacent(chosenNode, allNodes[chosenNode.AdjecentNodes[i]]);
                }
            }
            else
            {
                while(chosenNode != null)
                {
                    path.Push(chosenNode);
                    
                    chosenNode = previousNodes[chosenNode.Index] >= 0
                        ? allNodes[previousNodes[chosenNode.Index]]
                        : null;
                }

                SearchFinished = true;
            }

            iterations++;
        }
        else if(reachable.Count == 0)
        {
            SearchFinished = true;
        }
        else
        {
            Debug.LogError("I think something went wrong here");
        }
    }

    private void AddAdjacent(Node current, Node adjacent)
    {
        if(!explored.Contains(adjacent) && !reachable.Contains(adjacent)) //can you go there or has it already been checked?
        {
            previousNodes[adjacent.Index] = current.Index;
            reachable.Add(adjacent);
        }
    }
    
    private Node ChooseNode()
    {
        //Node chosen = reachable
        //    .Select(node => new KeyValuePair<Node, float>(node, Mathf.Abs(Vector2.Distance(node.CellBounds.center, goalNode.CellBounds.center))))
        //    .OrderBy(pair => pair.Value)
        //    .First().Key;

        //return chosen;

        return reachable[0];
    }
    
    public Node FindTerrainClosestTo(Vector3 point)
    {
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int colliderCount = Physics2D.Raycast(point, Vector2.down, new ContactFilter2D(), hits, 20f);

        hits = hits
            .Where(hit => hit.collider != null && hit.collider.tag == "Terrain")
            .ToArray();
        colliderCount = hits.Length;

        if(colliderCount > 0)
        {
            Vector2 nodePosition = hits
                .OrderByDescending(hit => hit.collider.transform.position.y)
                .First().transform.position;

            Node topNode = allNodes.Single(node => node.CellBounds.Contains(nodePosition));
            
            return topNode;
        }

        return null;
    }
}