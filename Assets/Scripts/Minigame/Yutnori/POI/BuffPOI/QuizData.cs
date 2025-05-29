using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "Quiz/QuizData", order = 1)]
public class QuizData : ScriptableObject
{
    public int nodeNumber;
    [TextArea] public string questionText;
    public List<Sprite> questionImages; // ���� �̹��� �巡�� ����
    public List<string> choices;        // �ؽ�Ʈ ������
    public List<Sprite> choiceImages;   // �̹��� ������
    public int correctIndex;
    [TextArea] public string explanation;
}
