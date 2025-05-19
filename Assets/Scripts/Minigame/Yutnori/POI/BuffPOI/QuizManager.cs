using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public QuizDatabase quizDatabase;
    public QuizPanelUI quizPanelUI;

    public void ShowRandomQuiz(System.Action<bool> onQuizEnd)
    {
        var quizzes = quizDatabase.quizzes;
        if (quizzes.Count == 0) return;

        var quiz = quizzes[Random.Range(0, quizzes.Count)];
        quizPanelUI.Show(quiz, onQuizEnd);
    }
}
