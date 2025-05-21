using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OndolSimul : MonoBehaviour
{
    public GameObject parentObject; // 빈 오브젝트 A
    public float activationInterval = 1f; // 활성화 간격 (초)
    public Button ondolSimulationButton; // "Ondol Simulation" 버튼
    public GameObject panel; // 버튼이 포함된 패널

    void Start()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent Object is not assigned!");
            return;
        }

        // 빈 오브젝트 A 활성화
        parentObject.SetActive(true);

        // 버튼에 이벤트 리스너 등록
        if (ondolSimulationButton != null)
        {
            ondolSimulationButton.onClick.AddListener(StartOndolSimulation);
        }
        else
        {
            Debug.LogError("Ondol Simulation Button is not assigned!");
        }
    }

    public void StartOndolSimulation()
    {
        // 버튼이 포함된 패널 비활성화
        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogError("Panel is not assigned!");
        }

        // 자식 활성화 코루틴 시작
        StartCoroutine(ActivateChildrenSequentially());
    }

    IEnumerator ActivateChildrenSequentially()
    {
        // 자식 오브젝트만 가져오기 (비활성화된 자식도 포함)
        Transform[] children = new Transform[parentObject.transform.childCount];
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            children[i] = parentObject.transform.GetChild(i);
        }

        // 자식 오브젝트 전체 비활성화
        foreach (var child in children)
        {
            if (child != null)
            {
                child.gameObject.SetActive(false);
            }
        }

        // 1초 간격으로 자식 오브젝트 순차적 활성화
        for (int i = 0; i < children.Length; i++)
        {
            yield return new WaitForSeconds(activationInterval);

            if (children[i] != null)
            {
                children[i].gameObject.SetActive(true);
            }
        }

        // 자식 활성화가 끝난 후 패널 다시 활성화
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
}
