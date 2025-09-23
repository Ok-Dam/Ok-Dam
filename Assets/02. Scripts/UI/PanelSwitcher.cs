using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject currentPanel;  // 현재 보이는 패널 (PanelA)
    public GameObject nextPanel;     // 다음에 보일 패널 (PanelB)

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip uiOpenSound;
    public AudioClip narration;

    public void SwitchPanel()
    {
        currentPanel.SetActive(false); // 현재 패널 끄기
        nextPanel.SetActive(true);     // 다음 패널 켜기

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (audioSource != null)
        {
            if (uiOpenSound != null) audioSource.PlayOneShot(uiOpenSound);
            if (narration != null) audioSource.PlayOneShot(narration);
        }
    }

    public void ClosePanel()
    {
        nextPanel.SetActive(false);

        // 다시 게임 모드 -> 마우스 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
