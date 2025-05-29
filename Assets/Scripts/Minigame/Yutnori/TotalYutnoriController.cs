using System.Linq;
using UnityEngine;
using Photon.Pun;

public class TotalYutnoriController : MonoBehaviour
{
    [Header("공유 오브젝트 (씬 상시 존재)")]
    public Camera explorationCamera;
    public GameObject miniMap;

    [Header("윷놀이 루트 프리팹")]
    public GameObject yutnoriRootPrefab;

    private GameObject yutnoriRootInstance;

    // 윷놀이 시작 버튼에서 호출
    public void OnYutnoriStartRequest()
    {
        // 1. 기존 인스턴스가 있으면 삭제
        if (yutnoriRootInstance != null)
            Destroy(yutnoriRootInstance);
        // 2. 프리팹에서 새로 생성
        yutnoriRootInstance = Instantiate(yutnoriRootPrefab);

        // 3. 윷놀이 루트 하위 오브젝트 동적 참조
        var yutnoriCamera = yutnoriRootInstance.transform.Find("YutnoriCamera")?.GetComponent<Camera>();
        var yutnoriUI = yutnoriRootInstance.transform.Find("YutnoriUI")?.gameObject;
        var boardRoot = yutnoriRootInstance.transform.Find("BoardRoot")?.gameObject;
        var gameManager = yutnoriRootInstance.GetComponentInChildren<YutnoriGameManager>(true)?.gameObject;
        var yuts = yutnoriRootInstance.transform.Find("Yuts")?.gameObject;
        var players = yutnoriRootInstance.transform.Find("Players")?.gameObject;

        // 4. 윷놀이 모드로 전환
        if (miniMap != null)
            miniMap.SetActive(false);
        if (explorationCamera != null)
            explorationCamera.gameObject.SetActive(false);
        if (yutnoriCamera != null)
            yutnoriCamera.gameObject.SetActive(true);
        if (yutnoriUI != null)
            yutnoriUI.SetActive(true);
        if (boardRoot != null)
            boardRoot.SetActive(true);
        if (gameManager != null)
            gameManager.SetActive(true);
        if (yuts != null)
            yuts.SetActive(true);
        if (players != null)
            players.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // (선택) 플레이어 컨트롤 비활성화 등 추가 로직 필요시 여기에
    }

    // 윷놀이 종료 버튼에서 호출
    public void OnYutnoriEndRequest()
    {
        // 1. 윷놀이 루트 하위 오브젝트 동적 참조
        var yutnoriCamera = yutnoriRootInstance?.transform.Find("YutnoriCamera")?.GetComponent<Camera>();
        var yutnoriUI = yutnoriRootInstance?.transform.Find("YutnoriUI")?.gameObject;
        var boardRoot = yutnoriRootInstance?.transform.Find("BoardRoot")?.gameObject;
        var gameManager = yutnoriRootInstance?.GetComponentInChildren<YutnoriGameManager>(true)?.gameObject;
        var yuts = yutnoriRootInstance?.transform.Find("Yuts")?.gameObject;
        var players = yutnoriRootInstance?.transform.Find("Players")?.gameObject;

        // 2. 모드 원상복구
        if (miniMap != null)
            miniMap.SetActive(true);
        if (explorationCamera != null)
            explorationCamera.gameObject.SetActive(true);
        if (yutnoriCamera != null)
            yutnoriCamera.gameObject.SetActive(false);
        if (yutnoriUI != null)
            yutnoriUI.SetActive(false);
        if (boardRoot != null)
            boardRoot.SetActive(false);
        if (gameManager != null)
            gameManager.SetActive(false);
        if (yuts != null)
            yuts.SetActive(false);
        if (players != null)
            players.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 3. 윷놀이 루트 오브젝트 삭제 (모든 하위 오브젝트 포함)
        if (yutnoriRootInstance != null)
        {
            Destroy(yutnoriRootInstance);
            yutnoriRootInstance = null;
        }

        // (선택) 플레이어 컨트롤 활성화 등 추가 로직 필요시 여기에
    }
}
