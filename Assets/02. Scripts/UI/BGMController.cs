using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public Toggle bgmToggle; // UI에서 연결할 Toggle
    public AudioSource bgmSource; // 배경음악 AudioSource

    void Start()
    {
        if (bgmToggle != null)
        {
            // 초기 상태 설정
            bgmToggle.isOn = bgmSource.isPlaying;

            // Toggle 이벤트에 메서드 연결
            bgmToggle.onValueChanged.AddListener(OnToggleBGM);
        }
    }

    public void OnToggleBGM(bool isOn)
    {
        if (bgmSource == null) return;

        if (isOn)
        {
            if (!bgmSource.isPlaying)
                bgmSource.Play();
        }
        else
        {
            bgmSource.Pause(); // 또는 Stop()
        }
    }
}
