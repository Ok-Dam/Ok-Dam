using UnityEngine;

public class WallController : MonoBehaviour
{
    public GameObject correctWall;  // 정답 벽
    public GameObject wrongWall;    // 오답 벽
    public GameObject quizCanvas;   // 퀴즈 UI가 포함된 Canvas

    // 정답 버튼 클릭 시 실행
    public void RemoveCorrectWall()
    {
        if (correctWall != null)
            correctWall.SetActive(false);  // 정답 벽 제거

        DisableCanvas();  // Canvas 비활성화
    }

    // 오답 버튼 클릭 시 실행
    public void RemoveWrongWall()
    {
        if (wrongWall != null)
            wrongWall.SetActive(false);    // 오답 벽 제거

        DisableCanvas();  // Canvas 비활성화
    }

    // Canvas를 비활성화하는 공통 함수
    private void DisableCanvas()
    {
        if (quizCanvas != null)
            quizCanvas.SetActive(false);
    }
}
