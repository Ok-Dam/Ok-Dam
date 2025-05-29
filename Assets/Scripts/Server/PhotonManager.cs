using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using static MapSceneInitializer;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    //���� �Է�
    private readonly string version = "1.0f";
    // ����� ���̵� �Է�
    private string userid;

    //Start���� ���� �����.
    private void Awake()
    {
        if (PhotonNetwork.IsConnected) return;
        //���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;
        //���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;
        //���� ���̵� �Ҵ�
        userid = "User" + Random.Range(1000, 9999);
        PhotonNetwork.NickName = userid;
        //���� ������ ��� Ƚ�� ����.(�ʴ� 30ȸ)
        Debug.Log(PhotonNetwork.SendRate);
        Debug.Log($"My NickName: {PhotonNetwork.NickName}");
        //���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    //���� ���� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //�κ� Ȯ��
        PhotonNetwork.JoinLobby();
    }

    //�κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); //���� ��ġ����ŷ ��� ����



        if (SceneManager.GetActiveScene().name == "MapScene" &&
        GameObject.FindWithTag("Player") == null)
        {
            Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int idx = Random.Range(1, points.Length);

            PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);
        }
    }

    //������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");

        //room�� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;      //�ִ� ������ ��
        ro.IsOpen = true;        //���� ���� ����
        ro.IsVisible = true;    //�κ񿡼� �� ��Ͽ� ���� ��ų �� ����

        //�� ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    //�� ���� �Ϸ� �� �ݹ��Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotoNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");
        }

        if (SceneManager.GetActiveScene().name == "MapScene" && GameObject.FindWithTag("Player") == null)
        {
            Vector3 spawnPos;
            Quaternion spawnRot;

            if (GameStateManager.isReturningFromMiniGame)
            {
                GameObject returnPoint = GameObject.Find("ReturnPoint");
                if (returnPoint != null)
                {
                    spawnPos = returnPoint.transform.position;
                    spawnRot = returnPoint.transform.rotation;
                }
                else
                {
                    Debug.LogWarning("ReturnPoint not found! Using default spawn.");
                    Transform[] fallback = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
                    int idx = Random.Range(1, fallback.Length);
                    spawnPos = fallback[idx].position;
                    spawnRot = fallback[idx].rotation;
                }

                GameStateManager.isReturningFromMiniGame = false;
            }
            else
            {
                Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
                int idx = Random.Range(1, points.Length);
                spawnPos = points[idx].position;
                spawnRot = points[idx].rotation;
            }

            PhotonNetwork.Instantiate("Player", spawnPos, spawnRot);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MapScene");  // �� ������ �ٽ� �� �� �ε�
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
