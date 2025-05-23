using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;

    public void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }

    // 콜백과 PlayerState를 모두 받도록 시그니처 수정
    public void ShowRandomQuiz(System.Action<bool> onQuizEnd, PlayerState playerState = null)
    {
        var quizzes = quizDatabase.quizzes;
        if (quizzes.Count == 0) return;

        var quiz = quizzes[Random.Range(0, quizzes.Count)];
        quizPanelUI.Show(quiz, (isCorrect) => OnQuizResult(isCorrect, playerState, onQuizEnd));
    }

    // 퀴즈 결과 처리 및 버프 지급
    private void OnQuizResult(bool isCorrect, PlayerState playerState, System.Action<bool> onQuizEnd)
    {
        if (isCorrect)
        {
            Buff selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            Debug.Log($"정답! '{selectedBuff.description}' 버프가 적용되었습니다.");
        }
        else
        {
            Debug.Log("오답! 버프가 적용되지 않습니다.");
        }
        // 외부 콜백 호출 (게임 흐름 복귀 등)
        onQuizEnd?.Invoke(isCorrect);
    }

    // 버프 효과 적용
    private void ApplyBuff(Buff buff, PlayerState player)
    {
        switch (buff.type)
        {
            case BuffType.ExtraThrow:
                player.bonusThrowCount++;
                break;
            case BuffType.StackPiece:
                player.canStackPiece = true;
                break;
            case BuffType.NextMovePlus:
                player.nextMovePlus = 1;
                break;
            case BuffType.NextBuffAutoSuccess:
                player.nextBuffAutoSuccess = true;
                break;
        }
        // 필요하다면 UI에 버프 아이콘/설명 표시
    }
}
