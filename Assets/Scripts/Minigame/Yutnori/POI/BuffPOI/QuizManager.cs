using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;
    public PlayerState state;

    // 이미 출제된 퀴즈 인덱스 기록
    private List<int> usedQuizIndices = new List<int>();

    public void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }

    public void ShowQuizByNodeNumber(int nodeNumber, System.Action<bool> onQuizEnd, PlayerState playerState = null)
    {
        var quizzes = quizDatabase.quizzes;
        var nodeQuizzes = quizzes.Where(q => q.nodeNumber == nodeNumber).ToList();
        QuizData quizToShow = nodeQuizzes.Count > 0
            ? nodeQuizzes[Random.Range(0, nodeQuizzes.Count)]
            : quizzes[Random.Range(0, quizzes.Count)];

        // 자동 성공 버프가 있으면 바로 정답 처리
        if (playerState != null && playerState.nextBuffAutoSuccess)
        {
            playerState.ConsumeNextBuffAutoSuccess();
            Debug.Log("[BuffPOI] 버프 자동 성공");
            OnQuizResult(true, playerState, onQuizEnd); // 정답 처리
            return;
        }

        quizPanelUI.Show(quizToShow, (isCorrect) => OnQuizResult(isCorrect, playerState, onQuizEnd));
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
