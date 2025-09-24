using System.Collections.Generic;
using UnityEngine;

public class TailManager : MonoBehaviour
{
    public GameObject tailPrefab;
    public int tailLength = 0;

    private List<GameObject> tailObjects = new List<GameObject>();
    private List<Vector2Int> tailPositions = new List<Vector2Int>();  // Grid 기준 위치 리스트

    public GridManager gridManager;
    private Vector2Int previousHeadPos; // 꼬리 위치용

    private List<Vector3> tailWorldPositions = new List<Vector3>(); // 실제 꼬리 위치(부드러운 이동용)

    private Vector2Int entrancePos;  // 입구 좌표 저장
    private List<Vector2Int> regenCells = new List<Vector2Int>();  // 재생성 가능한 3개 그리드 좌표

    [SerializeField]private float regenInterval = 0.5f;  // 재생성 딜레이 (초)
    private float regenTimer = 0f;

    private pacPlayerController playerController;

    void Start()
    {
        playerController = GetComponent<pacPlayerController>();
        entrancePos = gridManager.ReturnEntrancePosition();

        // 입구 바로 위 칸과 양옆 칸 좌표 계산 및 리스트에 저장
        // 입구가 (x, y)라 하면 위 칸은 (x, y - 1), 양옆은 (x-1, y-1), (x+1, y-1)
        regenCells.Clear();
        int x = entrancePos.x;
        int y = entrancePos.y;

        regenCells.Add(new Vector2Int(x, y - 1));
        regenCells.Add(new Vector2Int(x - 1, y - 1));
        regenCells.Add(new Vector2Int(x + 1, y - 1));
    }

    void Update()
    {
        Vector2Int currentHeadPos = playerController.InternalGridPos;

        if (playerController.isMoving)
        {
            SmoothFollowUpdate();
        }

        RegenerationCheck();
    }

    void RegenerationCheck()
    {
        if (playerController == null) return;
        Vector2Int playerPos = playerController.InternalGridPos;

        // 플레이어 위치가 재생성 가능한 셀 3개 중 하나에 있는지 확인
        bool inRegenZone = regenCells.Contains(playerPos);

        if (inRegenZone)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;

                // 꼬리 길이 1 증가, 최대 꼬리 길이 제한 적용 필요하면 여기서 처리
                SetTailLength(tailLength + 1);
            }
        }
        else
        {
            // 재생성 영역 벗어나면 타이머 리셋 (선택 사항)
            regenTimer = 0f;
        }
    }

    public void SetTailLength(int length)
    {
        tailLength = Mathf.Max(0, length);
        UpdateTailObjects();
    }

    public void UpdateHeadPosition(Vector2Int newHeadPos)
    {
        if (tailLength > 0)
        {
            tailPositions.Insert(0, previousHeadPos);
            if (tailPositions.Count > tailLength)
                tailPositions.RemoveAt(tailPositions.Count - 1);
        }
        previousHeadPos = newHeadPos;

        UpdateTailObjects();
        InitializeTailWorldPositions();
        UpdateTailObjectsPositionInstant();
    }

    void InitializeTailWorldPositions()
    {
        // 새 꼬리가 생기거나 길이가 바뀌었을 때 부드러운 위치 배열 초기화
        while (tailWorldPositions.Count < tailLength)
        {
            Vector3 pos = tailPositions.Count > tailWorldPositions.Count
                ? gridManager.CoordToWorldPos(tailPositions[tailWorldPositions.Count].x, tailPositions[tailWorldPositions.Count].y)
                : tailWorldPositions.Count > 0 ? tailWorldPositions[tailWorldPositions.Count - 1] : transform.position;
            tailWorldPositions.Add(pos);
        }
        while (tailWorldPositions.Count > tailLength)
        {
            tailWorldPositions.RemoveAt(tailWorldPositions.Count - 1);
        }
    }

    void SmoothFollowUpdate()
    {
        const float followSpeed = 10f;  // 조정 가능, 꼬리 부드러움 정도
        if (tailObjects.Count == 0) return;

        // 머리 위치 월드 좌표
        Vector3 headWorldPos = gridManager.CoordToWorldPos(previousHeadPos.x, previousHeadPos.y);
        headWorldPos.z -= gridManager.cellSize * 0.5f;

        // 첫 꼬리 목표 위치는 머리 위치
        Vector3 previousPos = headWorldPos;

        for (int i = 0; i < tailObjects.Count; i++)
        {
            Vector3 targetPos = tailWorldPositions[i];
            // 목표 위치를 따라 부드럽게 이동 (lerp)
            tailWorldPositions[i] = Vector3.Lerp(targetPos, previousPos, Time.deltaTime * followSpeed);

            // 꼬리 오브젝트 위치 업데이트
            tailObjects[i].transform.position = tailWorldPositions[i];
            // 꼬리 방향(회전)은 간단히 머리 방향 따르도록 기본 유지 가능

            previousPos = tailWorldPositions[i];
        }
    }

    // 꼬리 프리팹 생성/삭제 동기화 함수, 앞서 구현된 함수 그대로 유지
    void UpdateTailObjects()
    {
        while (tailObjects.Count < tailLength)
        {
            GameObject tailObj = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity, transform);
            TailSegment segment = tailObj.GetComponent<TailSegment>();
            if (segment != null)
            {
                segment.tailManager = this;
                segment.nextSegment = tailObjects.Count > 0
                    ? tailObjects[tailObjects.Count - 1].GetComponent<TailSegment>()
                    : null;
            }
            tailObjects.Add(tailObj);
        }
        while (tailObjects.Count > tailLength)
        {
            GameObject toRemove = tailObjects[tailObjects.Count - 1];
            tailObjects.RemoveAt(tailObjects.Count - 1);
            Destroy(toRemove);
        }
    }

    // 꼬리 위치 즉시 동기화 (초기화용)
    void UpdateTailObjectsPositionInstant()
    {
        for (int i = 0; i < tailObjects.Count; i++)
        {
            if (i >= tailPositions.Count) break;

            Vector3 pos = gridManager.CoordToWorldPos(tailPositions[i].x, tailPositions[i].y);
            tailObjects[i].transform.position = new Vector3(pos.x, 0, pos.z - gridManager.cellSize * 0.5f);
        }
    }

    // 꼬리 충돌처리 등 기존 기능 유지
    public void HandleTailCollision(TailSegment collidedSegment, GameObject collider)
    {
        collidedSegment.DeleteSegment();

        int indexToRemove = tailObjects.FindIndex(obj => obj == collidedSegment.gameObject);
        if (indexToRemove >= 0)
        {
            int removeCount = tailObjects.Count - indexToRemove;
            tailObjects.RemoveRange(indexToRemove, removeCount);
            tailPositions.RemoveRange(indexToRemove, removeCount);
            tailWorldPositions.RemoveRange(indexToRemove, removeCount);
            tailLength = Mathf.Max(0, tailLength - removeCount);
        }

        if (collider.CompareTag("Enemy"))
        {
            Destroy(collider);
        }
        // 게임오버 조건 체크는 별도 함수에서 처리 예정
    }
}
