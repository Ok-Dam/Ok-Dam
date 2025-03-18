using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    public GameObject panel; // 열고 싶은 UI 패널

    private void Start()
    {
        // 패널을 시작할 때 비활성화
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        // 마우스 클릭으로 오브젝트를 클릭한 경우
        if (Input.GetMouseButtonDown(0))  // 0은 왼쪽 마우스 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 오브젝트가 현재 오브젝트인지 확인
                if (hit.collider.gameObject == gameObject)
                {
                    TogglePanel();
                }
            }
        }
    }

    // 패널 열기/닫기 토글
    void TogglePanel()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
