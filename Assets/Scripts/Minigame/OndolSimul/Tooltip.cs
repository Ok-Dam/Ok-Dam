using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipUI; // 설명 UI (미리 배치된 World Space Canvas)

    void Start()
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(false); // 시작 시 비활성화
        }
    }

    void OnMouseEnter()
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(false);
        }
    }
}
