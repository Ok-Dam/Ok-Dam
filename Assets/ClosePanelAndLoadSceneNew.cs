using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;     // ���� �г�
    public string sceneToLoadName;      // �̵��� �� �̸�

    public void CloseAndLoad()
    {
        if (panelToClose != null)
            panelToClose.SetActive(false);

        SceneManager.LoadScene(sceneToLoadName);
    }
}
