using UnityEngine;
using UnityEngine.UI;

public class OndolTemperature : MonoBehaviour
{
    public GameObject effectPrefab; // 온도 변화 효과 프리팹
    public GameObject speechBubble; // 말풍선 UI
    public Text temperatureText;    // 온도 표시 텍스트
    public Button tempButton;       // 온도 확인 버튼

    private GameObject currentEffect;

    void Start()
    {
        // 버튼에 이벤트 연결
        tempButton.onClick.AddListener(ShowTemperatureEffect);
        speechBubble.SetActive(false); // 초기에는 말풍선 비활성화
    }

    void ShowTemperatureEffect()
    {
        if (currentEffect == null)
        {
            // 온돌 위에 효과 생성
            currentEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            // 온도 정보 업데이트
            speechBubble.SetActive(true);
            temperatureText.text = "온도: 45°C";

            // 일정 시간 후 효과 제거
            Invoke("HideEffect", 5f);
        }
    }

    void HideEffect()
    {
        Destroy(currentEffect);
        speechBubble.SetActive(false);
    }
}
