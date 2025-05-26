using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;
    private List<PointOfInterest> highlightedNodes = new List<PointOfInterest>();
    public IReadOnlyList<PointOfInterest> HighlightedNodes => highlightedNodes; // 같은 리스트지만 읽기 전용으로 제공, 외부 참고용

    // 분기점 때문에 직전 노드가 여러 개일 수도 있으니 List
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
                // 출발 전 빽도: 시작점만 하이라이트
                var mapGen = FindObjectOfType<MapGenerator>();
                var startPoint = mapGen.getStartingPoint();
                HighlightNode(startPoint);
            }
            else
            {
                // 출발 후 빽도: 이전 노드들 하이라이트
                var prevNodes = FindPreviousNodes(startNode);
                foreach (var node in prevNodes)
                    HighlightNode(node);
            }
            return;
        }

        // 기존: 일반 이동
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

            // dead end 처리: 더 이상 갈 곳이 없고, 아직 moveDistance에 도달하지 못했을 때
            if (current.node.NextPointsOfInterest == null || current.node.NextPointsOfInterest.Count == 0)
            {
                // dead end에 도달하면 결과에 추가
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

    // 출발 노드에서 목적지 노드까지의 경로를 반환 (최단 경로)
    // 왜 static? 클래스 전체에서 공유되는 유틸리티 기능이기 때문. 인스턴스 없이도 호출할 수 있고, 여러 곳에서 재사용
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
        return null; // 경로 없음
    }


    private void HighlightNode(PointOfInterest node)
    {
        Highlighter highlighter = node.GetComponentInChildren<Highlighter>();
        if (highlighter != null)
        {
            highlighter.StartBlink();
            highlightedNodes.Add(node);
            gameManager.CurrentPlayer.canMove = true; // 노드 클릭 시 말 움직일지 말지 제어하는 bool
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
