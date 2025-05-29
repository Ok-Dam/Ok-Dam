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
    // Tab Ű�� ������ Ŀ�� ���߱�
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
