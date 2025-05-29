using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;
    private BuffManager buffManager;

    // �̹� ������ ���� �ε��� ���
    private List<int> usedQuizIndices = new List<int>();

    public void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }

    // �ݹ�� PlayerState�� ��� �޵��� �ñ״�ó ����
    public void ShowRandomQuiz(System.Action<bool> onQuizEnd, PlayerState playerState = null)
    {
        var quizzes = quizDatabase.quizzes;
        if (quizzes.Count == 0) return;

        // ���� �������� ���� ���� �ε��� ��� ����
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < quizzes.Count; i++)
        {
            if (!usedQuizIndices.Contains(i))
                availableIndices.Add(i);
        }

        // ��� ��� ���������� �̷� �ʱ�ȭ(�� ���� ���� �ٽ� ����)
        if (availableIndices.Count == 0)
        {
            usedQuizIndices.Clear();
            for (int i = 0; i < quizzes.Count; i++)
                availableIndices.Add(i);
        }

        // ���� �ε��� ����
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
