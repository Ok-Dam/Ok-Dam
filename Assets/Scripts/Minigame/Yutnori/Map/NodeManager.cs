using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    [Header("노드 부모 (Nodes 오브젝트)")]
    [SerializeField] private Transform nodesParent; // Inspector에서 Nodes(Empty) 할당

    [SerializeField] private Canvas uiCanvas; // Inspector에서 할당
    [SerializeField] private Camera mainCamera; //

    [Header("노드 하이라이트 프리팹")]
    [SerializeField] private GameObject nodeHighlightPrefab; // Inspector에서 할당

    private YutnoriGameManager gameManager;

    private List<PointOfInterest> boardNodes; // 29개 노드, nodeNumber 순서대로

    private List<GameObject> spawnedHighlights = new List<GameObject>(); // 동적으로 생성된 하이라이트 오브젝트
    private List<PointOfInterest> highlightedNodes = new List<PointOfInterest>();
    public IReadOnlyList<PointOfInterest> HighlightedNodes => highlightedNodes;

    void Awake()
    {
        // 자식 노드 자동 수집 및 nodeNumber 순 정렬
        boardNodes = new List<PointOfInterest>();
        foreach (Transform child in nodesParent)
        {
            var poi = child.GetComponent<PointOfInterest>();
            if (poi != null)
                boardNodes.Add(poi);
        }
        boardNodes = boardNodes.OrderBy(n => n.nodeNumber).ToList();
    }

    private void Start()
    {
        gameManager = GetComponent<YutnoriGameManager>();
    }

    // 직전 노드(분기점 등) 반환
    public List<PointOfInterest> FindPreviousNodes(PointOfInterest node)
    {
        return node.PreviousPointsOfInterest;
    }

    /// <summary>
    /// 이동 가능 노드 하이라이트 (분기점 포함, 순환/고정 경로)
    /// </summary>
    public void HighlightReachableNodes(PointOfInterest startNode, int distance, PlayerPiece piece)
    {
        ClearHighlights();

        // 빽도(-1) 처리: 출발 전이면 29번(출발점), 출발 후면 PreviousPointsOfInterest
        if (distance == -1)
        {
            if (!piece.HasStarted())
            {
                var startPoint = boardNodes.FirstOrDefault(n => n.nodeNumber == 29);
                if (startPoint != null)
                    HighlightNode(startPoint, piece);
                else
                    Debug.Log("HighlightReachableNodes - Baekdo: startPoint not found!");
            }
            else
            {
                var prevNodes = FindPreviousNodes(startNode);
                foreach (var node in prevNodes)
                    HighlightNode(node, piece);
            }
            return;
        }

        // 일반 이동: 분기점 포함, 여러 경로 모두 탐색
        var reachableNodes = FindReachableNodes(startNode, distance);
        foreach (var node in reachableNodes)
            HighlightNode(node, piece);
    }


    private List<PointOfInterest> FindReachableNodes(PointOfInterest start, int distance)
    {
        List<PointOfInterest> result = new();
        Queue<(PointOfInterest node, int depth)> queue = new();
        HashSet<(int, int)> visited = new();

        // 1. 시작점이 junction이면 shortcutTarget만 enqueue
        if (start.isJunction && start.shortcutTarget != null)
        {
            queue.Enqueue((start.shortcutTarget, 1));
            visited.Add((start.shortcutTarget.nodeNumber, 1));
        }
        else
        {
            foreach (var next in start.NextPointsOfInterest)
            {
                queue.Enqueue((next, 1));
                visited.Add((next.nodeNumber, 1));
            }
        }

        // 2. 이후는 무조건 NextPointsOfInterest만 탐색
        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();

            // 도착점(29)에 도달하면 무조건 멈춤
            if (current.nodeNumber == 29 && (current != start || depth > 0))
            {
                if (depth == distance)
                    result.Add(current);
                continue;
            }

            if (depth == distance)
            {
                result.Add(current);
                continue;
            }

            foreach (var next in current.NextPointsOfInterest)
            {
                if (!visited.Contains((next.nodeNumber, depth + 1)))
                {
                    queue.Enqueue((next, depth + 1));
                    visited.Add((next.nodeNumber, depth + 1));
                }
            }
        }
        return result;
    }

    public static List<PointOfInterest> FindPath(PointOfInterest start, PointOfInterest end)
    {
        var queue = new Queue<List<PointOfInterest>>();
        var visited = new HashSet<PointOfInterest>();

        // 1. 시작점이 junction이면 shortcutTarget만 enqueue
        if (start.isJunction && start.shortcutTarget != null)
        {
            var path = new List<PointOfInterest> { start, start.shortcutTarget };
            queue.Enqueue(path);
            visited.Add(start);
            visited.Add(start.shortcutTarget);
        }
        else
        {
            queue.Enqueue(new List<PointOfInterest> { start });
            visited.Add(start);
        }

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var current = path.Last();

            // 도착
            if (current == end)
                return path;

            // 도착점(29)에 도달하면 멈춤 (단, 출발점에서 바로 멈추면 안 됨)
            if (current.nodeNumber == 29 && (current != start || path.Count > 1))
                continue;

            // 이후는 무조건 NextPointsOfInterest만 탐색
            foreach (var next in current.NextPointsOfInterest)
            {
                if (!visited.Contains(next))
                {
                    var newPath = new List<PointOfInterest>(path) { next };
                    queue.Enqueue(newPath);
                    visited.Add(next);
                }
            }
        }
        Debug.Log("FindPath - 경로 없음");
        return null;
    }



    public void HighlightNode(PointOfInterest node, PlayerPiece piece)
    {
        if (nodeHighlightPrefab != null && uiCanvas != null && mainCamera != null)
        {
            GameObject highlight = Instantiate(nodeHighlightPrefab, uiCanvas.transform);
            RectTransform highlightRect = highlight.GetComponent<RectTransform>();
            Vector2 anchoredPos = WorldToCanvasPosition(uiCanvas, mainCamera, node.transform.position);
            highlightRect.anchoredPosition = anchoredPos;
            spawnedHighlights.Add(highlight);

            // 클릭 이벤트 연결
            Button btn = highlight.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => gameManager.MoveSelectedPieceTo(node));
            }
        }
        highlightedNodes.Add(node);
        piece.canMove = true;
    }
    public void ClearHighlights()
    {
        // 기존 하이라이트 프리팹 모두 제거
        foreach (var go in spawnedHighlights)
        {
            Destroy(go);
        }
        spawnedHighlights.Clear();

        highlightedNodes.Clear();
        if (gameManager != null && gameManager.CurrentPlayer != null)
            gameManager.CurrentPlayer.canMove = false;
    }

    // 하이라이트 이미지로 하게 바껴서 canvas에다 표시해야됨 > canvass에 좌표 전달용
    public static Vector2 WorldToCanvasPosition(Canvas canvas, Camera camera, Vector3 worldPosition)
    {
        Vector2 viewportPosition = camera.WorldToViewportPoint(worldPosition);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        return new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
        );
    }
}
