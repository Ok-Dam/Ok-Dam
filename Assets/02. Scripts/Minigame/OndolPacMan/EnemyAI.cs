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
    // 시야에서 사라져도 몇 초 동안은 계속 쫓아옴
    private float chaseTimer = 0f;
    private float chaseDuration = 3f; 

    private float moveCooldown = 0.3f;
    private float moveTimer = 0f;

    private pacPlayerController playerController;

    private bool isMoving = false; // 이동 중 상태 플래그
    private Vector3 targetWorldPos; // 부드러운 이동 목표 위치
    private Vector2Int currentDirection = Vector2Int.down; // 현재 이동 방향 (초기값은 아래쪽)

    void Start()
    {
        playerController = player.GetComponent<pacPlayerController>();
        enemyGridPos = FindEnemyGridPos();
    }

    void Update()
    {
        // 이동 중이면 이동 처리를 먼저
        if (isMoving) return;

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            moveTimer = 0f;
            playerGridPos = playerController.InternalGridPos;

            if (CanSeePlayer(enemyGridPos, playerGridPos))
            {
                Debug.Log("Chase Player");
                currentState = State.ChasePlayer;
                chaseTimer = chaseDuration;  // 시야 확보 시 타이머 리셋
            }
            else
            {
                if (currentState == State.ChasePlayer)
                {
                    // 시야를 잃었지만 타이머가 남았으면 계속 ChasePlayer 유지
                    chaseTimer -= moveCooldown;
                    if (chaseTimer <= 0f)
                    {
                        currentState = State.RandomMove;
                    }
                }
                else
                {
                    currentState = State.RandomMove;
                }
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
        currentDirection = newGridPos - enemyGridPos; // 이동 방향 갱신 (moveTo 후 enemyGridPos가 바뀌므로 임시변수 필요)
        targetWorldPos = gridManager.CoordToWorldPos(newGridPos.x, newGridPos.y);
        targetWorldPos = new Vector3(targetWorldPos.x, 0, targetWorldPos.z - gridManager.cellSize * 0.5f);

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
}
