using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;
    public PlayerState state;

    // Hanok Info 데이터 리스트 - Inspector에서 할당
    [Header("Hanok Info 데이터")]
    public List<HanokInfoData> hanokInfoDataList;

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

    // 한옥 정보 안내 함수 추가
    public void ShowHanokInfoByNodeAndPart(int nodeNumber, HanokPart part, System.Action onEnd)
    {
        var infoData = hanokInfoDataList.Find(x => x.nodeNumber == nodeNumber);
        if (infoData != null)
        {
            var partInfo = infoData.infos.Find(i => i.part == part)
                        ?? infoData.infos.FirstOrDefault();
            if (partInfo != null)
            {
                quizPanelUI.ShowInformation(partInfo.infoText, partInfo.image, onEnd);
                return;
            }
        }
        quizPanelUI.ShowInformation("해당 정보를 찾을 수 없습니다.", null, onEnd);
    }
}
