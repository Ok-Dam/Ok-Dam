using System.Collections;
using UnityEngine;

public class SequentialUI : MonoBehaviour
{
    public GameObject[] uiElements;  // 1~5�� UI ������Ʈ
    public GameObject panel;         // �������� ��Ÿ�� PANEL ������Ʈ
    private float[] activationTimes = { 8f, 12f, 15f, 24f, 32f };

    void Start()
    {
        // ��� UI�� �г��� ��Ȱ��ȭ
        foreach (var ui in uiElements)
            ui.SetActive(false);
        panel.SetActive(false);

        StartCoroutine(ActivateSequence());
    }

    IEnumerator ActivateSequence()
    {
        GameObject previous = null;

        for (int i = 0; i < uiElements.Length && i < activationTimes.Length; i++)
        {
            float waitTime = i == 0 ? activationTimes[i] : activationTimes[i] - activationTimes[i - 1];
            yield return new WaitForSeconds(waitTime);

            if (previous != null)
                previous.SetActive(false);

            uiElements[i].SetActive(true);
            previous = uiElements[i];
        }

        // ������ UI (5��°) ��Ȱ��ȭ + PANEL Ȱ��ȭ (3�� ��)
        yield return new WaitForSeconds(3f);
        if (previous != null)
            previous.SetActive(false);

        panel.SetActive(true);
    }
}
