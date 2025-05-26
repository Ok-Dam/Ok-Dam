using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanelManager : MonoBehaviour
{
    public GameObject startPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // UI Ŭ�� �����ϰ�
        Cursor.visible = true;
    }

    public void OnStartButtonClicked()
    {
        // �г� �ݱ�
        startPanel.SetActive(false);

        // ���콺 ��� �� ���� �÷���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �Ǵ� �� ��ȯ
        // SceneManager.LoadScene("MainScene");
    }
}
