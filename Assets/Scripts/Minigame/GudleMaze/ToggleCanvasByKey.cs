using UnityEngine;

public class ToggleCanvasByKey : MonoBehaviour
{
    public GameObject targetCanvas;  // 활성화/비활성화할 Canvas 오브젝트

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (targetCanvas != null)
            {
                // 현재 상태 반대로 전환 (켜져 있으면 끄고, 꺼져 있으면 켜고)
                bool isActive = targetCanvas.activeSelf;
                targetCanvas.SetActive(!isActive);
            }
        }
    }
}
