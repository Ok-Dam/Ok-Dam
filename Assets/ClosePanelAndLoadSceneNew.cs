using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;         // ���� �г�
    public GameObject loadingPanel;         // "Loading ��..." �г�
    public string sceneToLoadName;          // �̵��� �� �̸�
    public float loadingDelay = 1.0f;       // �� �ε� �� ��� �ð� (��)

    public void CloseAndLoad()
    {
        StartCoroutine(CloseAndLoadCoroutine());
    }

    private IEnumerator CloseAndLoadCoroutine()
    {
        // 1. ���� �г� �ݱ�
        if (panelToClose != null)
            panelToClose.SetActive(false);

        // 2. �ε� �г� ���̱�
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // 3. ��� ��� (�ε� �޽��� ������ �ð� Ȯ��)
        yield return new WaitForSeconds(loadingDelay);

        // 4. �� �̵�
        SceneManager.LoadScene(sceneToLoadName);
    }
}

