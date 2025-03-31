using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlowController : MonoBehaviour
{
    public GameObject uiParent; // "UI_exp" 빈 오브젝트
    public Button smogFlowButton; // "smog flow" 버튼
    public GameObject buttonPanel; // 버튼이 포함된 패널

    private void Start()
    {
        // 시작 시 빈 오브젝트는 활성화, 자식 오브젝트는 비활성화
        uiParent.SetActive(true);
        ToggleChildObjects(false);

        // 버튼에 이벤트 리스너 등록
        smogFlowButton.onClick.AddListener(StartFlowSequence);
    }

    void ToggleChildObjects(bool isActive)
    {
        foreach (Transform child in uiParent.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

    public void StartFlowSequence()
    {
        // 버튼을 눌렀을 때 버튼이 포함된 패널 비활성화
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
        }

        StartCoroutine(FlowSequence());
    }

    IEnumerator FlowSequence()
    {
        // 자식 객체들이 차례대로 활성화되도록 2개씩 활성화
        for (int i = 0; i < uiParent.transform.childCount; i += 2)
        {
            // 현재 자식 객체 2개 활성화
            if (i < uiParent.transform.childCount)
                uiParent.transform.GetChild(i).gameObject.SetActive(true);
            if (i + 1 < uiParent.transform.childCount)
                uiParent.transform.GetChild(i + 1).gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            // 2개 비활성화
            if (i < uiParent.transform.childCount)
                uiParent.transform.GetChild(i).gameObject.SetActive(false);
            if (i + 1 < uiParent.transform.childCount)
                uiParent.transform.GetChild(i + 1).gameObject.SetActive(false);
        }

        // 모든 자식 객체가 활성화되었으면 버튼이 포함된 패널을 다시 활성화
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(true);
        }
    }
}
