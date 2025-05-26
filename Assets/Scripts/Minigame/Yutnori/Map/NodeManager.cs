using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;
    private List<PointOfInterest> highlightedNodes = new List<PointOfInterest>();
    public IReadOnlyList<PointOfInterest> HighlightedNodes => highlightedNodes; // ���� ����Ʈ���� �б� �������� ����, �ܺ� �����

    // �б��� ������ ���� ��尡 ���� ���� ���� ������ List
    public List<PointOfInterest> FindPreviousNodes(PointOfInterest node)
    {
        return node.PreviousPointsOfInterest;
    }

    public void HighlightReachableNodes(PointOfInterest startNode, int distance, PlayerPiece piece)
    {
        ClearHighlights();

        if (distance == -1)
        {
            if (!piece.HasStarted())
            {
                // ��� �� ����: �������� ���̶���Ʈ
                var mapGen = FindObjectOfType<MapGenerator>();
                var startPoint = mapGen.getStartingPoint();
                HighlightNode(startPoint);
            }
            else
            {
                // ��� �� ����: ���� ���� ���̶���Ʈ
                var prevNodes = FindPreviousNodes(startNode);
                foreach (var node in prevNodes)
                    HighlightNode(node);
            }
            return;
        }

        // ����: �Ϲ� �̵�
        var reachableNodes = FindReachableNodes(startNode, distance);
        foreach (var node in reachableNodes)
            HighlightNode(node);
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

            // dead end ó��: �� �̻� �� ���� ����, ���� moveDistance�� �������� ������ ��
            if (current.node.NextPointsOfInterest == null || current.node.NextPointsOfInterest.Count == 0)
            {
                // dead end�� �����ϸ� ����� �߰�
                result.Add(current.node);
                continue;
            }

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

    // ��� ��忡�� ������ �������� ��θ� ��ȯ (�ִ� ���)
    // �� static? Ŭ���� ��ü���� �����Ǵ� ��ƿ��Ƽ ����̱� ����. �ν��Ͻ� ���̵� ȣ���� �� �ְ�, ���� ������ ����
    public static List<PointOfInterest> FindPath(PointOfInterest start, PointOfInterest end)
    {
        var queue = new Queue<List<PointOfInterest>>();
        var visited = new HashSet<PointOfInterest>();
        queue.Enqueue(new List<PointOfInterest> { start });
        visited.Add(start);

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var last = path.Last();
            if (last == end)
                return path;

            foreach (var next in last.NextPointsOfInterest)
            {
                if (!visited.Contains(next))
                {
                    var newPath = new List<PointOfInterest>(path) { next };
                    queue.Enqueue(newPath);
                    visited.Add(next);
                }
            }
        }
        return null; // ��� ����
    }


    private void HighlightNode(PointOfInterest node)
    {
        Highlighter highlighter = node.GetComponentInChildren<Highlighter>();
        if (highlighter != null)
        {
            highlighter.StartBlink();
            highlightedNodes.Add(node);
            gameManager.CurrentPlayer.canMove = true; // ��� Ŭ�� �� �� �������� ���� �����ϴ� bool
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
        gameManager.CurrentPlayer.canMove = false;
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
