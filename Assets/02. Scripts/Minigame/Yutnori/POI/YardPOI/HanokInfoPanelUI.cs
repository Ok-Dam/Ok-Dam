using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HanokInfoPanelUI : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Image infoImage;
    public Button closeButton;

    private System.Action onClose; // 닫기 콜백(턴 종료 등)

    // 정보 표시 및 패널 활성화
    public void Show(string text, Sprite image, System.Action onCloseAction)
    {
        infoText.text = text;
        if (image != null)
        {
            infoImage.sprite = image;
            infoImage.gameObject.SetActive(true);
        }
        else
        {
            infoImage.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);

        onClose = onCloseAction;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
            onClose?.Invoke();
        });
    }
}
