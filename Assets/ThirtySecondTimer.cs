using UnityEngine;
using UnityEngine.UI;

public class ThirtySecondTimer : MonoBehaviour
{
    public Slider timerSlider;          // �����̴� ����
    public GameObject panelToActivate; // �ð��� ������ �� �г�

    private float timeLeft = 30f;      // ���� �ð� 30��

    void Start()
    {
        timerSlider.maxValue = timeLeft;
        timerSlider.value = timeLeft;

        if (panelToActivate != null)
            panelToActivate.SetActive(false); // ó���� ��Ȱ��ȭ
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerSlider.value = timeLeft;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                if (panelToActivate != null)
                    panelToActivate.SetActive(true); // �ð� ���� �� �г� Ȱ��ȭ
            }
        }
    }
}
