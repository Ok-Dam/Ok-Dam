using UnityEngine;
using UnityEngine.UI;

public class ShortcutDialogUI : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    private System.Action onYes;
    private System.Action onNo;

    // ���̾�α׸� ǥ���ϰ� �ݹ��� ���
    public void Show(System.Action yesAction, System.Action noAction, bool isLastShortcut)
    {
        gameObject.SetActive(true);
        onYes = yesAction;
        onNo = noAction;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);

        // ������ shortcut�̸� �� ��ư ����
        yesButton.gameObject.SetActive(!isLastShortcut);
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
