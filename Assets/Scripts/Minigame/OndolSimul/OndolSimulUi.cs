using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OndolSimulUi : MonoBehaviour
{
    private Transform[] children;
    private int currentIndex = -1; // 초기값을 -1로 설정해 첫 번째 클릭 시 첫 자식이 활성화되도록 함

    void Start()
    {
        // 자식 객체들을 배열에 저장
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
            children[i].gameObject.SetActive(false); // 모두 비활성화
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            ActivateNextChild();
        }
    }

    void ActivateNextChild()
    {
        // 현재 활성화된 객체 비활성화
        if (currentIndex >= 0 && currentIndex < children.Length)
        {
            children[currentIndex].gameObject.SetActive(false);
        }

        // 다음 객체 활성화 (1번부터 순환)
        currentIndex = (currentIndex + 1) % children.Length;
        children[currentIndex].gameObject.SetActive(true);
    }
}
