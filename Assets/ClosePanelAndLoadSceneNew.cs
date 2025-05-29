using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;
using static MapSceneInitializer;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;         // ���� �г�
    public GameObject loadingPanel;         // "Loading ��..." �г�
    public string sceneToLoadName;          // �̵��� �� �̸�
    public float loadingDelay = 1.0f;       // �� �ε� �� ��� �ð� (��)

    public void CloseAndLoad()
    {
        Debug.Log("�� ��ȯ �õ� ��...");
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            StartCoroutine(CloseAndLoadCoroutine());
        }
        else
        {
            Debug.LogWarning("Photon�� ������� �ʾƼ� �� ��ȯ �ȵ�.");
        }
    }

    private IEnumerator CloseAndLoadCoroutine()
    {


        // 2. �ε� �г� ���̱�
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // 3. ��� ��� (�ε� �޽��� ������ �ð� Ȯ��)
        yield return new WaitForSeconds(loadingDelay);

        Debug.Log("�� �̸� Ȯ��: " + sceneToLoadName);
        MapSceneInitializer.SetRestartFlag();
        GameStateManager.isReturningFromMiniGame = true;
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Single);
    }
}

