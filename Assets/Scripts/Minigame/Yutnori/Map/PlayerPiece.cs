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
        if (gameManager.stage == GameStage.Move)
            gameManager.SelectPiece(this);
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
        StartCoroutine(MoveByPath(destination));
    }

    private IEnumerator MoveByPath(PointOfInterest destination)
    {
        List<PointOfInterest> path = NodeManager.FindPath(currentNode, destination);
        if (path == null || path.Count < 2)
            yield break;

        // 버프: 이동거리 +1 적용 및 소멸
        if (playerState.nextMovePlus > 0)
        {
            playerState.moveDistance += playerState.nextMovePlus;
            playerState.ConsumeNextMovePlus();
        }

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = path[i - 1].transform.position + Vector3.up * 0.5f;
            Vector3 end = path[i].transform.position + Vector3.up * 0.5f;
            yield return MoveAlongArc(start, end, 0.4f, 4.0f);
            currentNode = path[i];
        }

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

    public void SetShortcutUsed(bool used) { shortcutUsed = used; }
    public bool HasUsedShortcut() { return shortcutUsed; }
}
