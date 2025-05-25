using UnityEngine;
using UnityEngine.UI;

public class SliderPanelController : MonoBehaviour
{
    public Slider slider;               // ����� �����̴�
    public float fillDuration = 5f;     // �����̴��� �� ���� �� �ɸ��� �ð�
    public GameObject currentPanel;     // ���� �г�
    public GameObject nextPanel;        // ���� �г�

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
