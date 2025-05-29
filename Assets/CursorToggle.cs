using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    void OnEnable()
    {
        //
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    { 
    // Tab 키를 누르면 커서 감추기
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
