using UnityEngine;
using UnityEngine.UI;

public class UISliderController : MonoBehaviour
{
    public GameObject[] panels;         // 순서대로 보여줄 패널들
    public GameObject sliderObject;     // 설명 끝나고 보여줄 슬라이더
    private int currentPanelIndex = 0;

    void Start()
    {
        // 첫 패널만 보여줌
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == 0);

        // 슬라이더는 초반엔 꺼둠
        sliderObject.SetActive(false);
    }

    public void OnNextButtonClicked()
    {
        // 현재 패널 끄기
        panels[currentPanelIndex].SetActive(false);

        currentPanelIndex++;

        if (currentPanelIndex < panels.Length)
        {
            // 다음 패널 켜기
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            // 패널이 다 끝나면 슬라이더 등장!
            sliderObject.SetActive(true);
        }
    }
}
