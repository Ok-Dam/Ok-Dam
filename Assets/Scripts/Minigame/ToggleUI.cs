using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour

{
    public List<GameObject> uiElements; // UI 요소를 담을 리스트
    public Button toggleButton;  // 버튼

    private int currentIndex = 0;  // 현재 표시할 UI 요소의 인덱스

    void Start()
    {
        // 버튼 클릭 시 UI 토글
        toggleButton.onClick.AddListener(ToggleVisibility);

        // 처음에는 모든 UI 요소를 숨깁니다.
        foreach (var ui in uiElements)
        {
            ui.SetActive(false);
        }
    }

    // UI 요소를 토글하는 함수
    void ToggleVisibility()
    {
        // 현재 UI 요소가 활성화 되어 있으면 비활성화
        uiElements[currentIndex].SetActive(false);

        // 인덱스를 증가시켜 다음 UI 요소로 넘어가고, 리스트의 마지막까지 갔으면 처음으로 돌아갑니다.
        currentIndex = (currentIndex + 1) % uiElements.Count;

        // 새로운 UI 요소를 활성화
        uiElements[currentIndex].SetActive(true);
    }
}
