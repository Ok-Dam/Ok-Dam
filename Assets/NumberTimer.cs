using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberTimer : MonoBehaviour
{
    public Text timerText;
    public int startTime = 30;
    public GameObject failPanel;          // ���� �г�
    public GameObject objectToDeactivate; // ���� �� ���� ������Ʈ
    public GameObject startPanel;         // �ٽ� ������ Start Panel

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
        // 1. ���� �г� �����ֱ�
        if (failPanel != null)
            failPanel.SetActive(true);

        // 2. �ٸ� ������Ʈ �����
        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(false);

        // 3. 3�� ���
        yield return new WaitForSeconds(3f);

        // 4. ���� �г� ����� START �гη� ��ȯ
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
