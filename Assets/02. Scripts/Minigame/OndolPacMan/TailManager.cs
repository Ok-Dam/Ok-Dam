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

    private List<Vector3> tailWorldPositions = new List<Vector3>();

    private Vector2Int entrancePos;
    private List<Vector2Int> regenCells = new List<Vector2Int>();

    [SerializeField] private float regenInterval = 0.5f;
    private float regenTimer = 0f;

    private pacPlayerController playerController;

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

        Debug.Log("[TailManager Start] Initialized with entrance at " + entrancePos);
    }

    void Update()
    {
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

        bool inRegenZone = regenCells.Contains(playerPos);
        Debug.Log($"[RegenerationCheck] Player pos: {playerPos} In regen zone: {inRegenZone} Regen timer: {regenTimer}");

        if (inRegenZone)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;
                Debug.Log("[RegenerationCheck] Regenerating tail length.");
                SetTailLength(tailLength + 1);
            }
        }
        else
        {
            if (regenTimer > 0f)
                Debug.Log("[RegenerationCheck] Regen zone exit, reset timer.");
            regenTimer = 0f;
        }
    }

    public void SetTailLength(int length)
    {
        int oldLength = tailLength;
        tailLength = Mathf.Max(0, length);

        Debug.Log($"[SetTailLength] Tail length changing from {oldLength} to {tailLength}");

        if (tailLength > oldLength && tailPositions.Count == 0)
        {
            Vector2Int currentHead = playerController.InternalGridPos;
            Vector2Int tailStartPos = currentHead - playerController.LastMoveDirection;
            tailPositions.Insert(0, tailStartPos);
            Debug.Log($"[SetTailLength] Added initial tail start position {tailStartPos} behind head {currentHead}");
        }

        UpdateTailObjects();
        InitializeTailWorldPositions();
        UpdateTailObjectsPositionInstant();
    }

    public void UpdateHeadPosition(Vector2Int newHeadPos)
    {
        Debug.Log($"[UpdateHeadPosition] Called with new head pos: {newHeadPos}; previousHeadPos: {previousHeadPos}");

        if (tailLength > 0 && newHeadPos != previousHeadPos)
        {
            tailPositions.Insert(0, previousHeadPos);
            int maxCount = tailLength;

            if (tailPositions.Count > maxCount)
            {
                tailPositions.RemoveRange(maxCount, tailPositions.Count - maxCount);
                Debug.Log("[UpdateHeadPosition] Trimmed tail positions list to max count: " + maxCount);
            }
            Debug.Log($"[UpdateHeadPosition] Inserted previous head pos: {previousHeadPos}. Tail positions count: {tailPositions.Count}");
        }

        previousHeadPos = newHeadPos;

        UpdateTailObjects();
        InitializeTailWorldPositions();
        UpdateTailObjectsPositionInstant();
    }

    void InitializeTailWorldPositions()
    {
        Debug.Log("[InitializeTailWorldPositions] Syncing tailWorldPositions list.");
        while (tailWorldPositions.Count < tailLength)
        {
            int idx = tailWorldPositions.Count;
            Vector3 pos = idx < tailPositions.Count
                ? gridManager.CoordToWorldPos(tailPositions[idx].x, tailPositions[idx].y)
                : (tailWorldPositions.Count > 0 ? tailWorldPositions[tailWorldPositions.Count - 1] : gridManager.CoordToWorldPos(entrancePos.x, entrancePos.y));
            tailWorldPositions.Add(pos);

            Debug.Log($"[InitializeTailWorldPositions] Added world position {pos} at index {idx}");
        }
        while (tailWorldPositions.Count > tailLength)
        {
            tailWorldPositions.RemoveAt(tailWorldPositions.Count - 1);
            Debug.Log("[InitializeTailWorldPositions] Removed excess tail world position.");
        }
    }

    void SmoothFollowUpdate()
    {
        const float followSpeed = 3f;  // Slower follow speed now
        if (tailObjects.Count == 0 || tailWorldPositions.Count < tailObjects.Count)
            return;

        Vector3 prevPos = playerController.transform.position;

        for (int i = 0; i < tailObjects.Count; i++)
        {
            Vector3 targetPos = tailWorldPositions[i];
            float lerpFactor = Mathf.Clamp01(Time.deltaTime * followSpeed);
            tailWorldPositions[i] = Vector3.Lerp(targetPos, prevPos, lerpFactor);
            // Or alternatively:
            // float maxMoveDist = followSpeed * Time.deltaTime;
            // tailWorldPositions[i] = Vector3.MoveTowards(targetPos, prevPos, maxMoveDist);

            tailObjects[i].transform.position = tailWorldPositions[i];
            prevPos = tailWorldPositions[i];
        }
    }


    void UpdateTailObjects()
    {
        Debug.Log($"[UpdateTailObjects] Tail length: {tailLength}, Tail objects count: {tailObjects.Count}");
        while (tailObjects.Count < tailLength)
        {
            int idx = tailObjects.Count;
            Vector2Int spawnGridPos = idx < tailPositions.Count ? tailPositions[idx] : playerController.InternalGridPos - playerController.LastMoveDirection;
            Vector3 spawnWorldPos = gridManager.CoordToWorldPos(spawnGridPos.x, spawnGridPos.y);
            spawnWorldPos.z -= gridManager.cellSize * 0.5f;

            Debug.Log($"[UpdateTailObjects] Spawning tail segment {idx} at grid {spawnGridPos}, world pos {spawnWorldPos}");

            GameObject tailObj = Instantiate(tailPrefab, spawnWorldPos, Quaternion.identity, transform);

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
            Debug.Log("[UpdateTailObjects] Removed excess tail object.");
        }
    }

    void UpdateTailObjectsPositionInstant()
    {
        for (int i = 0; i < tailObjects.Count; i++)
        {
            if (i >= tailPositions.Count) break;

            Vector2Int gridPos = tailPositions[i];
            Vector3 pos = gridManager.CoordToWorldPos(gridPos.x, gridPos.y);
            Vector3 adjustedPos = new Vector3(pos.x, 0, pos.z - gridManager.cellSize * 0.5f);

            tailObjects[i].transform.position = adjustedPos;

            Debug.Log($"[UpdateTailObjectsPositionInstant] Tail segment {i} position set to GridPos: {gridPos}, WorldPos: {adjustedPos}");
        }
    }

    public void HandleTailCollision(TailSegment collidedSegment, GameObject collider)
    {
        collidedSegment.DeleteSegment();

        int indexToRemove = tailObjects.FindIndex(obj => obj == collidedSegment.gameObject);
        Debug.Log("[HandleTailCollision] Collision detected on tail index " + indexToRemove);

        if (indexToRemove >= 0)
        {
            int removeCount = tailObjects.Count - indexToRemove;
            tailObjects.RemoveRange(indexToRemove, removeCount);
            tailPositions.RemoveRange(indexToRemove, removeCount);
            tailWorldPositions.RemoveRange(indexToRemove, removeCount);
            tailLength = Mathf.Max(0, tailLength - removeCount);

            Debug.Log($"[HandleTailCollision] Removed {removeCount} tail segments. New tail length: {tailLength}");
        }

        if (collider.CompareTag("Enemy"))
        {
            Destroy(collider);
            Debug.Log("[HandleTailCollision] Enemy destroyed on collision.");
        }
    }
}
