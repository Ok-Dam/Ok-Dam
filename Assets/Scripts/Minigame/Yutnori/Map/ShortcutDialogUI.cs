using UnityEngine;
using UnityEngine.UI;

public class ShortcutDialogUI : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    private System.Action onYes;
    private System.Action onNo;

    // 다이얼로그를 표시하고 콜백을 등록
    public void Show(System.Action yesAction, System.Action noAction)
    {
        gameObject.SetActive(true);
        onYes = yesAction;
        onNo = noAction;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void OnYesClicked()
    {
        gameObject.SetActive(false);
        onYes?.Invoke();
    }

    private void OnNoClicked()
    {
        gameObject.SetActive(false);
        onNo?.Invoke();
    }
}
