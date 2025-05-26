using UnityEngine;
using UnityEngine.UI;

public class TimerAndPanelActivator : MonoBehaviour
{
    public float timeRemaining = 30f;        // 타이머 시작 시간 (초)
    public GameObject panelToActivate;       // 시간이 0이 되면 활성화할 패널
    public Text timerText;                   // UI에 타이머를 표시할 텍스트 (선택 사항)

    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerRunning = false;

                if (panelToActivate != null)
                    panelToActivate.SetActive(true);
            }

            // 타이머 텍스트가 있으면 UI에 시간 표시
            if (timerText != null)
                timerText.text = Mathf.Ceil(timeRemaining).ToString();
        }
    }
}
