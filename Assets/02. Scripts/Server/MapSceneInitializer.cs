using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MapSceneInitializer : MonoBehaviourPunCallbacks
{
    private static bool restarted = false;

    void Awake()
    {

        Debug.Log("[MapSceneInitializer] Awake");

        // ❌ 기존에 여기서 isReturningFromMiniGame = false 하던 줄은 제거해야 함!
        // GameStateManager.isReturningFromMiniGame = false;  // <- 지워주세요

        // ✅ 복귀라면: 저장된 스폰 키로 ReturnPoint를 먼저 만들어 둔다
        if (GameStateManager.isReturningFromMiniGame && ReturnSpawn.HasKey)
        {
            var key = ReturnSpawn.Consume();

            CreateOrMoveReturnPoint(key);
        }

        if (restarted)
        {
            restarted = false;
            return;
        }
    }

    void Start()
    {
        // ✅ 룸에 없으면 다시 들어가도록 보장 (그래야 OnJoinedRoom에서 Player가 생성됨)
        if (!PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsConnectedAndReady) PhotonNetwork.JoinRandomRoom();
            else PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Photon 콜백 그대로 유지(필요 시)
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MapScene");
    }

    public static void SetRestartFlag()
    {
        restarted = true;
    }

    // ---------- helpers ----------
    private void CreateOrMoveReturnPoint(string key)
    {
        // 1) SpawnPoint(Key) 우선
        Transform t = TryFindSpawnTransformByKey(key);

        // 2) 없으면 Default
        if (t == null) t = TryFindSpawnTransformByKey("Default");

        if (t == null)
        {
            Debug.LogWarning($"[MapSceneInitializer] No spawn transform for key '{key}'.");
            return;
        }

        var rp = GameObject.Find("ReturnPoint");
        if (rp == null) rp = new GameObject("ReturnPoint");

        rp.transform.SetPositionAndRotation(t.position, t.rotation);
        Debug.Log($"[MapSceneInitializer] ReturnPoint set by key '{key}' at {t.position}");
    }

    private Transform TryFindSpawnTransformByKey(string key)
    {
        // (A) SpawnPoint 컴포넌트 기반
        var all = FindObjectsOfType<SpawnPoint>(true);
        var sp = all.FirstOrDefault(p => p.Key == key);
        if (sp != null) return sp.transform;

        // (B) SpawnPointGroup 하위에 같은 이름 자식
        var group = GameObject.Find("SpawnPointGroup");
        if (group != null)
        {
            var child = group.transform.Find(key);
            if (child != null) return child;
        }

        // (C) 씬에 같은 이름 오브젝트
        var go = GameObject.Find(key);
        if (go != null) return go.transform;

        return null;
    }
}
