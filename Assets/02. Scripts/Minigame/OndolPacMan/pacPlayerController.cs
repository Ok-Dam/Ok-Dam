using System.Collections.Generic;
using UnityEngine;

public class pacPlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public float moveCooldown = 0.3f; // 이동 속도 조절용 딜레이
    private float moveTimer = 0f;

    // 내부적으로 배열 인덱스 기준 gridPos 유지
    private Vector2Int internalGridPos;

    // 인스펙터에 보이는 y좌표 반전 값
    [SerializeField]
    private Vector2Int displayedGridPos;

    private List<Vector2Int> tailPositions = new List<Vector2Int>(); // 꼬리 위치 리스트
    public int tailLength = 0; // 꼬리 길이 (처음엔 0)

    // 배열 인덱스 기준 방향 (위가 y+1)
    private Vector2Int currentDirection;

    void Start()
    {
        internalGridPos = FindEntrancePosition();
        UpdateDisplayedGridPos();
        UpdateWorldPosition();

        // 초기 방향을 화면에서 위쪽 움직임에 맞게 아래 방향으로 설정 (y축 반전 보정)
        currentDirection = Vector2Int.down; // (0, -1)
    }

    void Update()
    {
        HandleInput();

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            MoveForward();
            moveTimer = 0f;
        }
    }

    Vector2Int FindEntrancePosition()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (gridManager.gridMap[x, y] == 2)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(1, 1);
    }

    void HandleInput()
    {
        // 배열에서랑 실제 인겜에서랑 y좌표 반대라 반대로 설정
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Vector2Int.down)
        {
            currentDirection = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentDirection != Vector2Int.up)
        {
            currentDirection = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentDirection != Vector2Int.right)
        {
            currentDirection = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentDirection != Vector2Int.left)
        {
            currentDirection = Vector2Int.right;
        }
    }

    void MoveForward()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager is null!");
            return;
        }
        if (gridManager.gridMap == null)
        {
            Debug.LogError("GridMap is null inside GridManager!");
            return;
        }

        Vector2Int newPos = internalGridPos + currentDirection;

        if (newPos.x < 0 || newPos.x >= gridManager.gridWidth
            || newPos.y < 0 || newPos.y >= gridManager.gridHeight
            || gridManager.gridMap[newPos.x, newPos.y] == 1)
        {
            return;
        }

        MoveTo(newPos);
    }

    void MoveTo(Vector2Int newPos)
    {
        if (tailLength > 0)
        {
            tailPositions.Insert(0, internalGridPos);

            if (tailPositions.Count > tailLength)
            {
                tailPositions.RemoveAt(tailPositions.Count - 1);
            }
        }

        internalGridPos = newPos;
        UpdateDisplayedGridPos();
        UpdateWorldPosition();
    }

    void UpdateWorldPosition()
    {
        Vector3 worldPos = gridManager.CoordToWorldPos(internalGridPos.x, internalGridPos.y);
        transform.position = new Vector3(worldPos.x, 0, worldPos.z - gridManager.cellSize * 0.5f);

        float zRotationDegrees = 0f;

        if (currentDirection == Vector2Int.up)
            zRotationDegrees = 0f;
        else if (currentDirection == Vector2Int.down)
            zRotationDegrees = 180f;
        else if (currentDirection == Vector2Int.left)
            zRotationDegrees = 90f;
        else // right
            zRotationDegrees = -90f;

        Quaternion baseRotation = Quaternion.Euler(-10.0f, 0, zRotationDegrees);
        transform.rotation = baseRotation;
    }





    void UpdateDisplayedGridPos()
    {
        displayedGridPos = new Vector2Int(internalGridPos.x, gridManager.gridHeight - 1 - internalGridPos.y);
    }

    public Vector2Int gridPos
    {
        get { return displayedGridPos; }
    }
}
