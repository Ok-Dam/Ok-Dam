using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDisabler : MonoBehaviour
{
    public Button disableButton; // 비활성화할 버튼
    public GameObject targetObject; // 비활성화할 오브젝트

    void Start()
    {
        if (disableButton != null && targetObject != null)
        {
            // 버튼 클릭 시 DisableObject 메서드 호출
            disableButton.onClick.AddListener(DisableObject);
        }
        else
        {
            Debug.LogError("Button or Target Object is not assigned!");
        }
    }

    void DisableObject()
    {
        targetObject.SetActive(false); // 오브젝트 비활성화
    }
}

