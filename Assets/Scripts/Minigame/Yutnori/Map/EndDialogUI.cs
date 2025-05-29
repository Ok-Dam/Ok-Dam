using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용시

public class EndPanelUI : MonoBehaviour
{
    public Button restartButton;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnEndButton()
    {
        // 현재 씬 다시 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
