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
            buffMessage = (autoSuccess ? "[�ڵ� ����] " : "����! ") + $"'{selectedBuff.description}' ���� ȹ��!";
        }
        else
        {
            buffMessage = "����! ������ ���� ���߽��ϴ�.";
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
        string explanation = quizPanelUI.lastExplanation; // QuizPanelUI�� lastExplanation �߰� �ʿ�

        if (autoSuccess)
        {
            // �ڵ� ���� ���� �Ҹ�
            playerState.ConsumeNextBuffAutoSuccess();
            // ���� ���� (����/���� �������)
            selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            buffMessage = $"[�ڵ� ����] '{selectedBuff.description}' ����!";
            Debug.Log($"�ڵ� ����! '{selectedBuff.description}' ������ ����Ǿ����ϴ�.");
        }
        else if (isCorrect)
        {
            selectedBuff = buffManager.GetRandomBuff();
            if (playerState != null)
            {
                ApplyBuff(selectedBuff, playerState);
            }
            buffMessage = $"'{selectedBuff.description}' ����!";
            Debug.Log($"'{selectedBuff.description}' ������ ����.");
        }
        else
        {
            buffMessage = "������ ���� ���߽��ϴ�.";
            Debug.Log("������ ������� �ʽ��ϴ�.");
        }

        // ExpPanel�� �ؼ��� ���� �޽��� ǥ��
        // onQuizEnd�� Action<bool>�� ���
        quizPanelUI.ShowExpPanel(
            isCorrect,
            explanation,
            buffMessage,
            () => { onQuizEnd?.Invoke(isCorrect); } // �ݵ�� ���ٷ� ���μ� ����!
        );


        // onQuizEnd�� ���⼭ ���� ȣ������ �ʴ´�!
    }


}
