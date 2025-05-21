using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelE; // 패널 E
    public GameObject panelG; // 패널 G
    public List<GameObject> panelEChildren; // 패널 E의 자식 객체들

    private bool isPanelEActive = false; // 패널 E 진행 완료 여부 확인

    // Start is called before the first frame update
    void Start()
    {
        // 패널 E 자식 객체를 리스트에 추가
        panelEChildren = new List<GameObject>();
        foreach (Transform child in panelE.transform)
        {
            panelEChildren.Add(child.gameObject);
        }

        // 패널 E는 처음에는 비활성화
        panelE.SetActive(false);
        panelG.SetActive(true); // 패널 G는 활성화
    }

    // 버튼 "f" 클릭 시 패널 E로 이동
    public void OnButtonClick()
    {
        panelG.SetActive(false); // 패널 G 비활성화
        panelE.SetActive(true); // 패널 E 활성화
        isPanelEActive = true;
        ActivatePanelEChildren(); // 패널 E 자식 객체들을 활성화
    }

    // 패널 E의 자식 객체들을 활성화
    private void ActivatePanelEChildren()
    {
        foreach (var child in panelEChildren)
        {
            child.SetActive(true);
        }
    }

    // 패널 E의 모든 자식 객체가 활성화되었고, 그 후 비활성화가 되면 호출
    public void CompletePanelE()
    {
        // 자식 객체들을 비활성화
        foreach (var child in panelEChildren)
        {
            child.SetActive(false);
        }

        // 패널 E 비활성화 후 패널 G로 돌아옴
        panelE.SetActive(false);
        panelG.SetActive(true);
    }
}

