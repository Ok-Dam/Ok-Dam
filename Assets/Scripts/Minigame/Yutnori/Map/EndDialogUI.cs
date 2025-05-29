using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ����

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
        // ���� �� �ٽ� �ε�
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
