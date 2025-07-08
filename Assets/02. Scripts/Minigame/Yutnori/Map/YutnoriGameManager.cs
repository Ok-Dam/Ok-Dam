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
    [SerializeField] private GameUIManager gameUIManager; // �ϼ� ȭ�鿡 ���� ��
    public PointOfInterest startingNode;
    public PointOfInterest beforeEndNode;

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

        nodeManager = GetComponent<NodeManager>();
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
                { 
                    endPanelUI.Show();
                    endPanelUI.SetEndButtonText($"{turnCount}�� �ҿ�!");
                }
                break;
        }
    }
    public void ProcessYutResult(string result)
    {

        // 1. PlayerState�� ��� ���(���� ��ȭ �ϰ� ó��)
        CurrentPlayer.RegisterYutResult(result);

        // 2. UI ����
        gameUIManager.ShowYutResults(CurrentPlayer.yutResultIndices);

        // 3. ���� Ƚ���� ���� ���� �ܰ� ����
        if (CurrentPlayer.throwsLeft > 0)
        {
            setGameStage(GameStage.Throw); // ��� ������
                                           // �ʿ��ϴٸ� UI �ȳ�, ��ư Ȱ��ȭ ��
        }
        else
        {
            setGameStage(GameStage.Move); // Move �ܰ�� ��ȯ
                                          // ��/�� ���� �� Move �ܰ� ���� ����
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

            // ���� Ư�� ó��
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

        // �̹� ���� ���õǾ� �ִٸ� ��ü �� ���̶���Ʈ�� �ٽ� �� �ʿ� ����
        if (CurrentPlayer.piece != null)
        {
            HighlightReachableNodesBySelection();
        }
        else
        {
            // ���� ���� ���õ��� �ʾҴٸ�, ��� ���� ���̶���Ʈ�ؼ� ���� ����
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
                    // ��� �� ����: ������ Ŭ�� �� EndPOI ���� ���� �̵�
                    nodeManager.ClearHighlights();
                    StartCoroutine(CurrentPlayer.piece.MoveByBackdoPath(node.PreviousPointsOfInterest[0]));
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

    // GameManager.cs

    // �̵� �Ϸ� �� ȣ��Ǵ� �Լ�
    public void OnPieceMoveComplete()
    {
        // 1. ����� �� UI ����
        gameUIManager.DeleteYutResultImage(CurrentPlayer.selectedResultIndex);

        // 2. ���� ����Ʈ���� ����
        if (CurrentPlayer.selectedResultIndex >= 0 && CurrentPlayer.selectedResultIndex < CurrentPlayer.yutResultIndices.Count)
        {
            CurrentPlayer.yutResultIndices.RemoveAt(CurrentPlayer.selectedResultIndex);
            CurrentPlayer.selectedResultIndex = -1;
        }

        // 3. UI �簻�� (�߿�!)
        gameUIManager.ShowYutResults(CurrentPlayer.yutResultIndices);
    }



    // �� �Ѱܵ� �Ǵ��� Ȯ��
    void CheckTurnEndable(PlayerState player)
    {
        // 1. ����� �� ����
        if (player.selectedResultIndex >= 0 && player.selectedResultIndex < player.yutResultIndices.Count)
        {
            player.yutResultIndices.RemoveAt(player.selectedResultIndex);
            player.selectedResultIndex = -1;
        }

        // 2. ���� ��/���� ��ȸ �б�
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
        // (���� �÷��̾��� ���� �÷��̾�� �ε��� ����)
        // currentPlayerIndex = (currentPlayerIndex + 1) % playerStates.Length;
        CurrentPlayer.ResetTurn();           // �÷��̾� ����(�� ��� ��) �ʱ�ȭ
        gameUIManager.ClearYutResults();     // UI�� ǥ�õ� �� �̹��� �� �ʱ�ȭ
        setGameStage(GameStage.Throw);
        // (���⼭ �� ī��Ʈ UI ���� �� �߰� ó��)
    }

    public void HandleFinishPiece(PlayerPiece piece)
    {
        // playerPieces���� ����
        playerPieces.Remove(piece);

        piece.isFinished = true;

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

                    // currentNode.nodeNumber�� ����
                    quizManager.ShowQuizByNodeNumber(targetPiece.currentNode.nodeNumber, (isCorrect) =>
                    {
                        CheckTurnEndable(CurrentPlayer);
                    }, targetPiece.playerState);

                    break;
                }

        }
    }

}
