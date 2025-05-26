using System.Collections;
using UnityEngine;

public class SequentialUI : MonoBehaviour
{
    public GameObject[] uiElements;  // 1~5번 UI 오브젝트
    public GameObject panel;         // 마지막에 나타날 PANEL 오브젝트
    private float[] activationTimes = { 8f, 12f, 15f, 24f, 32f };

    void Start()
    {
        // 모든 UI와 패널을 비활성화
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

        // 마지막 UI (5번째) 비활성화 + PANEL 활성화 (3초 후)
        yield return new WaitForSeconds(3f);
        if (previous != null)
            previous.SetActive(false);

        panel.SetActive(true);
    }
}
