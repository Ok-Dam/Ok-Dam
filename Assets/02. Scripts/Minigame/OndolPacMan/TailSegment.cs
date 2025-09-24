using System.Collections;
using UnityEngine;

public class TailSegment : MonoBehaviour
{
    public TailSegment nextSegment; // now points to next segment toward tail end (same as before)

    public TailManager tailManager;

    public Vector2Int currentGridPos;
    public Vector2Int targetGridPos;
    public float moveProgress = 1f;
    private float moveDuration;
    private bool isMoving = false;

    private Collider col;
    private Animator anim;

    private void Awake()
    {
        col = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
    }

    public void InitializePosition(Vector2Int startGridPos, float moveDurationSeconds)
    {
        currentGridPos = startGridPos;
        targetGridPos = startGridPos;
        moveProgress = 1f;
        moveDuration = moveDurationSeconds;
        isMoving = false;

        UpdateWorldPosition();
    }

    void UpdateWorldPosition()
    {
        Vector3 pos = tailManager.gridManager.CoordToWorldPos(currentGridPos.x, currentGridPos.y);
        pos.z -= tailManager.gridManager.cellSize * 0.5f;
        transform.position = pos;
    }

    public void MoveToNextGrid(Vector2Int newTargetGrid)
    {
        currentGridPos = targetGridPos;
        targetGridPos = newTargetGrid;
        moveProgress = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        moveProgress += Time.deltaTime / moveDuration;
        if (moveProgress >= 1f)
        {
            moveProgress = 1f;
            isMoving = false;
            currentGridPos = targetGridPos;
        }
        Vector3 startPos = tailManager.gridManager.CoordToWorldPos(currentGridPos.x, currentGridPos.y);
        startPos.z -= tailManager.gridManager.cellSize * 0.5f;
        Vector3 endPos = tailManager.gridManager.CoordToWorldPos(targetGridPos.x, targetGridPos.y);
        endPos.z -= tailManager.gridManager.cellSize * 0.5f;
        transform.position = Vector3.Lerp(startPos, endPos, moveProgress);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pacEnemy") || other.CompareTag("pacPlayerHead")) { 
            if (col != null) col.enabled = false;
            if (anim != null)
            {
                anim.SetTrigger("Die");
            }
            tailManager.HandleTailCollision(this, other.gameObject);
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public void DeleteSegment()
    {
        if (nextSegment != null)
            nextSegment.DeleteSegment();
        Destroy(gameObject);
    }
}
