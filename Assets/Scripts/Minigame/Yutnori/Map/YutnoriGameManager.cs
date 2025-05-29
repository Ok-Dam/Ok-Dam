using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End }

public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;
    private NodeManager nodeManager;
    //[SerializeField] private ShortcutDialogUI shortcutDialogUI;
    [SerializeField] private EndPanelUI endPanelUI;
    [SerializeField] private List<PlayerPiece> playerPieces = new List<PlayerPiece>();
    private QuizManager quizManager;
    [SerializeField] private GameUIManager gameUIManager; // 턴수 화면에 띄우는 용
    public PointOfInterest startingNode;
    public PointOfInterest beforeEndNode;

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

        nodeManager = GetComponent<NodeManager>();
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
                { 
                    endPanelUI.Show();
                    endPanelUI.SetEndButtonText($"{turnCount}턴 소요!");
                }
                break;
        }
    }
    public void ProcessYutResult(string result)
    {

        // 1. PlayerState에 결과 등록(상태 변화 일괄 처리)
        CurrentPlayer.RegisterYutResult(result);

        // 2. UI 갱신
        gameUIManager.ShowYutResults(CurrentPlayer.yutResultIndices);

        // 3. 던질 횟수에 따라 다음 단계 결정
        if (CurrentPlayer.throwsLeft > 0)
        {
            setGameStage(GameStage.Throw); // 계속 던지기
                                           // 필요하다면 UI 안내, 버튼 활성화 등
        }
        else
        {
            setGameStage(GameStage.Move); // Move 단계로 전환
                                          // 패/말 선택 등 Move 단계 로직 시작
        }
    }

    public void startMoveStage()
    {
        HighlightAllPieces(true);
    }
    public void SelectPiece(PlayerPiece piece)
    {
        PlayerPiece root = piece;
        while (root.parentPiece != null)
            root = root.parentPiece;

        CurrentPlayer.piece = root;

        HighlightAllPieces(false);
        root.Highlight(true);

        if (CurrentPlayer.selectedResultIndex >= 0)
        {
            int moveDist = CurrentPlayer.GetSelectedMoveDistance();

            // 빽도 특수 처리
            if (moveDist == -1)
            {
                nodeManager.HighlightReachableNodes(root.currentNode, -1, root);
            }
            else
            {
                nodeManager.HighlightReachableNodes(root.currentNode, moveDist, root);
            }
        }
        else
        {
            nodeManager.ClearHighlights();
        }
    }

    public void OnYutResultSelected(int selectedIndex)
    {
        CurrentPlayer.selectedResultIndex = selectedIndex;

        // 이미 말이 선택되어 있다면 전체 말 하이라이트를 다시 할 필요 없음
        if (CurrentPlayer.piece != null)
        {
            HighlightReachableNodesBySelection();
        }
        else
        {
            // 말이 아직 선택되지 않았다면, 모든 말을 하이라이트해서 선택 유도
            HighlightAllPieces(true);
            nodeManager.ClearHighlights();
        }
    }

    private void HighlightAllPieces(bool highlight)
    {
        foreach (var p in playerPieces)
            p.Highlight(highlight);
    }

    private void HighlightReachableNodesBySelection()
    {
        nodeManager.ClearHighlights();
        int moveDist = CurrentPlayer.GetSelectedMoveDistance();
        nodeManager.HighlightReachableNodes(CurrentPlayer.piece.currentNode, moveDist, CurrentPlayer.piece);
    }



    public void MoveSelectedPieceTo(PointOfInterest node)
    {
        if (CurrentPlayer.piece != null && CurrentPlayer.piece.parentPiece == null)
        {
            if (CurrentPlayer.GetSelectedMoveDistance() == -1)
            {
                if (!CurrentPlayer.piece.HasStarted())
                {
                    // 출발 전 빽도: 시작점 클릭 시 EndPOI 직전 노드로 이동
                    nodeManager.ClearHighlights();
                    StartCoroutine(CurrentPlayer.piece.MoveByBackdoPath(node.PreviousPointsOfInterest[0]));
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

    // GameManager.cs

    // 이동 완료 후 호출되는 함수
    public void OnPieceMoveComplete()
    {
        // 1. 사용한 패 UI 삭제
        gameUIManager.DeleteYutResultImage(CurrentPlayer.selectedResultIndex);

        // 2. 내부 리스트에서 삭제
        if (CurrentPlayer.selectedResultIndex >= 0 && CurrentPlayer.selectedResultIndex < CurrentPlayer.yutResultIndices.Count)
        {
            CurrentPlayer.yutResultIndices.RemoveAt(CurrentPlayer.selectedResultIndex);
            CurrentPlayer.selectedResultIndex = -1;
        }

        // 3. UI 재갱신 (중요!)
        gameUIManager.ShowYutResults(CurrentPlayer.yutResultIndices);
    }



    // 턴 넘겨도 되는지 확인
    void CheckTurnEndable(PlayerState player)
    {
        // 1. 사용한 패 삭제
        if (player.selectedResultIndex >= 0 && player.selectedResultIndex < player.yutResultIndices.Count)
        {
            player.yutResultIndices.RemoveAt(player.selectedResultIndex);
            player.selectedResultIndex = -1;
        }

        // 2. 남은 패/던질 기회 분기
        if (player.yutResultIndices.Count > 0)
        {
            setGameStage(GameStage.Move);
            return;
        }
        else if (player.throwsLeft > 0)
        {
            setGameStage(GameStage.Throw);
        }
        else
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
            turnCount++;
            gameUIManager.UpdateTurn(turnCount);
        // (여러 플레이어라면 다음 플레이어로 인덱스 변경)
        // currentPlayerIndex = (currentPlayerIndex + 1) % playerStates.Length;
        CurrentPlayer.ResetTurn();           // 플레이어 상태(패 기록 등) 초기화
        gameUIManager.ClearYutResults();     // UI에 표시된 패 이미지 등 초기화
        setGameStage(GameStage.Throw);
        // (여기서 턴 카운트 UI 갱신 등 추가 처리)
    }

    public void HandleFinishPiece(PlayerPiece piece)
    {
        // playerPieces에서 제거
        playerPieces.Remove(piece);

        piece.isFinished = true;

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
                        
                        CheckTurnEndable(CurrentPlayer);
                    }

                    break;
                }
            case POIType.Yard:
                CheckTurnEndable(CurrentPlayer);
                break;
            case POIType.Event:
                CheckTurnEndable(CurrentPlayer);
                break;
            case POIType.Room:
                {
                    var targetPiece = allStacked[0];

                    // currentNode.nodeNumber를 전달
                    quizManager.ShowQuizByNodeNumber(targetPiece.currentNode.nodeNumber, (isCorrect) =>
                    {
                        CheckTurnEndable(CurrentPlayer);
                    }, targetPiece.playerState);

                    break;
                }

        }
    }

}
