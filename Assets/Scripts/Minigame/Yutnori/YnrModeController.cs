using System.Linq;
using UnityEngine;
using Photon.Pun;

public class YnrModeController : MonoBehaviour
{
    public Camera explorationCamera;
    public Camera yutnoriCamera;
    public GameObject yutnoriUI;
    public GameObject miniMap;
    public GameObject boardRoot;
    public GameObject gameManager;
    public GameObject yuts;
    public GameObject players; // 말. 한옥 맵 상의 플레이어 x

    // UI 버튼에서 이 메서드만 호출하게 
    public void OnYutnoriStartRequest()
    {
        // 내 플레이어 찾기 
        var localPlayer = FindObjectsOfType<PhotonView>()
            .FirstOrDefault(pv => pv.IsMine && pv.CompareTag("Player"))?.gameObject;
        if (localPlayer != null)
            EnterYutnoriMode(localPlayer);
        yutnoriCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void EnterYutnoriMode(GameObject player)
    {
        // 모든 스크립트/콜라이더/카메라/미니맵 등 제어
        foreach (var script in player.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script is PhotonView || script is Animator) continue;
            script.enabled = false;
        }
        foreach (var col in player.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        foreach (var rb in player.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }

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
        if (gameManager != null) gameManager.SetActive(true);
        if (yuts != null) yuts.SetActive(true);
        if (players != null) players.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnYutnoriEndRequest()
    {
        // 내 플레이어 찾기
        var localPlayer = FindObjectsOfType<PhotonView>()
            .FirstOrDefault(pv => pv.IsMine && pv.CompareTag("Player"))?.gameObject;
        if (localPlayer != null)
            ExitYutnoriMode(localPlayer);

        yutnoriCamera.GetComponent<AudioListener>().enabled = false;
    }

    public void ExitYutnoriMode(GameObject player)
    {
        // 1. 플레이어 컨트롤, 카메라, 미니맵, UI, 보드 등 원상복구
        foreach (var script in player.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script is PhotonView || script is Animator) continue;
            script.enabled = true; // 다시 활성화
        }
        foreach (var col in player.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        foreach (var rb in player.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }

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
        if (gameManager != null) gameManager.SetActive(false);
        if (yuts != null) yuts.SetActive(false);
        if (players != null) players.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // (선택) 윷놀이 중 생성된 임시 오브젝트/하이라이트/이펙트 등도 정리 필요
    }
}
