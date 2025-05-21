using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panelB; // 패널 B
    public GameObject panelC; // 패널 C

    // 버튼을 눌렀을 때 패널 B를 열고 패널 C를 닫기
    public void OpenPanel()
    {
        if (panelB != null)
        {
            panelB.SetActive(true); // 패널 B 활성화
        }

        if (panelC != null)
        {
            panelC.SetActive(false); // 패널 C 비활성화
        }
    }
}
