using UnityEngine;

public class LockCursor : MonoBehaviour
{
    void Start()
    {
        // 마우스 커서 잠그기 + 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ESC 누르면 마우스 잠금 해제 (테스트용)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
