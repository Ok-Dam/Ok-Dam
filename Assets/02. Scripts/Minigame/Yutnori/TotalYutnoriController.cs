using System.Linq;
using UnityEngine;
using Photon.Pun;

public class TotalYutnoriController : MonoBehaviour
{
    [Header("���� ������Ʈ (�� ��� ����)")]
    public Camera explorationCamera;
    public GameObject miniMap;

    [Header("������ ��Ʈ ������")]
    public GameObject yutnoriRootPrefab;

    private GameObject yutnoriRootInstance;

    // ������ ���� ��ư���� ȣ��
    public void OnYutnoriStartRequest()
    {
        // 1. ���� �ν��Ͻ��� ������ ����
        if (yutnoriRootInstance != null)
            Destroy(yutnoriRootInstance);
        // 2. �����տ��� ���� ����
        yutnoriRootInstance = Instantiate(yutnoriRootPrefab);

        // 3. ������ ��Ʈ ���� ������Ʈ ���� ����
        var yutnoriCamera = yutnoriRootInstance.transform.Find("YutnoriCamera")?.GetComponent<Camera>();
        var yutnoriUI = yutnoriRootInstance.transform.Find("YutnoriUI")?.gameObject;
        var boardRoot = yutnoriRootInstance.transform.Find("BoardRoot")?.gameObject;
        var gameManager = yutnoriRootInstance.GetComponentInChildren<YutnoriGameManager>(true)?.gameObject;
        var yuts = yutnoriRootInstance.transform.Find("Yuts")?.gameObject;
        var players = yutnoriRootInstance.transform.Find("Players")?.gameObject;

        // 4. ������ ���� ��ȯ
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

        // (����) �÷��̾� ��Ʈ�� ��Ȱ��ȭ �� �߰� ���� �ʿ�� ���⿡
    }

    // ������ ���� ��ư���� ȣ��
    public void OnYutnoriEndRequest()
    {
        // 1. ������ ��Ʈ ���� ������Ʈ ���� ����
        var yutnoriCamera = yutnoriRootInstance?.transform.Find("YutnoriCamera")?.GetComponent<Camera>();
        var yutnoriUI = yutnoriRootInstance?.transform.Find("YutnoriUI")?.gameObject;
        var boardRoot = yutnoriRootInstance?.transform.Find("BoardRoot")?.gameObject;
        var gameManager = yutnoriRootInstance?.GetComponentInChildren<YutnoriGameManager>(true)?.gameObject;
        var yuts = yutnoriRootInstance?.transform.Find("Yuts")?.gameObject;
        var players = yutnoriRootInstance?.transform.Find("Players")?.gameObject;

        // 2. ��� ���󺹱�
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

        // 3. ������ ��Ʈ ������Ʈ ���� (��� ���� ������Ʈ ����)
        if (yutnoriRootInstance != null)
        {
            Destroy(yutnoriRootInstance);
            yutnoriRootInstance = null;
        }

        // (����) �÷��̾� ��Ʈ�� Ȱ��ȭ �� �߰� ���� �ʿ�� ���⿡
    }
}
