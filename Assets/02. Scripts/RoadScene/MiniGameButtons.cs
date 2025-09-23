using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameButtons : MonoBehaviour
{
    // 들어가기
    public void GoToOndol() => SceneTransitioner.EnterMiniGame("OndolPacMan", "FromOndol");
    public void GoToBuilding() => SceneTransitioner.EnterMiniGame("Building", "FromBuilding");

    // 돌아오기 (미니게임 씬 UI에서 연결)
    public void BackToMapFullRestart(string returnSpawnKey)
    {

        // 1) 돌아올 스폰 키 저장 + 복귀 플래그 ON
        ReturnSpawn.Set(returnSpawnKey);                // 예) "FromMaze", "FromGudeul"
        GameStateManager.isReturningFromMiniGame = true;

        // 2) 룸에 있으면 나가서 완전 재시작 → 네 PhotonManager.OnLeftRoom()이 MapScene을 로드함
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();                 // → OnLeftRoom()에서 SceneManager.LoadScene("MapScene")
        }
        else
        {
            // 룸 밖이면 PhotonNetwork.LoadLevel은 동작하지 않음. 로컬 로드만 가능.
            SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        }
        Debug.Log($"[BackToMap] key={returnSpawnKey}, inRoom={PhotonNetwork.InRoom}");
    }
}