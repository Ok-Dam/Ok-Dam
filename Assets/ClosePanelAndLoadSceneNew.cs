using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosePanelAndLoadSceneNew : MonoBehaviour
{
    public GameObject panelToClose;     // ¥›¿ª ∆–≥Œ
    public string sceneToLoadName;      // ¿Ãµø«“ æ¿ ¿Ã∏ß

    public void CloseAndLoad()
    {
        if (panelToClose != null)
            panelToClose.SetActive(false);

        SceneManager.LoadScene(sceneToLoadName);
    }
}
