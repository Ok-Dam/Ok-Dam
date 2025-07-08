using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MapSceneInitializer : MonoBehaviourPunCallbacks
{
    private static bool restarted = false;

    void Awake()
    {
        Debug.Log("[MapSceneInitializer] Start ¡¯¿‘");
        GameStateManager.isReturningFromMiniGame = false;

        if (restarted)
        {
            restarted = false;
            PhotonNetwork.LeaveRoom();
            return;
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MapScene");
    }

    public static void SetRestartFlag()
    {
        restarted = true;
    }
}
