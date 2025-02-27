using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가
using UnityEngine.UI; // UI 요소를 다루기 위해 필요

public class SceneLoader : MonoBehaviour
{
    public Button sceneChangeButton; // 버튼 변수

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        sceneChangeButton.onClick.AddListener(LoadGudleScene);
        sceneChangeButton.onClick.AddListener(LoadGoraeScene);
    }

    public void LoadGudleScene()
    {
        Debug.Log("버튼 클릭! 'Gudle' 씬으로 이동");
        SceneManager.LoadScene("Gudle"); // 씬 이동
    }

    public void LoadGoraeScene()
    {
        Debug.Log("버튼 클릭! 'Gorae' 씬으로 이동");
        SceneManager.LoadScene("Gorae"); // 씬 이동
    }
}



