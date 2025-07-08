using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public Toggle bgmToggle; // UI���� ������ Toggle
    public AudioSource bgmSource; // ������� AudioSource

    void Start()
    {
        if (bgmToggle != null)
        {
            // �ʱ� ���� ����
            bgmToggle.isOn = bgmSource.isPlaying;

            // Toggle �̺�Ʈ�� �޼��� ����
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
            bgmSource.Pause(); // �Ǵ� Stop()
        }
    }
}
