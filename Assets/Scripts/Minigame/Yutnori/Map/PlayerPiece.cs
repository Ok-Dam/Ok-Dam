using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum HanokPart
{
    Roof,   // ����
    Column, // ��
    Wall,   // ���
    Floor   // �ٴ�
}

public class PlayerPiece : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private HanokPart hanokPart;
    [SerializeField] private MapGenerator mapGenerator;

    public bool isFinished = false;

    // ���� ����
    public List<PlayerPiece> stackedPieces = new List<PlayerPiece>();
    public PlayerPiece parentPiece = null;

    private bool shortcutUsed = false;
    public PlayerState playerState; // �÷��̾� ���� ����

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

    // ������ �̵� �Լ� (�ڷ� �����̴�)
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

    // ������ ��
    private IEnumerator MoveAlongArcWithStacked(Vector3 start, Vector3 end, float duration, float arcHeight)
    {
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            float height = 4 * arcHeight * t * (1 - t);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += height;

            SetStackedPositions(pos); // ����� ��ġ ����ȭ

            time += Time.deltaTime;
            yield return null;
        }
        SetStackedPositions(end); // ������ ��ġ�� ���߱�
    }

    // �θ� ��ġ�� �������� ���� ������ ��ġ�ϴ� �Լ�
    private void SetStackedPositions(Vector3 basePos)
    {
        int count = stackedPieces.Count + 1; // �θ� ���� ��ü ����

        float offset = 1.2f; // ���� ����

        if (count == 1)
        {
            // 1��: �߾�
            transform.position = basePos;
        }
        else if (count == 2)
        {
            // 2��: ��/��� ��ġ
            transform.position = basePos + Vector3.left * offset / 2f;
            stackedPieces[0].transform.position = basePos + Vector3.right * offset / 2f;
        }
        else if (count == 3)
        {
            // 3��: ��(1), �Ʒ�(2) �ﰢ�� ����
            float yOffset = 0.18f; // ���� ����
            transform.position = basePos + new Vector3(0, 0, yOffset); // �� �߾�
            stackedPieces[0].transform.position = basePos + new Vector3(-offset / 2f, 0, -yOffset);
            stackedPieces[1].transform.position = basePos + new Vector3(offset / 2f, 0, -yOffset);
        }
        else if (count == 4)
        {
            // 4��: 2x2 ���簢��
            float x = offset / 2f;
            float z = offset / 2f;
            transform.position = basePos + new Vector3(-x, 0, z);
            stackedPieces[0].transform.position = basePos + new Vector3(x, 0, z);
            stackedPieces[1].transform.position = basePos + new Vector3(-x, 0, -z);
            stackedPieces[2].transform.position = basePos + new Vector3(x, 0, -z);
        }
        else
        {
            // 5�� �̻�: �Ϸ�(Ȥ�� �߰� Ȯ��)
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
            return; // �̹� �ڽ��̸� ���� �Ұ�
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
