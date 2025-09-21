using UnityEngine;

public class MiniGameButtons : MonoBehaviour
{
    // 들어가기
    public void GoToMaze() => SceneTransitioner.EnterMiniGame("OndolPacMan");
    public void GoToGudeul() => SceneTransitioner.EnterMiniGame("Building");

    // 돌아오기 (미니게임 씬 UI에서 연결)
    public void BackToMap() => SceneTransitioner.ReturnToMap();
}