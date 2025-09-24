using System.Collections.Generic;
using UnityEngine;

public class TailManager : MonoBehaviour
{
    public GameObject tailPrefab;
    public int tailLength = 0;

    private List<GameObject> tailObjects = new List<GameObject>();
    private List<Vector2Int> tailPositions = new List<Vector2Int>();

    public GridManager gridManager;
    private Vector2Int previousHeadPos;

    private Vector2Int entrancePos;
    private List<Vector2Int> regenCells = new List<Vector2Int>();

    [SerializeField] private float regenInterval = 0.5f;
    private float regenTimer = 0f;

    private pacPlayerController playerController;

    public float tailSegmentMoveDuration = 0.3f;

    void Start()
    {
        playerController = GetComponent<pacPlayerController>();
        entrancePos = gridManager.ReturnEntrancePosition();

        previousHeadPos = playerController.InternalGridPos;
        tailPositions.Clear();

        regenCells.Clear();
        int x = entrancePos.x, y = entrancePos.y;

        regenCells.Add(new Vector2Int(x, y - 1));
        regenCells.Add(new Vector2Int(x - 1, y - 1));
        regenCells.Add(new Vector2Int(x + 1, y - 1));
    }

    void Update()
    {
        if (playerController.isMoving)
            UpdateTailMovement();

        RegenerationCheck();
    }

    void RegenerationCheck()
    {
        if (playerController == null) return;
        Vector2Int playerPos = playerController.InternalGridPos;

        bool inRegenZone = regenCells.Contains(playerPos);
        if (inRegenZone)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;
                SetTailLength(tailLength + 1);
            }
        }
        else
        {
            regenTimer = 0f;
        }
    }

    public void SetTailLength(int length)
    {
        int oldLength = tailLength;
        tailLength = Mathf.Max(0, length);

        if (tailLength > oldLength)
        {
            Vector2Int tailStartPos;

            if (tailPositions.Count > 1)
            {
                Vector2Int lastPos = tailPositions[tailPositions.Count - 1];
                Vector2Int prevPos = tailPositions[tailPositions.Count - 2];
                Vector2Int direction = lastPos - prevPos;
                tailStartPos = lastPos - direction;
            }
            else if (tailPositions.Count == 1)
            {
                Vector2Int lastPos = tailPositions[0];
                Vector2Int direction = playerController.LastMoveDirection;
                tailStartPos = lastPos - direction;
            }
            else
            {
                Vector2Int currentHead = playerController.InternalGridPos;
                Vector2Int direction = playerController.LastMoveDirection;
                tailStartPos = currentHead - direction;
            }

            tailPositions.Add(tailStartPos);
        }

        UpdateTailObjects();
        UpdateTailLayers();  // update layers for collisions
    }

    public void UpdateHeadPosition(Vector2Int newHeadPos)
    {
        if (tailLength > 0 && newHeadPos != previousHeadPos)
        {
            tailPositions.Insert(0, previousHeadPos);
            if (tailPositions.Count > tailLength)
                tailPositions.RemoveRange(tailLength, tailPositions.Count - tailLength);
        }
        previousHeadPos = newHeadPos;
    }

    void UpdateTailMovement()
    {
        for (int i = 0; i < tailObjects.Count; i++)
        {
            TailSegment segment = tailObjects[i].GetComponent<TailSegment>();
            Vector2Int nextGridPos = (i < tailPositions.Count) ? tailPositions[i] : playerController.InternalGridPos;

            if (segment.moveProgress >= 1f && segment.currentGridPos != nextGridPos)
            {
                segment.MoveToNextGrid(nextGridPos);
            }
        }
    }

    void UpdateTailObjects()
    {
        while (tailObjects.Count < tailLength)
        {
            int idx = tailObjects.Count;
            Vector2Int spawnGridPos = idx < tailPositions.Count ? tailPositions[idx] : playerController.InternalGridPos - playerController.LastMoveDirection;

            GameObject tailObj = Instantiate(tailPrefab, transform);

            TailSegment segment = tailObj.GetComponent<TailSegment>();
            if (segment != null)
            {
                segment.tailManager = this;
                // nextSegment points toward tail_end (i.e. next segment is tailObjects[i+1])
                TailSegment nextSeg = (tailObjects.Count < tailLength - 1) ? tailObjects[tailObjects.Count + 1].GetComponent<TailSegment>() : null;
                segment.nextSegment = nextSeg;
                segment.InitializePosition(spawnGridPos, playerController.moveCooldown);
            }
            tailObjects.Add(tailObj);
            UpdateTailLayers();
        }

        while (tailObjects.Count > tailLength)
        {
            GameObject toRemove = tailObjects[tailObjects.Count - 1];
            tailObjects.RemoveAt(tailObjects.Count - 1);
            Destroy(toRemove);
            UpdateTailLayers();
        }

        //꼬리 리스트 전체를 순회하면서 nextSegment를 일괄 설정 - failsafe
        for (int i = 0; i < tailObjects.Count; i++)
        {
            TailSegment segment = tailObjects[i].GetComponent<TailSegment>();
            if (segment != null)
            {
                segment.nextSegment = (i < tailObjects.Count - 1) ? tailObjects[i + 1].GetComponent<TailSegment>() : null;
            }
        }

    }

    private void UpdateTailLayers()
    {
        for (int i = 0; i < tailObjects.Count; i++)
        {
            TailSegment segment = tailObjects[i].GetComponent<TailSegment>();
            if (segment != null)
            {
                if (i == 0) // First tail segment gets special collision layer
                {
                    segment.gameObject.layer = LayerMask.NameToLayer("pacTailFirst");
                }
                else
                {
                    // Default layer or your normal tail layer
                    segment.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }

    // 꼬리 삭제
    public void HandleTailCollision(TailSegment collidedSegment, GameObject collider)
    {
        int indexToRemove = tailObjects.FindIndex(obj => obj == collidedSegment.gameObject);

        if (indexToRemove >= 0)
        {
            // 꼬리 끝 방향부터 부딫힌 segment까지 삭제: indexToRemove부터 꼬리 끝까지 삭제
            for (int i = tailObjects.Count - 1; i >= indexToRemove; i--)
            {
                tailObjects[i].GetComponent<TailSegment>().DeleteSegment();
            }

            // 꼬리 리스트와 위치 리스트에서 해당 구간만 삭제
            tailObjects.RemoveRange(indexToRemove, tailObjects.Count - indexToRemove);
            tailPositions.RemoveRange(indexToRemove, tailPositions.Count - indexToRemove);
            tailLength = tailObjects.Count;
            UpdateTailLayers();
        }

        if (collider.CompareTag("pacEnemy"))
        {
            EnemyAI enemyAI = collider.GetComponent<EnemyAI>();
            enemyAI.DieByTail();
        }
    }


}
