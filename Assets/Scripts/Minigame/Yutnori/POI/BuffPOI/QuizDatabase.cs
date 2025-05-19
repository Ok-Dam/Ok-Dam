using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuizDatabase", menuName = "Quiz/QuizDatabase", order = 2)]
public class QuizDatabase : ScriptableObject
{
    public List<QuizData> quizzes;
}
