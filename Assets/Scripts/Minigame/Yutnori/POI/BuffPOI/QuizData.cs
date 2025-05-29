using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "Quiz/QuizData", order = 1)]
public class QuizData : ScriptableObject
{
    public int nodeNumber;
    [TextArea] public string questionText;
    public List<Sprite> questionImages; // 여러 이미지 드래그 가능
    public List<string> choices;        // 텍스트 선택지
    public List<Sprite> choiceImages;   // 이미지 선택지
    public int correctIndex;
    [TextArea] public string explanation;
}
