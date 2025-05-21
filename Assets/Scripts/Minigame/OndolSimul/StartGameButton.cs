using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public GameObject instructionCanvas; // 게임 방법 설명창 Canvas

    public void OnStartGame()
    {
        instructionCanvas.SetActive(false);
        // 여기에 추가로 게임을 시작하는 로직을 넣을 수 있어요!
    }
}
