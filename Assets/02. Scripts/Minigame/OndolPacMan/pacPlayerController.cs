using System.Collections;
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
    // 배열이랑 실제 유니티 화면에서랑 y축이 반대라서 배열 검사에선 internaGridPos쓰고, 내가 좌표 확인 할 땐 displayedGridPos로 확인
    [SerializeField]
    private Vector2Int displayedGridPos;

    private List<Vector2Int> tailPositions = new List<Vector2Int>(); // 꼬리 위치 리스트
    public int tailLength = 0; // 꼬리 길이 (처음엔 0)

    // 배열 인덱스 기준 방향 (위가 y+1)
    private Vector2Int currentDirection;

    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private Vector2Int? reservedDirection = null; // 이동 중 예약할 방향

    void Start()
    {
        internalGridPos = FindEntrancePosition();
        UpdateDisplayedGridPos();

        // internalGridPos에 맞춰 플레이어 게임 오브젝트의 위치를 명확히 설정
        Vector3 startWorldPos = gridManager.CoordToWorldPos(internalGridPos.x, internalGridPos.y);
        transform.position = new Vector3(startWorldPos.x, 0, startWorldPos.z - gridManager.cellSize * 0.5f);

        currentDirection = Vector2Int.down; // 초기 방향 설정
    }


    void Update()
    {
        HandleInput();

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown && !isMoving)
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
                    return gridManager.ClampToGrid(new Vector2Int(x, y));
                }
            }
        }
        return gridManager.ClampToGrid(new Vector2Int(1, 1));
    }


    void HandleInput()
    {
        Vector2Int inputDir = currentDirection;

        if (Input.GetKeyDown(KeyCode.W))
            inputDir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.S))
            inputDir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.A))
            inputDir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D))
            inputDir = Vector2Int.right;

        // 반대 방향 전환 방지
        if (inputDir != currentDirection && inputDir != -currentDirection)
        {
            if (isMoving)
            {
                reservedDirection = inputDir;
            }
            else
            {
                currentDirection = inputDir;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IPlayerInteractable interactable = other.GetComponent<IPlayerInteractable>();
        if (interactable != null)
        {
            interactable.OnPlayerInteract(gameObject);
        }
    }

    void MoveForward()
    {
        if (isMoving)
            return;

        if (reservedDirection.HasValue)
        {
            currentDirection = reservedDirection.Value;
            reservedDirection = null;

            // 예약 방향으로 회전은 바로 적용
            UpdateRotation();
        }

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

        if (gridManager.gridMap[newPos.x, newPos.y] == 1) return;
       
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

        // 방향 회전 먼저 즉시 적용
        UpdateRotation();

        // 부드러운 위치 이동 시작
        targetWorldPos = gridManager.CoordToWorldPos(internalGridPos.x, internalGridPos.y);
        targetWorldPos = new Vector3(targetWorldPos.x, 0, targetWorldPos.z - gridManager.cellSize * 0.5f);

        if (!isMoving)
        {
            StartCoroutine(SmoothMove());
        }
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

        // 위치 이동 완료 후 회전 갱신(필요시)
        UpdateRotation();

        // 해당 그리드 열 받음 처리 
        HeatMapManager heatMapManager = FindObjectOfType<HeatMapManager>();
        if (heatMapManager != null)
        {
            int x = internalGridPos.x;
            int y = internalGridPos.y;

            // Only heat allowed cell types via HeatCell internally
            if (heatMapManager.HeatCell(x, y))
            {
                pacGameManager.Instance.IncrementHeatedCount();
            }
        }

        isMoving = false;

        AfterMoveCheck();
    }

    // 출구 도착했으면 부품 다 모았는지 검사
    void AfterMoveCheck()
    {
        if (gridManager == null) return;

        int x = internalGridPos.x;
        int y = internalGridPos.y;

        if (x < 0 || x >= gridManager.gridWidth || y < 0 || y >= gridManager.gridHeight)
            return;

        if (gridManager.gridMap[x, y] == 3) // Exit tile detected
        {
            pacGameManager.Instance.CheckWinCondition();
        }
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
        else // right
            zRotationDegrees = -90f;

        Quaternion baseRotation = Quaternion.Euler(-10.0f, 0, zRotationDegrees);
        transform.rotation = baseRotation;
    }

    void UpdateWorldPosition()
    {
        // 기존 이동 즉시 위치 갱신 대신 부드러운 이동을 쓴다면 삭제하거나 주석 처리 가능
        Vector3 worldPos = gridManager.CoordToWorldPos(internalGridPos.x, internalGridPos.y);
        transform.position = new Vector3(worldPos.x, 0, worldPos.z - gridManager.cellSize * 0.5f);
        UpdateRotation();
    }

    void UpdateDisplayedGridPos()
    {
        displayedGridPos = new Vector2Int(internalGridPos.x, gridManager.gridHeight - 1 - internalGridPos.y);
    }

    public Vector2Int DisplayedGridPos
    {
        get { return displayedGridPos; }
    }
    public Vector2Int InternalGridPos
    {
        get { return internalGridPos; }
    }

}
