using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosePanelAndLoadScene : MonoBehaviour
{
    public string sceneToLoadName;  // �̵��� �� �̸�

    public void CloseAndLoad()
    {
        SceneManager.LoadScene(sceneToLoadName);
    }
}
