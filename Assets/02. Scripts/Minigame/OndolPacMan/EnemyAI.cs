using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Grid 좌표와 GameObject.position의 혼용에 주의
// 2. 배열 검사에서의 좌표는 인게임에서 보이는 좌표와 y축이 반대임에 주의

public class EnemyAI : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject player;

    private Vector2Int enemyGridPos;    // Grid 좌표 (배열 인덱스)
    private Vector2Int playerGridPos;   // Grid 좌표 (배열 인덱스)

    private enum State { RandomMove, ChasePlayer }
    private State currentState = State.RandomMove;

    private float moveCooldown = 0.3f;
    private float moveTimer = 0f;

    private pacPlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<pacPlayerController>();
        enemyGridPos = FindEnemyGridPos();
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            moveTimer = 0f;
            // player GridPos는 pacPlayerController의 내부 GridPos로부터 가져오기
            playerGridPos = playerController.InternalGridPos;

            if (CanSeePlayer(enemyGridPos, playerGridPos))
            {
                Debug.Log("Chase Player");
                currentState = State.ChasePlayer;
            }
            else
            {
                currentState = State.RandomMove;
            }
            switch (currentState)
            {
                case State.RandomMove:
                    RandomMove();
                    break;
                case State.ChasePlayer:
                    ChasePlayer();
                    break;
            }
        }
    }

    // 월드 좌표 → 그리드 배열 좌표 변환 함수
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

    void ChasePlayer()
    {
        Vector2Int direction = playerGridPos - enemyGridPos;
        Vector2Int moveDir = Vector2Int.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            moveDir = (direction.x > 0) ? Vector2Int.right : Vector2Int.left;
        else if (direction.y != 0)
            moveDir = (direction.y > 0) ? Vector2Int.up : Vector2Int.down;

        Vector2Int newGridPos = enemyGridPos + moveDir;
        if (IsWalkable(newGridPos))
        {
            MoveTo(newGridPos);
        }
        else
        {
            RandomMove();
        }
    }

    bool IsWalkable(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= gridManager.gridWidth || gridPos.y < 0 || gridPos.y >= gridManager.gridHeight)
        {
            Debug.Log("[IsWalkable] Out-of-bounds: " + gridPos + " (grid size: " + gridManager.gridWidth + ", " + gridManager.gridHeight + ")");
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
        enemyGridPos = newGridPos;
        Vector3 worldPos = gridManager.CoordToWorldPos(newGridPos.x, newGridPos.y);
        transform.position = new Vector3(worldPos.x, 0, worldPos.z - gridManager.cellSize * 0.5f);
    }
}
