using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End}
public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private PlayerPiece[] playerPieces; // 플레이어 말 4개

    // 현재 윷 결과 저장
    private string currentYutResult = "";
    private int moveDistance = 0;
    public bool canThrowAgain = false;

    // 선택된 말
    private PlayerPiece selectedPiece = null;

    public bool isDraggingYut { get; private set; } 
    public void SetYutDragState(bool state) => isDraggingYut = state; 

    // Start is called before the first frame update
    void Start()
    {
        setGameStage(GameStage.Interact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGameStage(GameStage stage) 
    {
        this.stage = stage; 
    }

    public void ProcessYutResult(string result)
    {
        currentYutResult = result;

        // 결과에 따른 이동 거리 설정
        switch (result)
        {
            case "도": moveDistance = 1; canThrowAgain = false; break;
            case "개": moveDistance = 2; canThrowAgain = false; break;
            case "걸": moveDistance = 3; canThrowAgain = false; break;
            case "윷": moveDistance = 4; canThrowAgain = true; break;
            case "모": moveDistance = 5; canThrowAgain = true; break;
            case "빽도": moveDistance = -1; canThrowAgain = false; break;
            default: moveDistance = 0; canThrowAgain = false; break;
        }

        // 말 선택 단계로 변경
        HighlightAllPieces(true);
    }

    // 모든 말 하이라이트
    public void HighlightAllPieces(bool highlight)
    {
        foreach (var piece in playerPieces)
        {
            piece.Highlight(highlight);
        }
    }

    // 말 선택
    public void SelectPiece(PlayerPiece piece)
    {
        selectedPiece = piece;
        HighlightAllPieces(false);
        piece.Highlight(true);

        // NodeManager에게 경로 탐색 및 하이라이트 요청
        nodeManager.HighlightReachableNodes(piece.currentNode, moveDistance);
    }

    // 선택된 말을 노드로 이동
    public void MoveSelectedPieceTo(PointOfInterest node)
    {
        if (selectedPiece != null)
        {
            nodeManager.ClearHighlights(); // NodeManager에게 하이라이트 해제 요청
            selectedPiece.MoveTo(node);
            selectedPiece = null;
        }
    }

}
