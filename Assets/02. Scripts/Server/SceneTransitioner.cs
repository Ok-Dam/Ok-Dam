using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitioner
{
    /// <summary>
    /// 미니게임 씬으로 전환 (현재 플레이어의 위치/회전을 저장해두고 떠남)
    /// </summary>
    public static void EnterMiniGame(string miniGameSceneName)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && ReturnPointManager.Instance != null)
        {
            // 돌아올 위치/각도 저장
            ReturnPointManager.Instance.SetReturnPoint(player.transform); // 위치·회전 저장 :contentReference[oaicite:1]{index=1}
        }

        // 아직은 '돌아오는 중'이 아님
        GameStateManager.isReturningFromMiniGame = false; // 플래그 초기화 :contentReference[oaicite:2]{index=2}

        // 미니게임 로드(싱글 UI형)
        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// 맵으로 복귀 (복귀 플래그 ON → 맵 로드 후 복원 로직이 동작)
    /// </summary>
    public static void ReturnToMap()
    {
        GameStateManager.isReturningFromMiniGame = true; // 복귀 모드 진입 :contentReference[oaicite:3]{index=3}
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }
}
