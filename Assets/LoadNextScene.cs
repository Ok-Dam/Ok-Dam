using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // 이 함수는 버튼의 OnClick 이벤트에 연결s

    // 또는 씬 이름으로 이동하고 싶으면 아래 함수 사용
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
