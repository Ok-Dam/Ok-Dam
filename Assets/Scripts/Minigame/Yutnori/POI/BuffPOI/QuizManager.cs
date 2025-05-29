using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;

    // 이미 출제된 퀴즈 인덱스 기록
    private List<int> usedQuizIndices = new List<int>();

    public void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }

    // 콜백과 PlayerState를 모두 받도록 시그니처 수정
    public void ShowRandomQuiz(System.Action<bool> onQuizEnd, PlayerState playerState = null)
    {
        var quizzes = quizDatabase.quizzes;
        if (quizzes.Count == 0) return;

        // 아직 출제되지 않은 퀴즈 인덱스 목록 생성
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < quizzes.Count; i++)
        {
            if (!usedQuizIndices.Contains(i))
                availableIndices.Add(i);
        }

        // 모든 퀴즈가 출제됐으면 이력 초기화(한 바퀴 돌고 다시 시작)
        if (availableIndices.Count == 0)
        {
            usedQuizIndices.Clear();
            for (int i = 0; i < quizzes.Count; i++)
                availableIndices.Add(i);
        }

        // 랜덤 인덱스 선택
        int randomIdx = availableIndices[Random.Range(0, availableIndices.Count)];
        usedQuizIndices.Add(randomIdx);

        var quiz = quizzes[randomIdx];
        quizPanelUI.Show(quiz, (isCorrect) => OnQuizResult(isCorrect, playerState, onQuizEnd));
    }

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
        onQuizEnd?.Invoke(isCorrect);
    }

    // 버프 효과 적용
    private void ApplyBuff(Buff buff, PlayerState player)
    {
        switch (buff.type)
        {
            case BuffType.ExtraThrow:
                player.AddThrowChance(1);
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
