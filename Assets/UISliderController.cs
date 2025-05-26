using UnityEngine;
using UnityEngine.UI;

public class UISliderController : MonoBehaviour
{
    public GameObject[] panels;         // ������� ������ �гε�
    public GameObject sliderObject;     // ���� ������ ������ �����̴�
    private int currentPanelIndex = 0;

    void Start()
    {
        // ù �гθ� ������
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == 0);

        // �����̴��� �ʹݿ� ����
        sliderObject.SetActive(false);
    }

    public void OnNextButtonClicked()
    {
        // ���� �г� ����
        panels[currentPanelIndex].SetActive(false);

        currentPanelIndex++;

        if (currentPanelIndex < panels.Length)
        {
            // ���� �г� �ѱ�
            panels[currentPanelIndex].SetActive(true);
        }
        else
        {
            // �г��� �� ������ �����̴� ����!
            sliderObject.SetActive(true);
        }
    }
}
