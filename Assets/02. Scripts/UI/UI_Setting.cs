using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting : MonoBehaviour
{
    public GameObject GOSETTING;

    void Update()
    {
        // tabŰ�� ����â ���⸸ ��
        if (Input.GetKeyDown(KeyCode.Tab) && !GOSETTING.activeSelf)
        {
            GOSETTING.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // ��ư���� ����â �ݱ�
    public void OnButtonClick()
    {
        GOSETTING.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
