using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Graph.cs
//Justin Logan
//A00066979
//04-18-2023


public class Graph : MonoBehaviour
{
    public List<GraphNode> nodes;
    public float radius;
    public LayerMask graphLayerMask;

    // Start is called before the first frame update
    void Awake()
    {
        BuildGrid();
    }

    private void BuildGrid()
    {
        // Add all child nodes to the nodes list
        foreach (Transform child in transform)
        {
            nodes.Add(child.GetComponent<GraphNode>());
        }

        // created all connections
        for (var i = 0; i < nodes.Count; i++)
        {
            for (var j = 0; j < nodes.Count; j++)
            {
                // ignore the node itself
                if (i != j)
                {
                    if (!Physics2D.Linecast(nodes[i].transform.position, nodes[j].transform.position, graphLayerMask))
                    {
                        if (Vector3.Distance(nodes[i].transform.position, nodes[j].transform.position) < radius)
                        {
                            nodes[i].neighbours.Add(nodes[j]);
                        }
                    }
                }
            }
        }
    }

    public GraphNode FindClosestNode(Vector3 position)
    {
        GraphNode closestNode = null;
        var minDistance = float.PositiveInfinity;
        foreach (var node in nodes)
        {
            var positionToNodeDistance = Vector3.Distance(node.transform.position, position);
            if ( positionToNodeDistance < minDistance)
            {
                minDistance = positionToNodeDistance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}
