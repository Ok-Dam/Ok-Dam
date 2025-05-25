using UnityEngine;
using UnityEngine.UI;

public class TimerSliderController : MonoBehaviour
{
    public Slider timerSlider;          // 시간 표시용 슬라이더
    public float duration = 10f;        // 타이머 총 시간 (초)
    public GameObject panelToActivate; // 시간이 끝나면 활성화할 패널

    private float currentTime;

    void Start()
    {
        currentTime = duration;
        timerSlider.maxValue = duration;
        timerSlider.value = duration;

        if (panelToActivate != null)
            panelToActivate.SetActive(false);  // 처음엔 비활성화
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
                    panelToActivate.SetActive(true); // 시간 종료되면 패널 활성화
            }
        }
    }
}
