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

        quizPanelUI.Show(quizToShow, (isCorrect) => OnQuizAnswered(isCorrect, playerState, onQuizEnd));
    }

    private void OnQuizAnswered(bool isCorrect, PlayerState playerState, System.Action<bool> onQuizEnd)
    {
        bool autoSuccess = playerState != null && playerState.nextBuffAutoSuccess;
        bool giveBuff = isCorrect || autoSuccess;
        Buff selectedBuff = null;
        string buffMessage = "";
        string explanation = quizPanelUI.lastExplanation;

        if (giveBuff)
        {
            if (autoSuccess)
            {
                playerState.ConsumeNextBuffAutoSuccess();
            }
            selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            buffMessage = (autoSuccess ? "[자동 성공] " : "정답! ") + $"'{selectedBuff.description}' 버프 획득!";
        }
        else
        {
            buffMessage = "오답! 버프를 얻지 못했습니다.";
        }

        quizPanelUI.ShowExpPanel(
            isCorrect,
            explanation,
            buffMessage,
            () => { onQuizEnd?.Invoke(isCorrect); }
        );
    }

    private void ApplyBuff(Buff buff, PlayerState player)
    {
        switch (buff.type)
        {
            case BuffType.ExtraThrow:
                player.AddThrowChance(1);
                break;
            case BuffType.NextMovePlus:
                player.nextMovePlus += 1;
                break;
            case BuffType.NextBuffAutoSuccess:
                player.nextBuffAutoSuccess = true;
                break;
        }
    }


    private void OnQuizResult(bool isCorrect, PlayerState playerState, System.Action<bool> onQuizEnd)
    {
        bool autoSuccess = playerState != null && playerState.nextBuffAutoSuccess;
        Buff selectedBuff = null;
        string buffMessage = "";
        string explanation = quizPanelUI.lastExplanation; // QuizPanelUI에 lastExplanation 추가 필요

        if (autoSuccess)
        {
            // 자동 성공 버프 소모
            playerState.ConsumeNextBuffAutoSuccess();
            // 버프 지급 (정답/오답 상관없이)
            selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            buffMessage = $"[자동 성공] '{selectedBuff.description}' 버프!";
            Debug.Log($"자동 성공! '{selectedBuff.description}' 버프가 적용되었습니다.");
        }
        else if (isCorrect)
        {
            selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            buffMessage = $"'{selectedBuff.description}' 버프!";
            Debug.Log($"'{selectedBuff.description}' 버프가 적용.");
        }
        else
        {
            buffMessage = "버프를 얻지 못했습니다.";
            Debug.Log("버프가 적용되지 않습니다.");
        }

        // ExpPanel에 해설과 버프 메시지 표시
        // onQuizEnd가 Action<bool>인 경우
        quizPanelUI.ShowExpPanel(
            isCorrect,
            explanation,
            buffMessage,
            () => { onQuizEnd?.Invoke(isCorrect); } // 반드시 람다로 감싸서 전달!
        );


        // onQuizEnd는 여기서 직접 호출하지 않는다!
    }


}
