using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End }

public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private ShortcutDialogUI shortcutDialogUI;
    [SerializeField] private EndPanelUI endPanelUI;
    [SerializeField] private PlayerPiece[] playerPieces;
    private QuizManager quizManager;
    [SerializeField] private GameUIManager gameUIManager; // 턴수 화면에 띄우는 용

    // 현재 턴인 플레이어와 관련 정보(두 번 던지기 등)
    public PlayerState[] playerStates;
    private int currentPlayerIndex = 0;
    public PlayerState CurrentPlayer => playerStates[currentPlayerIndex];

    // 몇 턴이나 지났는지
    private int turnCount = 1;
    public int TurnCount => turnCount;

    public bool isDraggingYut { get; private set; }
    public void SetYutDragState(bool state) => isDraggingYut = state;

    void Start()
    {
        // PlayerState 배열이 null이거나 크기가 0이면 playerPieces.Length로 새로 생성
        if (playerStates == null || playerStates.Length != playerPieces.Length)
            playerStates = new PlayerState[playerPieces.Length];

        gameUIManager.UpdateTurn(turnCount); // ui 텍스트 초기화

        quizManager = GetComponent<QuizManager>();

        // 각 요소가 null이면 new PlayerState()로 생성
        for (int i = 0; i < playerStates.Length; i++)
        {
            if (playerStates[i] == null)
                playerStates[i] = new PlayerState();

            playerStates[i].piece = playerPieces[i];
            playerPieces[i].playerState = playerStates[i];
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
        CurrentPlayer.currentYutResult = result;
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
        CurrentPlayer.piece = piece;
        HighlightAllPieces(false);
        piece.Highlight(true);
        nodeManager.HighlightReachableNodes(piece.currentNode, CurrentPlayer.moveDistance);
    }

    public void MoveSelectedPieceTo(PointOfInterest node)
    {
        if (CurrentPlayer.piece != null)
        {
            nodeManager.ClearHighlights();
            CurrentPlayer.piece.MoveTo(node);
            // CurrentPlayer.piece = null; // 필요시 해제
        }
    }

    private void UseShortcut(PlayerPiece piece, PointOfInterest currentPoi)
    {
        PointOfInterest nextShortcut = FindNextShortcut(currentPoi, 10);
        Debug.Log(nextShortcut != null ? nextShortcut.name : "No shortcut found");

        if (nextShortcut != null)
        {
            piece.SetShortcutUsed(true);
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

    public void interactByPOI(PlayerPiece piece, PointOfInterest poi)
    {
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
                // 버프 자동 성공 버프 소멸 처리
                if (piece.playerState.nextBuffAutoSuccess)
                {
                    // 버프 무조건 지급 처리
                    piece.playerState.ConsumeNextBuffAutoSuccess();
                }
                quizManager.ShowRandomQuiz((isCorrect) =>
                {
                    // 퀴즈 결과에 따라 보너스 던지기 획득 등 처리됨
                    EndTurn(); // setGameStage(GameStage.Throw) 대신 EndTurn 호출
                }, piece.playerState);
                break;

            case POIType.Shortcut:
                if (!piece.HasUsedShortcut())
                {
                    var nextShortcut = FindNextShortcut(poi);
                    bool isLastShortcut = (nextShortcut == null);

                    shortcutDialogUI.Show(
                        () => UseShortcut(piece, poi),
                        () => { EndTurn(); }, // setGameStage(GameStage.Throw) 대신 EndTurn 호출
                        isLastShortcut
                    );
                }
                else EndTurn();
                break;
            case POIType.End:
                setGameStage(GameStage.End);
                break;
        }
    }
}
