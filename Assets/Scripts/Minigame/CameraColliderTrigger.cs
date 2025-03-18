using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderTrigger : MonoBehaviour



{
    public GameObject[] uiElements;  // 각 UI 창을 배열로 저장
    private bool[] isColliding;      // 각 collider와의 충돌 여부를 추적

    private void Start()
    {
        isColliding = new bool[uiElements.Length];

        // 모든 UI 창을 비활성화
        foreach (var ui in uiElements)
        {
            ui.SetActive(false);
        }
    }

    // 충돌 시작 시 호출
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체의 태그나 이름을 기준으로 활성화할 UI 찾기
        for (int i = 0; i < uiElements.Length; i++)
        {
            if (collision.gameObject.CompareTag("Collider" + (i + 1)))
            {
                uiElements[i].SetActive(true);
                isColliding[i] = true;
                break;
            }
        }
    }

    // 충돌 종료 시 호출
    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            if (collision.gameObject.CompareTag("Collider" + (i + 1)) && isColliding[i])
            {
                uiElements[i].SetActive(false);
                isColliding[i] = false;
                break;
            }
        }
    }
}
