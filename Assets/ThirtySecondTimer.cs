using UnityEngine;
using UnityEngine.UI;

public class ThirtySecondTimer : MonoBehaviour
{
    public Slider timerSlider;          // 슬라이더 연결
    public GameObject panelToActivate; // 시간이 끝나면 켤 패널

    private float timeLeft = 30f;      // 제한 시간 30초

    void Start()
    {
        timerSlider.maxValue = timeLeft;
        timerSlider.value = timeLeft;

        if (panelToActivate != null)
            panelToActivate.SetActive(false); // 처음엔 비활성화
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
                    panelToActivate.SetActive(true); // 시간 종료 시 패널 활성화
            }
        }
    }
}
