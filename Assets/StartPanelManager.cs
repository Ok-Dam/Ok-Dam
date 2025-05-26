using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanelManager : MonoBehaviour
{
    public GameObject startPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // UI 클릭 가능하게
        Cursor.visible = true;
    }

    public void OnStartButtonClicked()
    {
        // 패널 닫기
        startPanel.SetActive(false);

        // 마우스 잠금 후 본격 플레이
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 또는 씬 전환
        // SceneManager.LoadScene("MainScene");
    }
}
