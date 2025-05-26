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

    public bool isFinished = false;

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
        Debug.Log($"[MoveByPath] {name} from {currentNode.name} to {destination.name}, path.Count={path?.Count ?? 0}");
        if (path == null || path.Count < 2)
            yield break;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = path[i - 1].transform.position + Vector3.up * 0.5f;
            Vector3 end = path[i].transform.position + Vector3.up * 0.5f;
            yield return MoveAlongArcWithStacked(start, end, 0.4f, 4.0f);
            currentNode = path[i];
        }

        TryStackOnSameNode();
        gameManager.setGameStage(GameStage.Interact);
        gameManager.interactByPOI(this, currentNode);
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

    // 부모 위치를 기준으로 업힌 말들을 배치하는 함수
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
            // 2개: 좌/우로 배치
            transform.position = basePos + Vector3.left * offset / 2f;
            stackedPieces[0].transform.position = basePos + Vector3.right * offset / 2f;
        }
        else if (count == 3)
        {
            // 3개: 위(1), 아래(2) 삼각형 형태
            float yOffset = 0.18f; // 세로 간격
            transform.position = basePos + new Vector3(0, 0, yOffset); // 위 중앙
            stackedPieces[0].transform.position = basePos + new Vector3(-offset / 2f, 0, -yOffset);
            stackedPieces[1].transform.position = basePos + new Vector3(offset / 2f, 0, -yOffset);
        }
        else if (count == 4)
        {
            // 4개: 2x2 정사각형
            float x = offset / 2f;
            float z = offset / 2f;
            transform.position = basePos + new Vector3(-x, 0, z);
            stackedPieces[0].transform.position = basePos + new Vector3(x, 0, z);
            stackedPieces[1].transform.position = basePos + new Vector3(-x, 0, -z);
            stackedPieces[2].transform.position = basePos + new Vector3(x, 0, -z);
        }
        else
        {
            // 5개 이상: 일렬(혹은 추가 확장)
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = basePos + Vector3.right * offset * (i - (count - 1) / 2f);
                if (i == 0)
                    transform.position = pos;
                else
                    stackedPieces[i - 1].transform.position = pos;
            }
        }
    }
    
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
                if (this.parentPiece == null && !other.stackedPieces.Contains(this))
                {
                    this.parentPiece = other;
                    other.stackedPieces.Add(this);
                    other.SetStackedPositions(other.transform.position);
                }
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
