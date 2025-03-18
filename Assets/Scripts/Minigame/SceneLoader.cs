using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button sceneChangeButton; // 버튼 변수
    public string sceneName; // 씬 이름 변수

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        sceneChangeButton.onClick.AddListener(LoadScene);
    }

    // 버튼 클릭 시 씬 전환
    public void LoadScene()
    {
        Debug.Log("버튼 클릭! " + sceneName + " 씬으로 이동");
        SceneManager.LoadScene(sceneName); // 씬 이동
    }
}

