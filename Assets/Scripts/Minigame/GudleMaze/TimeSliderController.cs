using UnityEngine;
using UnityEngine.UI;

public class TimerSliderController : MonoBehaviour
{
    public Slider timerSlider;          // �ð� ǥ�ÿ� �����̴�
    public float duration = 10f;        // Ÿ�̸� �� �ð� (��)
    public GameObject panelToActivate; // �ð��� ������ Ȱ��ȭ�� �г�

    private float currentTime;

    void Start()
    {
        currentTime = duration;
        timerSlider.maxValue = duration;
        timerSlider.value = duration;

        if (panelToActivate != null)
            panelToActivate.SetActive(false);  // ó���� ��Ȱ��ȭ
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerSlider.value = currentTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                if (panelToActivate != null)
                    panelToActivate.SetActive(true); // �ð� ����Ǹ� �г� Ȱ��ȭ
            }
        }
    }
}
