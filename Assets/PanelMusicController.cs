using UnityEngine;

public class PanelMusicController : MonoBehaviour
{
    public GameObject targetPanel;     // ���� ��� ����ϴ� �г�
    public AudioSource bgmSource;      // ������� ������ҽ�

    private bool lastPanelState;

    void Start()
    {
        lastPanelState = targetPanel.activeSelf;

        // �ʱ� ���¿� ���� ���� ����
        if (!targetPanel.activeSelf)
        {
            bgmSource.Play();
        }
    }

    void Update()
    {
        bool currentPanelState = targetPanel.activeSelf;

        // ���°� �ٲ���� ���� ó��
        if (currentPanelState != lastPanelState)
        {
            if (currentPanelState) // �������� ���� ����
            {
                if (bgmSource.isPlaying)
                    bgmSource.Stop();
            }
            else // �������� ���� ���
            {
                if (!bgmSource.isPlaying)
                    bgmSource.Play();
            }

            lastPanelState = currentPanelState;
        }
    }
}
