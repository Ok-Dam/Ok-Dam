using UnityEngine;
using UnityEngine.UI;

public class SliderPanelController : MonoBehaviour
{
    public Slider slider;               // 연결된 슬라이더
    public float fillDuration = 5f;     // 슬라이더가 다 차는 데 걸리는 시간
    public GameObject currentPanel;     // 현재 패널
    public GameObject nextPanel;        // 다음 패널

    private float timer = 0f;
    private bool isFilling = false;

    void Start()
    {
        slider.value = 0;
        isFilling = true;
    }

    void Update()
    {
        if (isFilling)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Clamp01(timer / fillDuration);

            if (slider.value >= 1f)
            {
                isFilling = false;
                ActivateNextPanel();
            }
        }
    }

    void ActivateNextPanel()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
}
