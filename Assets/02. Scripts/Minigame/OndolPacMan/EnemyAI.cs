using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject player;

    private Vector2Int enemyGridPos;    // Grid 좌표 (배열 인덱스)
    private Vector2Int playerGridPos;   // Grid 좌표 (배열 인덱스)

    private enum State { RandomMove, ChasePlayer }
    private State currentState = State.RandomMove;

    private float chaseTimer = 0f;
    private float chaseDuration = 3f;

    private float moveCooldown = 0.3f;

    private pacPlayerController playerController;

    private bool isMoving = false; // 이동 중 상태 플래그
    private Vector3 targetWorldPos; // 부드러운 이동 목표 위치
    private Vector2Int currentDirection = Vector2Int.down; // 현재 이동 방향 (초기값은 아래쪽)

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private int pathIndex = 0;

    void Start()
    {
        playerController = player.GetComponent<pacPlayerController>();
        enemyGridPos = FindEnemyGridPos();
    }

    void Update()
    {
        playerGridPos = playerController.InternalGridPos;

        if (CanSeePlayer(enemyGridPos, playerGridPos))
        {
            if (currentState != State.ChasePlayer)
            {
                Debug.Log("[State] Switching to ChasePlayer");
            }
            currentState = State.ChasePlayer;
            chaseTimer = chaseDuration;
        }
        else if (currentState == State.ChasePlayer)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
            {
                Debug.Log("[State] Chase timer expired, switching to RandomMove");
                currentState = State.RandomMove;
                currentPath.Clear();
            }
        }
        else
        {
            if (currentState != State.RandomMove)
            {
                Debug.Log("[State] Switching to RandomMove");
            }
            currentState = State.RandomMove;
        }

        if (!isMoving)
        {
            switch (currentState)
            {
                case State.RandomMove:
                    Debug.Log("[Action] RandomMove called");
                    RandomMove();
                    break;
                case State.ChasePlayer:
                    Debug.Log("[Action] ChasePlayerWithAStar called");
                    ChasePlayerWithAStar();
                    break;
            }
        }
    }



    Vector2Int FindEnemyGridPos()
    {
        Vector3 worldPos = transform.position;
        int gridX = Mathf.FloorToInt(worldPos.x / gridManager.cellSize);
        int gridY = Mathf.FloorToInt((gridManager.gridHeight - 1) - ((worldPos.z - gridManager.cellSize * 0.5f) / gridManager.cellSize));
        Vector2Int gridPos = new Vector2Int(gridX, gridY);
        return gridManager.ClampToGrid(gridPos);
    }

    bool CanSeePlayer(Vector2Int enemyGridPos, Vector2Int playerGridPos)
    {
        if (enemyGridPos.x == playerGridPos.x) // 같은 열
        {
            int startY = Mathf.Min(enemyGridPos.y, playerGridPos.y) + 1;
            int endY = Mathf.Max(enemyGridPos.y, playerGridPos.y) - 1;
            for (int y = startY; y <= endY; y++)
            {
                if (y < 0 || y >= gridManager.gridHeight)
                    return false;
                if (gridManager.gridMap[enemyGridPos.x, y] == 1)
                    return false;
            }
            return true;
        }
        else if (enemyGridPos.y == playerGridPos.y) // 같은 행
        {
            int startX = Mathf.Min(enemyGridPos.x, playerGridPos.x) + 1;
            int endX = Mathf.Max(enemyGridPos.x, playerGridPos.x) - 1;
            for (int x = startX; x <= endX; x++)
            {
                if (x < 0 || x >= gridManager.gridWidth)
                    return false;
                if (gridManager.gridMap[x, enemyGridPos.y] == 1)
                    return false;
            }
            return true;
        }
        return false;
    }

    void RandomMove()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in directions)
        {
            Vector2Int candidateGridPos = enemyGridPos + dir;
            if (IsWalkable(candidateGridPos))
            {
                possibleMoves.Add(candidateGridPos);
            }
        }

        if (possibleMoves.Count > 0)
        {
            Vector2Int selectedPos = possibleMoves[Random.Range(0, possibleMoves.Count)];
            MoveTo(selectedPos);
        }
        else
        {
            Debug.Log("[RandomMove] 이동 가능한 위치가 없음");
        }
    }

    void ChasePlayerWithAStar()
    {
        // Recalculate path if needed
        if (currentPath == null || currentPath.Count == 0 || pathIndex >= currentPath.Count || currentPath[currentPath.Count - 1] != playerGridPos)
        {
            Debug.Log($"[Pathfinding] Calculating new path from {enemyGridPos} to {playerGridPos}");
            currentPath = FindPath(enemyGridPos, playerGridPos);
            pathIndex = 0;
            Debug.Log($"[Pathfinding] New path length: {currentPath.Count}");
        }

        // Skip nodes that equal enemyGridPos to avoid moving "onto" itself repeatedly
        while (pathIndex < currentPath.Count && currentPath[pathIndex] == enemyGridPos)
        {
            pathIndex++;
        }

        if (currentPath != null && pathIndex < currentPath.Count)
        {
            Vector2Int nextGridPos = currentPath[pathIndex];
            Debug.Log($"[Movement] Next path node: {nextGridPos}, Current position: {enemyGridPos}, PathIndex: {pathIndex}");

            if (!IsAdjacent(enemyGridPos, nextGridPos))
            {
                Debug.LogWarning($"[Warning] Next node not adjacent! Current: {enemyGridPos}, Next: {nextGridPos}");
                currentPath.Clear();
                return;
            }

            if (IsWalkable(nextGridPos))
            {
                Debug.Log($"[Movement] Moving to {nextGridPos}");
                MoveTo(nextGridPos);
                pathIndex++;
            }
            else
            {
                Debug.LogWarning($"[Movement] Next node {nextGridPos} not walkable. Clearing path.");
                currentPath.Clear();
            }
        }
        else
        {
            Debug.Log("[Pathfinding] Path complete or empty, falling back to RandomMove");
            RandomMove();
        }
    }



    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        // 동일 좌표도 인접으로 처리
        return (dx == 0 && dy == 0) || (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    bool IsWalkable(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= gridManager.gridWidth || gridPos.y < 0 || gridPos.y >= gridManager.gridHeight)
        {
            Debug.Log("[IsWalkable] Out-of-bounds: " + gridPos + $" (grid size: {gridManager.gridWidth}, {gridManager.gridHeight})");
            return false;
        }
        if (gridManager.gridMap[gridPos.x, gridPos.y] == 1) // 벽
        {
            return false;
        }
        return true;
    }

    void MoveTo(Vector2Int newGridPos)
    {
        if (isMoving)
        {
            Debug.Log("[MoveTo] 이동 중 명령 무시");
            return;
        }

        Vector2Int oldPos = enemyGridPos;
        enemyGridPos = newGridPos;

        currentDirection = newGridPos - oldPos;

        targetWorldPos = gridManager.CoordToWorldPos(newGridPos.x, newGridPos.y);
        targetWorldPos = new Vector3(targetWorldPos.x, 0, targetWorldPos.z - gridManager.cellSize * 0.5f);

        UpdateRotation(); // 이동 시작과 동시에 회전하도록 위치 변경

        StartCoroutine(SmoothMove());
    }


    IEnumerator SmoothMove()
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float elapsed = 0f;
        float duration = moveCooldown;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetWorldPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetWorldPos;
        UpdateRotation();
        isMoving = false;
        // Immediately trigger next movement on coroutine end
        // Next move will be triggered in Update() since isMoving = false now
    }

    void UpdateRotation()
    {
        float zRotationDegrees = 0f;

        if (currentDirection == Vector2Int.up)
            zRotationDegrees = 0f;
        else if (currentDirection == Vector2Int.down)
            zRotationDegrees = 180f;
        else if (currentDirection == Vector2Int.left)
            zRotationDegrees = 90f;
        else if (currentDirection == Vector2Int.right)
            zRotationDegrees = -90f;

        Quaternion baseRotation = Quaternion.Euler(-10.0f, 0, zRotationDegrees);
        transform.rotation = baseRotation;
    }

    // A* 경로탐색 함수

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node(start);
        startNode.G = 0;
        startNode.H = GetHeuristic(start, goal);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // F 값이 가장 작은 노드를 선택
            Node currentNode = openList.OrderBy(n => n.F).First();

            if (currentNode.Position == goal)
                return RetracePath(currentNode);

            openList.Remove(currentNode);
            closedSet.Add(currentNode.Position);

            foreach (var neighbourPos in GetNeighbours(currentNode.Position))
            {
                if (closedSet.Contains(neighbourPos))
                    continue;
                if (!IsWalkable(neighbourPos))
                    continue;

                int newG = currentNode.G + 1;

                Node neighbourNode = openList.Find(n => n.Position == neighbourPos);
                if (neighbourNode == null)
                {
                    neighbourNode = new Node(neighbourPos);
                    neighbourNode.G = newG;
                    neighbourNode.H = GetHeuristic(neighbourPos, goal);
                    neighbourNode.Parent = currentNode;
                    openList.Add(neighbourNode);
                }
                else if (newG < neighbourNode.G)
                {
                    neighbourNode.G = newG;
                    neighbourNode.Parent = currentNode;
                }
            }
        }
        // 경로가 없으면 빈 리스트 반환
        return new List<Vector2Int>();
    }

    List<Vector2Int> RetracePath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        return path;
    }

    int GetHeuristic(Vector2Int a, Vector2Int b)
    {
        // 맨해튼 거리 사용
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in directions)
        {
            Vector2Int neighbourPos = pos + dir;
            if (neighbourPos.x >= 0 && neighbourPos.x < gridManager.gridWidth &&
                neighbourPos.y >= 0 && neighbourPos.y < gridManager.gridHeight &&
                IsWalkable(neighbourPos))
            {
                neighbours.Add(neighbourPos);
            }
        }
        return neighbours;
    }
}

// 경로탐색용 노드 클래스
public class Node
{
    public Vector2Int Position;
    public Node Parent;
    public int G; // 시작 노드에서 현재 노드까지 누적 비용
    public int H; // 현재 노드에서 목표 노드까지 휴리스틱 비용

    public int F { get { return G + H; } }

    public Node(Vector2Int position)
    {
        Position = position;
    }
}
