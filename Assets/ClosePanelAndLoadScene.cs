using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosePanelAndLoadScene : MonoBehaviour
{
    public string sceneToLoadName;  // 이동할 씬 이름

    public void CloseAndLoad()
    {
        SceneManager.LoadScene(sceneToLoadName);
    }
}
