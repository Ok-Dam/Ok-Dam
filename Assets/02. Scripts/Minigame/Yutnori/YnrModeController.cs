using System.Linq;
using UnityEngine;
using Photon.Pun;

public class YnrModeController : MonoBehaviour
{
    public Camera explorationCamera;
    public Camera yutnoriCamera;
    public GameObject yutnoriUI;
    public GameObject miniMap;
    public GameObject boardRoot;
    public GameObject gameManager;
    public GameObject yuts;
    public GameObject players; //

  
    public void OnYutnoriStartRequest()
    {
    
        var localPlayer = FindObjectsOfType<PhotonView>()
            .FirstOrDefault(pv => pv.IsMine && pv.CompareTag("Player"))?.gameObject;
        if (localPlayer != null)
            EnterYutnoriMode(localPlayer);
        yutnoriCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void EnterYutnoriMode(GameObject player)
    {
      
        foreach (var script in player.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script is PhotonView || script is Animator) continue;
            script.enabled = false;
        }
        foreach (var col in player.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        foreach (var rb in player.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }

        if (miniMap != null)
            miniMap.SetActive(false);
        if (explorationCamera != null)
            explorationCamera.gameObject.SetActive(false);
        if (yutnoriCamera != null)
            yutnoriCamera.gameObject.SetActive(true);
        if (yutnoriUI != null)
            yutnoriUI.SetActive(true);
        if (boardRoot != null)
            boardRoot.SetActive(true);
        if (gameManager != null) gameManager.SetActive(true);
        if (yuts != null) yuts.SetActive(true);
        if (players != null) players.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
}