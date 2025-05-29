using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;
    public PlayerState state;

    // �̹� ������ ���� �ε��� ���
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

        // �ڵ� ���� ������ ������ �ٷ� ���� ó��
        if (playerState != null && playerState.nextBuffAutoSuccess)
        {
            playerState.ConsumeNextBuffAutoSuccess();
            Debug.Log("[BuffPOI] ���� �ڵ� ����");
            OnQuizResult(true, playerState, onQuizEnd); // ���� ó��
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
            Debug.Log($"����! '{selectedBuff.description}' ������ ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("����! ������ ������� �ʽ��ϴ�.");
        }
        onQuizEnd?.Invoke(isCorrect);
    }

    // ���� ȿ�� ����
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
        // �ʿ��ϴٸ� UI�� ���� ������/���� ǥ��
    }
}
