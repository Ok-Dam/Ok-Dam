using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;         // 닫을 패널
    public GameObject loadingPanel;         // "Loading 중..." 패널
    public string sceneToLoadName;          // 이동할 씬 이름
    public float loadingDelay = 1.0f;       // 씬 로딩 전 대기 시간 (초)

    public void CloseAndLoad()
    {
        StartCoroutine(CloseAndLoadCoroutine());
    }

    private IEnumerator CloseAndLoadCoroutine()
    {
        // 1. 기존 패널 닫기
        if (panelToClose != null)
            panelToClose.SetActive(false);

        // 2. 로딩 패널 보이기
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // 3. 잠시 대기 (로딩 메시지 보여줄 시간 확보)
        yield return new WaitForSeconds(loadingDelay);

        // 4. 씬 이동
        SceneManager.LoadScene(sceneToLoadName);
    }
}

