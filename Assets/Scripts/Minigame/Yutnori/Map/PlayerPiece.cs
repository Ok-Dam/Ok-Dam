using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum HanokPart
{
    Roof,   // 지붕
    Column, // 벽
    Wall,   // 기둥
    Floor   // 바닥
}

public class PlayerPiece : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private HanokPart hanokPart;
    [SerializeField] private MapGenerator mapGenerator;

    public Sprite finishHudSprite; // 완주시 띄울 본인 이미지

    // 업기 관련
    public List<PlayerPiece> stackedPieces = new List<PlayerPiece>();
    public PlayerPiece parentPiece = null;

    private bool shortcutUsed = false;
    public PlayerState playerState; // 플레이어 상태 참조

    public PointOfInterest currentNode { get; private set; }

    private Coroutine blinkCoroutine;

    void Start()
    {
        SetCurrentNode(mapGenerator.getStartingPoint());
    }

    public void SetCurrentNode(PointOfInterest node)
    {
        currentNode = node;
    }

    void OnMouseDown()
    {
        PlayerPiece root = this;
        while (root.parentPiece != null)
            root = root.parentPiece;

        if (gameManager.stage == GameStage.Move)
            gameManager.SelectPiece(root);
    }

    public bool HasStarted()
    {
        return currentNode.Type != POIType.Start;
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
            highlighter.StartBlink();
        else
            highlighter.StopBlink();
    }

    public void MoveTo(PointOfInterest destination)
    {
        if (parentPiece != null)
        {
            Debug.LogWarning($"[MoveTo] {name} is a child! MoveTo should only be called on parent.");
            return;
        }
        StartCoroutine(MoveByPath(destination));
    }

    private IEnumerator MoveByPath(PointOfInterest destination)
    {
        List<PointOfInterest> path = NodeManager.FindPath(currentNode, destination);
        if (path == null || path.Count < 2)
            yield break;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = path[i - 1].transform.position + Vector3.up * 0.5f;
            Vector3 end = path[i].transform.position + Vector3.up * 0.5f;
            yield return MoveAlongArcWithStacked(start, end, 0.4f, 4.0f);
            currentNode = path[i];
        }

        // 1. 먼저 상호작용(BuffPOI 등) 처리
        gameManager.setGameStage(GameStage.Interact);
        gameManager.interactByPOI(this, currentNode);

        // 2. 그 다음 업기 시도
        TryStackOnSameNode();
    }


    // 빽도용 이동 함수 (뒤로 움직이는)
    public IEnumerator MoveByBackdoPath(PointOfInterest prevNode)
    {
        Vector3 start = currentNode.transform.position + Vector3.up * 0.5f;
        Vector3 end = prevNode.transform.position + Vector3.up * 0.5f;
        yield return MoveAlongArcWithStacked(start, end, 0.4f, 4.0f);
        currentNode = prevNode;

        TryStackOnSameNode();
        gameManager.setGameStage(GameStage.Interact);
        gameManager.interactByPOI(this, currentNode);
    }


    private IEnumerator MoveAlongArc(Vector3 start, Vector3 end, float duration, float arcHeight)
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            float height = 4 * arcHeight * t * (1 - t);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += height;
            transform.position = pos;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

    // 업었을 때
    private IEnumerator MoveAlongArcWithStacked(Vector3 start, Vector3 end, float duration, float arcHeight)
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            float height = 4 * arcHeight * t * (1 - t);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += height;

            SetStackedPositions(pos); // 재귀적 위치 동기화

            time += Time.deltaTime;
            yield return null;
        }
        SetStackedPositions(end); // 마지막 위치도 맞추기
    }

    // 부모 위치를 기준으로 업힌 말들을 배치하는 함수 (basePos가 항상 중앙)
    private void SetStackedPositions(Vector3 basePos)
    {
        int count = stackedPieces.Count + 1; // 부모 포함 전체 개수
        float offset = 1.2f; // 간격 조정

        if (count == 1)
        {
            // 1개: 중앙
            transform.position = basePos;
        }
        else if (count == 2)
        {
            // 2개: 좌/우로 배치 (basePos가 중앙)
            transform.position = basePos + Vector3.left * offset / 2f;
            stackedPieces[0].transform.position = basePos + Vector3.right * offset / 2f;
        }
        else if (count == 3)
        {
            // 3개: basePos를 중심으로 정삼각형 배치
            float radius = offset * 0.6f;
            float angleStep = 2 * Mathf.PI / 3;
            for (int i = 0; i < 3; i++)
            {
                float angle = angleStep * i - Mathf.PI / 2; // 시작 각도 위쪽
                Vector3 pos = basePos + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                if (i == 0)
                    transform.position = pos;
                else
                    stackedPieces[i - 1].transform.position = pos;
            }
        }
        else if (count == 4)
        {
            // 4개: basePos를 중심으로 2x2 정사각형 배치
            float half = offset / 2f;
            Vector3[] positions = new Vector3[]
            {
            basePos + new Vector3(-half, 0, half),
            basePos + new Vector3(half, 0, half),
            basePos + new Vector3(-half, 0, -half),
            basePos + new Vector3(half, 0, -half)
            };
            transform.position = positions[0];
            stackedPieces[0].transform.position = positions[1];
            stackedPieces[1].transform.position = positions[2];
            stackedPieces[2].transform.position = positions[3];
        }
    }

    //업기. 손자는 불가능.
    public void TryStackOnSameNode()
    {
        if (parentPiece != null)
        {
            return; // 이미 자식이면 업기 불가
        }

        var allPieces = FindObjectsOfType<PlayerPiece>();
        foreach (var other in allPieces)
        {
            if (other == this) continue;
            bool sameNode = (other.currentNode == this.currentNode);
            bool samePlayer = (other.playerState == this.playerState);
            bool otherIsRoot = (other.parentPiece == null);

            if (sameNode && samePlayer && otherIsRoot)
            {
                // 1. 내 자식들 모두 이미 해당 노드에 있는 말의 자식으로 옮기기
                foreach (var child in new List<PlayerPiece>(stackedPieces))
                {
                    child.parentPiece = other;
                    if (!other.stackedPieces.Contains(child))
                        other.stackedPieces.Add(child);
                }
                stackedPieces.Clear();

                // 2. 내 기존 부모와의 관계 해제
                if (parentPiece != null)
                {
                    parentPiece.stackedPieces.Remove(this);
                    parentPiece = null;
                }

                // 3. 나도 기존 노드 말의 자식으로 편입
                if (!other.stackedPieces.Contains(this))
                {
                    this.parentPiece = other;
                    other.stackedPieces.Add(this);
                }

                other.SetStackedPositions(currentNode.transform.position);
                return;
            }
        }
    }
    public List<PlayerPiece> GetAllStacked()
    {
        var result = new List<PlayerPiece>();
        var visited = new HashSet<PlayerPiece>();
        CollectStacked(this, result, visited);
        return result;
    }
    private void CollectStacked(PlayerPiece piece, List<PlayerPiece> result, HashSet<PlayerPiece> visited)
    {
        if (visited.Contains(piece)) return;
        visited.Add(piece);
        result.Add(piece);
        foreach (var child in piece.stackedPieces)
            CollectStacked(child, result, visited);
    }

    public void SetShortcutUsed(bool used) { shortcutUsed = used; }
    public bool HasUsedShortcut() { return shortcutUsed; }
}
