using UnityEngine;
using UnityEngine.UI;

public class TimerAndPanelActivator : MonoBehaviour
{
    public float timeRemaining = 30f;        // Ÿ�̸� ���� �ð� (��)
    public GameObject panelToActivate;       // �ð��� 0�� �Ǹ� Ȱ��ȭ�� �г�
    public Text timerText;                   // UI�� Ÿ�̸Ӹ� ǥ���� �ؽ�Ʈ (���� ����)

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

            // Ÿ�̸� �ؽ�Ʈ�� ������ UI�� �ð� ǥ��
            if (timerText != null)
                timerText.text = Mathf.Ceil(timeRemaining).ToString();
        }
    }
}
