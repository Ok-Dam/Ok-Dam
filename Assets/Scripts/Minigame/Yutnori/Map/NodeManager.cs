using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    [Header("��� �θ� (Nodes ������Ʈ)")]
    [SerializeField] private Transform nodesParent; // Inspector���� Nodes(Empty) �Ҵ�

    [SerializeField] private Canvas uiCanvas; // Inspector���� �Ҵ�
    [SerializeField] private Camera mainCamera; //

    [Header("��� ���̶���Ʈ ������")]
    [SerializeField] private GameObject nodeHighlightPrefab; // Inspector���� �Ҵ�

    private YutnoriGameManager gameManager;

    private List<PointOfInterest> boardNodes; // 29�� ���, nodeNumber �������

    private List<GameObject> spawnedHighlights = new List<GameObject>(); // �������� ������ ���̶���Ʈ ������Ʈ
    private List<PointOfInterest> highlightedNodes = new List<PointOfInterest>();
    public IReadOnlyList<PointOfInterest> HighlightedNodes => highlightedNodes;

    void Awake()
    {
        // �ڽ� ��� �ڵ� ���� �� nodeNumber �� ����
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

    // ���� ���(�б��� ��) ��ȯ
    public List<PointOfInterest> FindPreviousNodes(PointOfInterest node)
    {
        return node.PreviousPointsOfInterest;
    }

    /// <summary>
    /// �̵� ���� ��� ���̶���Ʈ (�б��� ����, ��ȯ/���� ���)
    /// </summary>
    public void HighlightReachableNodes(PointOfInterest startNode, int distance, PlayerPiece piece)
    {
        ClearHighlights();

        // ����(-1) ó��: ��� ���̸� 29��(�����), ��� �ĸ� PreviousPointsOfInterest
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

        // �Ϲ� �̵�: �б��� ����, ���� ��� ��� Ž��
        var reachableNodes = FindReachableNodes(startNode, distance);
        foreach (var node in reachableNodes)
            HighlightNode(node, piece);
    }


    private List<PointOfInterest> FindReachableNodes(PointOfInterest start, int distance)
    {
        List<PointOfInterest> result = new();
        Queue<(PointOfInterest node, int depth)> queue = new();
        HashSet<(int, int)> visited = new();

        // 1. �������� junction�̸� shortcutTarget�� enqueue
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

        // 2. ���Ĵ� ������ NextPointsOfInterest�� Ž��
        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();

            // ������(29)�� �����ϸ� ������ ����
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

        // 1. �������� junction�̸� shortcutTarget�� enqueue
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

            // ����
            if (current == end)
                return path;

            // ������(29)�� �����ϸ� ���� (��, ��������� �ٷ� ���߸� �� ��)
            if (current.nodeNumber == 29 && (current != start || path.Count > 1))
                continue;

            // ���Ĵ� ������ NextPointsOfInterest�� Ž��
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
        Debug.Log("FindPath - ��� ����");
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

            // Ŭ�� �̺�Ʈ ����
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
        // ���� ���̶���Ʈ ������ ��� ����
        foreach (var go in spawnedHighlights)
        {
            Destroy(go);
        }
        spawnedHighlights.Clear();

        highlightedNodes.Clear();
        if (gameManager != null && gameManager.CurrentPlayer != null)
            gameManager.CurrentPlayer.canMove = false;
    }

    // ���̶���Ʈ �̹����� �ϰ� �ٲ��� canvas���� ǥ���ؾߵ� > canvass�� ��ǥ ���޿�
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
