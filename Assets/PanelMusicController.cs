using UnityEngine;

public class PanelMusicController : MonoBehaviour
{
    public GameObject targetPanel;     // 음악 제어에 사용하는 패널
    public AudioSource bgmSource;      // 배경음악 오디오소스

    private bool lastPanelState;

    void Start()
    {
        lastPanelState = targetPanel.activeSelf;

        // 초기 상태에 따라 음악 제어
        if (!targetPanel.activeSelf)
        {
            bgmSource.Play();
        }
    }

    void Update()
    {
        bool currentPanelState = targetPanel.activeSelf;

        // 상태가 바뀌었을 때만 처리
        if (currentPanelState != lastPanelState)
        {
            if (currentPanelState) // 열렸으면 음악 정지
            {
                if (bgmSource.isPlaying)
                    bgmSource.Stop();
            }
            else // 닫혔으면 음악 재생
            {
                if (!bgmSource.isPlaying)
                    bgmSource.Play();
            }

            lastPanelState = currentPanelState;
        }
    }
}
