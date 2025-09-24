using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class  pacResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    public void ShowResultPanel(int result, int heatedCount)
    {
        gameObject.SetActive(true);

        if (result == 0)
        {
            resultText.text = $"성공했습니다!\n데운 총 바닥 수 : {heatedCount}";
        }
        else if (result == 1)
        {
            resultText.text = $"실패했습니다!\n데운 총 바닥 수 : {heatedCount}";
        }
        else
        {
            Debug.LogError("result 인자 범위 밖 오류");
            resultText.text = "오류 발생";
        }
    }

    public void RestartGame()
    {
        // 현재 씬 재시작
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ExitButtonAction()
    {
        // TODO
        Debug.Log("Other button action is not implemented yet.");
    }
}
