using UnityEngine;

public class ToggleController : MonoBehaviour
{
    public GameObject targetCanvas;  // 활성화/비활성화할 Canvas
    private int pressCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pressCount++;

            // 홀수일 때 활성화, 짝수일 때 비활성화
            if (pressCount % 2 == 1)
            {
                targetCanvas.SetActive(true);
            }
            else
            {
                targetCanvas.SetActive(false);
            }
        }
    }
}
