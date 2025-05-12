using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum GameStage { Start, Throw, Move, Interact, End}
public class YutnoriGameManager : MonoBehaviour
{
    public GameStage stage;
    [SerializeField] private NodeManager nodeManager;
    [SerializeField] private ShortcutDialogUI shortcutDialogUI;
    [SerializeField] private PlayerPiece[] playerPieces; // 플레이어 말 4개

    // 현재 윷 결과 저장
    private string currentYutResult = "";
    private int moveDistance = 0;
    public bool canThrowAgain = false;
    public bool canMove = false; // MoveStage에서도 POI 클릭 가능한 시기는 정해져 있어서 이걸로 제어

    // 선택된 말
    private PlayerPiece selectedPiece = null;

    public bool isDraggingYut { get; private set; } 
    public void SetYutDragState(bool state) => isDraggingYut = state; 

    // Start is called before the first frame update
    void Start()
    {
        setGameStage(GameStage.Throw);
        // YutController에서 윷 던지고 나서 결과값 얻고 GameStage을 Move로 바꿈
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setGameStage(GameStage stage) 
    {
        this.stage = stage;
        switch (stage)
        {
            case GameStage.Start:
                {
                    // 처음 시작시 
                    break;
                }
            case GameStage.Throw:
                {
                    HighlightAllPieces(false);
                    break;
                }
            case GameStage.Move:
                {
                    startMoveStage();
                    break;
                }
            case GameStage.End:
            {
                    Debug.Log("END");
                break;
            }
        }
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
    }
    public void startMoveStage() 
    {
        // 말 선택 단계로 변경
        HighlightAllPieces(true);
        // PlayerPiece에서 onMouseDown으로 SelectPiece 호출
        // SelectPiece는 nodeManager의 HighlightReachableNodes 호출
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
    private void UseShortcut(PlayerPiece piece, PointOfInterest currentPoi)
    {
        // 이미 지름길 사용 중인지 체크 (플래그 필요시)
        // 다음 지름길 노드 찾기 (예: currentPoi.NextPointsOfInterest 중 Type == Shortcut)
        PointOfInterest nextShortcut = FindNextShortcut(currentPoi, 10); // 10칸 이내에서 탐색
        Debug.Log(nextShortcut.name);


        if (nextShortcut != null)
        {
            // 중복 방지 플래그 세팅
            piece.SetShortcutUsed(true); 
            selectedPiece = piece;
            MoveSelectedPieceTo(nextShortcut);
        }
    }

    // N칸 뒤의 Shortcut 노드 찾기 (예시: 5칸 뒤)
    private PointOfInterest FindNextShortcut(PointOfInterest start, int maxStep = 10)
    {
        var current = start;
        for (int i = 0; i < maxStep; i++)
        {
            if (current.Type == POIType.Shortcut && i != 0)
                return current;
            if (current.NextPointsOfInterest == null || current.NextPointsOfInterest.Count == 0)
                break;
            current = current.NextPointsOfInterest[0]; // 분기 없는 단순 경로라면 [0]
        }
        return null;
    }

    public void interactByPOI(PlayerPiece piece, PointOfInterest poi)
    {
        switch (poi.Type)
        {
            case POIType.Start:
                break;
            case POIType.Component:
                // UI창 띄우기
                setGameStage(GameStage.Throw);
                break;
            case POIType.Upgrade:
                // 업그레이드 처리
                setGameStage(GameStage.Throw);
                break;
            case POIType.Buff:
                // 버프 선택
                setGameStage(GameStage.Throw);
                break;
            case POIType.Shortcut:
                {
                    if (!piece.HasUsedShortcut()) // 이번 턴에 지름길 안 썼으면
                        shortcutDialogUI.Show(
                            () => UseShortcut(piece, poi), // 예
                            () => { setGameStage(GameStage.Throw); } // 아니요
                        );
                    else setGameStage(GameStage.Throw); 
                break;
                }
            case POIType.End:
                setGameStage(GameStage.End);
                break;
                // ...
        }
    }
}
