using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using static MapSceneInitializer;
using System.Linq;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    //버전 입력
    private readonly string version = "1.0f";
    // 사용자 아이디 입력
    private string userid;

    //Start보다 먼저 실행됨.
    private void Awake()
    {
        if (PhotonNetwork.IsConnected) return;
        //같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;
        //같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;
        //유저 아이디 할당
        userid = "User" + Random.Range(1000, 9999);
        PhotonNetwork.NickName = userid;
        //포톤 서버와 통신 횟수 설정.(초당 30회)
        Debug.Log(PhotonNetwork.SendRate);
        Debug.Log($"My NickName: {PhotonNetwork.NickName}");
        //서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    //포톤 서버 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //로비 확인
        PhotonNetwork.JoinLobby();
    }

    //로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); //랜덤 매치메이킹 기능 제공

    }

    //랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");

        //room의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;      //최대 접속자 수
        ro.IsOpen = true;        //룸의 오픈 여부
        ro.IsVisible = true;    //로비에서 룸 목록에 노출 시킬 시 여부

        //룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    //룸 생성 완료 후 콜백함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotoNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
            Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");

        // MapScene에서만, 그리고 아직 내 Player가 없을 때만 스폰
        if (SceneManager.GetActiveScene().name == "MapScene" &&
            GameObject.FindWithTag("Player") == null)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;
            bool chosen = false;

            // 0) 복귀라면: MapSceneInitializer가 Awake에서 만들어둔 "ReturnPoint" 최우선
            if (GameStateManager.isReturningFromMiniGame)
            {
                var rp = GameObject.Find("ReturnPoint");
                if (rp != null)
                {
                    spawnPos = rp.transform.position + new Vector3(0f, -1.9f, 0f);
                    spawnRot = rp.transform.rotation;
                    chosen = true;
                    Debug.Log("[Photon] Spawning at ReturnPoint.");
                }
            }

            // 1) (보조) 키 기반 스폰: 아직 못 골랐고, SpawnPoint(Key) 있으면 사용
            if (!chosen && ReturnSpawn.HasKey)
            {
                var key = ReturnSpawn.Consume();
                var all = Object.FindObjectsOfType<SpawnPoint>(true);
                var byKey = all.FirstOrDefault(sp => sp.Key == key);
                if (byKey != null)
                {
                    spawnPos = byKey.transform.position;
                    spawnRot = byKey.transform.rotation;
                    chosen = true;
                    Debug.Log($"[Photon] Spawning by key '{key}'.");
                }
            }

            // 2) (폴백) SpawnPoint(Default) 사용
            if (!chosen)
            {
                var all = Object.FindObjectsOfType<SpawnPoint>(true);
                var def = all.FirstOrDefault(sp => sp.Key == "Default");
                if (def != null)
                {
                    spawnPos = def.transform.position;
                    spawnRot = def.transform.rotation;
                    chosen = true;
                    Debug.Log("[Photon] Spawning at SpawnPoint 'Default'.");
                }
            }

            // 3) (최종 폴백) SpawnPointGroup 랜덤
            if (!chosen)
            {
                var group = GameObject.Find("SpawnPointGroup");
                if (group != null)
                {
                    var points = group.GetComponentsInChildren<Transform>();
                    if (points != null && points.Length > 1)
                    {
                        int idx = Random.Range(1, points.Length); // 0은 그룹 자신
                        spawnPos = points[idx].position;
                        spawnRot = points[idx].rotation;
                        chosen = true;
                        Debug.Log("[Photon] Spawning from SpawnPointGroup (random).");
                    }
                }

                if (!chosen)
                    Debug.LogWarning("[Photon] No spawn point found. Using (0,0,0).");
            }

            // 실제 스폰
            PhotonNetwork.Instantiate("Player", spawnPos, spawnRot);

            // 1회성 플래그 정리
            GameStateManager.isReturningFromMiniGame = false;
        }
    }



    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MapScene");  // 룸 나가면 다시 맵 씬 로딩
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}