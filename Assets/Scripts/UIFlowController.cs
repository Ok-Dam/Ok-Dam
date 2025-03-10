using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlowController : MonoBehaviour
{
    public GameObject uiParent; // "UI_exp" 빈 오브젝트
    public Button smogFlowButton; // "smog flow" 버튼

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
        // 버튼을 강제로 다시 활성화 (이 코드가 실행될 때마다 보장)
        smogFlowButton.gameObject.SetActive(true);

        StartCoroutine(FlowSequence());
    }

    IEnumerator FlowSequence()
    {
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

        // 버튼이 `uiParent` 안에 있다면 다시 활성화
        smogFlowButton.gameObject.SetActive(true);
    }
}
