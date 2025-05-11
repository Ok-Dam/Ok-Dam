using System.Collections;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    [SerializeField] private YutnoriGameManager gameManager;

    [SerializeField] private Highlighter highlighter;

    // 말의 현재 위치 (맵에 아직 없으면 null)
    public PointOfInterest currentNode { get; private set; }

    private Coroutine blinkCoroutine;

    void Start()
    {
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
        // 말 이동 애니메이션 (간단히 구현)
        StartCoroutine(MoveAnimation(destination));
    }

    private IEnumerator MoveAnimation(PointOfInterest destination)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = destination.transform.position + Vector3.up * 0.5f; // 노드 위에 위치
        float duration = 0.5f;
        float time = 0;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        currentNode = destination;

        // 이동 완료 후 다음 단계로
        if (gameManager.canThrowAgain)
            gameManager.setGameStage(GameStage.Throw); // 한 번 더 던지기
        else
            gameManager.setGameStage(GameStage.Interact); // 다음 플레이어 턴
    }
}
