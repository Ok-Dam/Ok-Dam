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
    [SerializeField] private GameUIManager gameUIManager; // �ϼ� ȭ�鿡 ���� ��

    // ���� ���� �÷��̾�� ���� ����(�� �� ������ ��)
    // 1�ο�: ���� PlayerState�� ���
    public PlayerState playerState;
    //private int currentPlayerIndex = 0;
    public PlayerState CurrentPlayer => playerState;

    // �� ���̳� ��������
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

        // ��� ���� ���� PlayerState�� �Ҵ�
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
        // �׽�Ʈ��: � ����� ������ ������ ó��
        //result = "��";
        

        CurrentPlayer.currentYutResult = result;
        // �׽�Ʈ��: �̵��Ÿ� ����
        CurrentPlayer.moveDistance = 8; return;

        switch (result)
        {
            case "��": CurrentPlayer.moveDistance = 1; break;
            case "��": CurrentPlayer.moveDistance = 2; break;
            case "��": CurrentPlayer.moveDistance = 3; break;
            case "��": CurrentPlayer.moveDistance = 4; CurrentPlayer.bonusThrowCount++; break;
            case "��": CurrentPlayer.moveDistance = 5; CurrentPlayer.bonusThrowCount++; break;
            case "����": CurrentPlayer.moveDistance = -1; break;
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

        // ���� Ư�� ó��
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
                    // ��� �� ����: ������ Ŭ�� �� EndPOI ���� ���� �̵�
                    var mapGen = FindObjectOfType<MapGenerator>();
                    var beforeEndNodes = mapGen.GetNodesBeforeEndPOI();
                    var targetNode = beforeEndNodes[0];
                    nodeManager.ClearHighlights();
                    CurrentPlayer.piece.MoveTo(targetNode);
                    return;
                }
                else
                {
                    // ��� �� ����: ���� ���(��)�� �ִϸ��̼� �̵�
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

            // ���� ��(�ڽ�)�̸� �θ𿡼� �и�
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
        // ���ʽ� ������(��/��/���� ��) ��ȸ�� ���� ������ �� ī��Ʈ ���� ���� �߰� ������
        if (CurrentPlayer.bonusThrowCount > 0)
        {
            CurrentPlayer.bonusThrowCount--;
            setGameStage(GameStage.Throw);
        }
        else
        {
            turnCount++;
            gameUIManager.UpdateTurn(turnCount);
            // (���� �÷��̾��� ���� �÷��̾�� �ε��� ����)
            // currentPlayerIndex = (currentPlayerIndex + 1) % playerStates.Length;
            setGameStage(GameStage.Throw);
        }

        // (���⼭ �� ī��Ʈ UI ���� �� �߰� ó��)
    }

    public void HandleFinishPiece(PlayerPiece piece)
    {
        // playerPieces���� ����
        playerPieces.Remove(piece);

        // ���� ���� ����
        if (piece.parentPiece != null)
            piece.parentPiece.stackedPieces.Remove(piece);
        piece.parentPiece = null;
        foreach (var child in new List<PlayerPiece>(piece.stackedPieces))
        {
            child.parentPiece = null;
        }
        piece.stackedPieces.Clear();

        // �� �����
        piece.gameObject.SetActive(false);

        // HUD�� �̹��� �߰�
        var hudManager = FindObjectOfType<FinishHudManager>();
        if (hudManager != null)
            hudManager.AddFinishedPiece(piece.finishHudSprite);
    }

    public void interactByPOI(PlayerPiece piece, PointOfInterest poi)
    {
        // ���� ������ ������ ��ȣ�ۿ� �ݺ�
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
                            Debug.Log($"[BuffPOI] {targetPiece.name}��(��) ���� �ڵ� ����");
                            targetPiece.playerState.ConsumeNextBuffAutoSuccess();
                        }
                        quizManager.ShowRandomQuiz((isCorrect) =>
                        {
                            // ���� ��� ó�� �� ���� ���� ȣ��
                            ShowNextQuiz();
                        }, targetPiece.playerState);
                    }

                    ShowNextQuiz(); // ù ������� ����
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
                    // 1. ������ ��� �� ����Ʈ ���� (���� �� ����)
                    var finishedPieces = piece.GetAllStacked();

                    // 2. ��� ���� ���� ���� GameManager�� ���� ó�� ȣ��
                    foreach (var finishedPiece in finishedPieces)
                    {
                        HandleFinishPiece(finishedPiece); // �Ʒ��� ����
                    }

                    // 3. ��� �� ���� ���� �˻�
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
