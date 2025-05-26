using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberTimer : MonoBehaviour
{
    public Text timerText;
    public int startTime = 30;
    public GameObject failPanel;          // 실패 패널
    public GameObject objectToDeactivate; // 실패 시 숨길 오브젝트
    public GameObject startPanel;         // 다시 보여줄 Start Panel

    private int currentTime;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();

        if (failPanel != null)
            failPanel.SetActive(false);
        if (startPanel != null)
            startPanel.SetActive(false);
        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(true);

        InvokeRepeating("DecreaseTime", 1f, 1f);
    }

    void DecreaseTime()
    {
        if (currentTime > 0)
        {
            currentTime--;
            UpdateTimerText();
        }
        else
        {
            CancelInvoke("DecreaseTime");
            StartCoroutine(ShowFailThenReturn());
        }
    }

    IEnumerator ShowFailThenReturn()
    {
        // 1. 실패 패널 보여주기
        if (failPanel != null)
            failPanel.SetActive(true);

        // 2. 다른 오브젝트 숨기기
        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(false);

        // 3. 3초 대기
        yield return new WaitForSeconds(3f);

        // 4. 실패 패널 숨기고 START 패널로 전환
        if (failPanel != null)
            failPanel.SetActive(false);
        if (startPanel != null)
            startPanel.SetActive(true);
    }

    void UpdateTimerText()
    {
        timerText.text = currentTime.ToString();
    }
}
