using UnityEngine;

public class ClickToActivateUI : MonoBehaviour
{
    public GameObject uiPanelToActivate; // 클릭 시 활성화할 UI

    void OnMouseDown()
    {
        if (uiPanelToActivate != null)
        {
            uiPanelToActivate.SetActive(true);
        }
    }
}
