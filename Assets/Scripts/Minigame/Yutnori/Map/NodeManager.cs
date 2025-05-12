using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;
    private List<PointOfInterest> highlightedNodes = new List<PointOfInterest>();

    public void HighlightReachableNodes(PointOfInterest startNode, int distance)
    {
        ClearHighlights();
        var reachableNodes = FindReachableNodes(startNode, distance);
        foreach (var node in reachableNodes) HighlightNode(node);
    }

    private List<PointOfInterest> FindReachableNodes(PointOfInterest start, int distance)
    {
        List<PointOfInterest> result = new List<PointOfInterest>();
        Queue<NodeWithDistance> queue = new Queue<NodeWithDistance>();
        queue.Enqueue(new NodeWithDistance(start, 0));
        HashSet<PointOfInterest> visited = new HashSet<PointOfInterest> { start };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.distance == distance)
            {
                result.Add(current.node);
                continue;
            }

            foreach (var next in current.node.NextPointsOfInterest.Where(n => !visited.Contains(n)))
            {
                visited.Add(next);
                queue.Enqueue(new NodeWithDistance(next, current.distance + 1));
            }
        }
        return result;
    }

    private void HighlightNode(PointOfInterest node)
    {
        Highlighter highlighter = node.GetComponentInChildren<Highlighter>();
        if (highlighter != null)
        {
            highlighter.StartBlink();
            highlightedNodes.Add(node);
            Debug.Log("Working");
        }
    }

    public void ClearHighlights()
    {
        foreach (var node in highlightedNodes)
        {
            Highlighter highlighter = node.GetComponentInChildren<Highlighter>();
            if (highlighter != null) highlighter.StopBlink();
        }
        highlightedNodes.Clear();
    }

    void OnMouseDown()
    {
        if (gameManager.stage == GameStage.Move)
        {
            PointOfInterest node = GetComponent<PointOfInterest>();
            gameManager.MoveSelectedPieceTo(node);
        }
    }

    private class NodeWithDistance
    {
        public PointOfInterest node;
        public int distance;
        public NodeWithDistance(PointOfInterest node, int distance)
        {
            this.node = node;
            this.distance = distance;
        }
    }
}
