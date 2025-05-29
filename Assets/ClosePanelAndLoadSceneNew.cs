using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;
using static MapSceneInitializer;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;         // 닫을 패널
    public GameObject loadingPanel;         // "Loading 중..." 패널
    public string sceneToLoadName;          // 이동할 씬 이름
    public float loadingDelay = 1.0f;       // 씬 로딩 전 대기 시간 (초)

    public void CloseAndLoad()
    {
        Debug.Log("씬 전환 시도 중...");
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            StartCoroutine(CloseAndLoadCoroutine());
        }
        else
        {
            Debug.LogWarning("Photon에 연결되지 않아서 씬 전환 안됨.");
        }
    }

    private IEnumerator CloseAndLoadCoroutine()
    {


        // 2. 로딩 패널 보이기
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // 3. 잠시 대기 (로딩 메시지 보여줄 시간 확보)
        yield return new WaitForSeconds(loadingDelay);

        Debug.Log("씬 이름 확인: " + sceneToLoadName);
        MapSceneInitializer.SetRestartFlag();
        GameStateManager.isReturningFromMiniGame = true;
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Single);
    }
}

