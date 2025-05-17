using UnityEngine;
using Photon.Pun;

public class MiniMapManager : MonoBehaviour
{
    public Camera miniMapCamera;
    public Canvas miniMapCanvas;
    public RenderTexture miniMapRT;
    public float cameraHeight = 50f;

    private Transform myPlayer;

    void Start()
    {
        // 로컬 플레이어 찾기
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                myPlayer = player.transform;
                break;
            }
        }

        if (miniMapCamera != null && miniMapRT != null)
        {
            miniMapCamera.targetTexture = miniMapRT;
            Debug.Log("[MiniMapManager] RenderTexture 연결 완료");
        }

        if (miniMapCanvas != null && miniMapCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            miniMapCanvas.worldCamera = Camera.main;
        }

        if (myPlayer == null)
        {
            Debug.LogWarning("[MiniMapManager] 내 플레이어를 찾지 못했습니다.");
        }
    }

    void LateUpdate()
    {
        if (myPlayer == null || miniMapCamera == null) return;

        Vector3 targetPos = myPlayer.position;
        targetPos.y = cameraHeight;
        miniMapCamera.transform.position = targetPos;
        miniMapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
