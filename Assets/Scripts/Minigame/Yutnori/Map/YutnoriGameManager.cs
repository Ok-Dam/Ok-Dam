using System.Collections.Generic;
using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End }

public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private ShortcutDialogUI shortcutDialogUI;
    [SerializeField] private EndPanelUI endPanelUI;
    [SerializeField] private List<PlayerPiece> playerPieces = new List<PlayerPiece>();
    private QuizManager quizManager;
    [SerializeField] private GameUIManager gameUIManager; // 턴수 화면에 띄우는 용

    // 현재 턴인 플레이어와 관련 정보(두 번 던지기 등)
    // 1인용: 단일 PlayerState만 사용
    public PlayerState playerState;
    //private int currentPlayerIndex = 0;
    public PlayerState CurrentPlayer => playerState;

    // 몇 턴이나 지났는지
    private int turnCount = 1;
    public int TurnCount => turnCount;

    public bool isDraggingYut { get; private set; }
    public void SetYutDragState(bool state) => isDraggingYut = state;

    void Start()
    {
        if (playerState == null)
            playerState = new PlayerState();

        gameUIManager.UpdateTurn(turnCount);

        quizManager = GetComponent<QuizManager>();

        // 모든 말에 같은 PlayerState를 할당
        for (int i = 0; i < playerPieces.Count; i++)
        {
            playerPieces[i].playerState = playerState;
        }
        setGameStage(GameStage.Throw);
    }
    void Update() { }

    public void setGameStage(GameStage stage)
    {
        this.stage = stage;
        switch (stage)
        {
            case GameStage.Start:
                break;
            case GameStage.Throw:
                HighlightAllPieces(false);
                break;
            case GameStage.Move:
                startMoveStage();
                break;
            case GameStage.End:
                endPanelUI.Show();
                Debug.Log("END");
                break;
        }
    }

    public void ProcessYutResult(string result)
    {
        // 테스트용: 어떤 결과든 무조건 빽도로 처리
        //result = "도";
        

        CurrentPlayer.currentYutResult = result;
        // 테스트용: 이동거리 증가
        CurrentPlayer.moveDistance = 8; return;

        switch (result)
        {
            case "도": CurrentPlayer.moveDistance = 1; break;
            case "개": CurrentPlayer.moveDistance = 2; break;
            case "걸": CurrentPlayer.moveDistance = 3; break;
            case "윷": CurrentPlayer.moveDistance = 4; CurrentPlayer.bonusThrowCount++; break;
            case "모": CurrentPlayer.moveDistance = 5; CurrentPlayer.bonusThrowCount++; break;
            case "빽도": CurrentPlayer.moveDistance = -1; break;
            default: CurrentPlayer.moveDistance = 0; break;
        }
        
    }
    public void startMoveStage()
    {
        HighlightAllPieces(true);
    }

    public void HighlightAllPieces(bool highlight)
    {
        foreach (var piece in playerPieces)
        {
            piece.Highlight(highlight);
        }
    }
    public void SelectPiece(PlayerPiece piece)
    {
        PlayerPiece root = piece;
        while (root.parentPiece != null)
            root = root.parentPiece;

        CurrentPlayer.piece = root;
        HighlightAllPieces(false);
        root.Highlight(true);

        // 빽도 특수 처리
        if (CurrentPlayer.moveDistance == -1)
            nodeManager.HighlightReachableNodes(root.currentNode, -1, root);
        else
            nodeManager.HighlightReachableNodes(root.currentNode, CurrentPlayer.moveDistance, root);
    }
    public void MoveSelectedPieceTo(PointOfInterest node)
    {
        if (CurrentPlayer.piece != null && CurrentPlayer.piece.parentPiece == null)
        {
            if (CurrentPlayer.moveDistance == -1)
            {
                if (!CurrentPlayer.piece.HasStarted())
                {
                    // 출발 전 빽도: 시작점 클릭 시 EndPOI 직전 노드로 이동
                    var mapGen = FindObjectOfType<MapGenerator>();
                    var beforeEndNodes = mapGen.GetNodesBeforeEndPOI();
                    var targetNode = beforeEndNodes[0];
                    nodeManager.ClearHighlights();
                    CurrentPlayer.piece.MoveTo(targetNode);
                    return;
                }
                else
                {
                    // 출발 후 빽도: 이전 노드(들)로 애니메이션 이동
                    nodeManager.ClearHighlights();
                    StartCoroutine(CurrentPlayer.piece.MoveByBackdoPath(node));
                    return;
                }
            }

            nodeManager.ClearHighlights();
            CurrentPlayer.piece.MoveTo(node);
        }
        else
        {
            Debug.Log("[MoveSelectedPieceTo] No valid root piece to move or piece is child");
        }
    }


    private void UseShortcut(PlayerPiece piece, PointOfInterest currentPoi)
    {
        PointOfInterest nextShortcut = FindNextShortcut(currentPoi, 10);
        Debug.Log(nextShortcut != null ? nextShortcut.name : "No shortcut found");

        if (nextShortcut != null)
        {
            piece.SetShortcutUsed(true);

            // 업힌 말(자식)이면 부모에서 분리
            if (piece.parentPiece != null)
            {
                piece.parentPiece.stackedPieces.Remove(piece);
                piece.parentPiece = null;
            }

            CurrentPlayer.piece = piece;
            MoveSelectedPieceTo(nextShortcut);
        }
    }

    private PointOfInterest FindNextShortcut(PointOfInterest start, int maxStep = 10)
    {
        var current = start;
        for (int i = 0; i < maxStep; i++)
        {
            if (current.Type == POIType.Shortcut && i != 0)
                return current;
            if (current.NextPointsOfInterest == null || current.NextPointsOfInterest.Count == 0)
                break;
            current = current.NextPointsOfInterest[0];
        }
        return null;
    }
    public void EndTurn()
    {
        // 보너스 던지기(윷/모/버프 등) 기회가 남아 있으면 턴 카운트 증가 없이 추가 던지기
        if (CurrentPlayer.bonusThrowCount > 0)
        {
            CurrentPlayer.bonusThrowCount--;
            setGameStage(GameStage.Throw);
        }
        else
        {
            turnCount++;
            gameUIManager.UpdateTurn(turnCount);
            // (여러 플레이어라면 다음 플레이어로 인덱스 변경)
            // currentPlayerIndex = (currentPlayerIndex + 1) % playerStates.Length;
            setGameStage(GameStage.Throw);
        }

        // (여기서 턴 카운트 UI 갱신 등 추가 처리)
    }

    public void HandleFinishPiece(PlayerPiece piece)
    {
        // playerPieces에서 제거
        playerPieces.Remove(piece);

        // 업기 구조 정리
        if (piece.parentPiece != null)
            piece.parentPiece.stackedPieces.Remove(piece);
        piece.parentPiece = null;
        foreach (var child in new List<PlayerPiece>(piece.stackedPieces))
        {
            child.parentPiece = null;
        }
        piece.stackedPieces.Clear();

        // 말 숨기기
        piece.gameObject.SetActive(false);

        // HUD에 이미지 추가
        var hudManager = FindObjectOfType<FinishHudManager>();
        if (hudManager != null)
            hudManager.AddFinishedPiece(piece.finishHudSprite);
    }

    public void interactByPOI(PlayerPiece piece, PointOfInterest poi)
    {
        // 업힌 말까지 포함해 상호작용 반복
        var allStacked = piece.GetAllStacked();

        switch (poi.Type)
        {
            case POIType.Start:
                break;
            case POIType.Component:
                setGameStage(GameStage.Throw);
                break;
            case POIType.Upgrade:
                setGameStage(GameStage.Throw);
                break;
            case POIType.Buff:
                {
                    int idx = 0;

                    void ShowNextQuiz()
                    {
                        if (idx >= allStacked.Count)
                        {
                            EndTurn();
                            return;
                        }

                        var targetPiece = allStacked[idx];
                        idx++;

                        if (targetPiece.playerState.nextBuffAutoSuccess)
                        {
                            Debug.Log($"[BuffPOI] {targetPiece.name}은(는) 버프 자동 성공");
                            targetPiece.playerState.ConsumeNextBuffAutoSuccess();
                        }
                        quizManager.ShowRandomQuiz((isCorrect) =>
                        {
                            // 퀴즈 결과 처리 후 다음 퀴즈 호출
                            ShowNextQuiz();
                        }, targetPiece.playerState);
                    }

                    ShowNextQuiz(); // 첫 퀴즈부터 시작
                    break;
                }
            case POIType.Shortcut:
                if (!piece.HasUsedShortcut())
                {
                    var nextShortcut = FindNextShortcut(poi);
                    bool isLastShortcut = (nextShortcut == null);

                    shortcutDialogUI.Show(
                        () => UseShortcut(piece, poi),
                        () => { EndTurn(); },
                        isLastShortcut
                    );
                }
                else EndTurn();
                break;
            case POIType.End:
                {
                    // 1. 완주할 모든 말 리스트 복사 (업힌 말 포함)
                    var finishedPieces = piece.GetAllStacked();

                    // 2. 모든 완주 말에 대해 GameManager의 완주 처리 호출
                    foreach (var finishedPiece in finishedPieces)
                    {
                        HandleFinishPiece(finishedPiece); // 아래에 구현
                    }

                    // 3. 모든 말 완주 여부 검사
                    if (playerPieces.Count == 0)
                    {
                        setGameStage(GameStage.End);
                    }
                    else
                    {
                        setGameStage(GameStage.Throw);
                    }

                    break;
                }
        }
    }

}
