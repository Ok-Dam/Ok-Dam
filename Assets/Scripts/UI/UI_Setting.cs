using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting : MonoBehaviour
{
    public GameObject GOSETTING;

    void Update()
    {
        // tab키로 설정창 열기만 함
        if (Input.GetKeyDown(KeyCode.Tab) && !GOSETTING.activeSelf)
        {
            GOSETTING.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // 버튼으로 설정창 닫기
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
