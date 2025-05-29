
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizPanelUI : MonoBehaviour
{
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Sprite questionImages; // 여러 질문 이미지 지원
    public List<Button> choiceButtons; // 버튼 수만큼 Inspector에서 할당
    public Button closeButton; // 퀴즈 패널의 확인 버튼
    public Image questionImageUI;

    // ExpPanel 관련
    public GameObject expPanel; // ExpPanel 오브젝트 (퀴즈 패널과 별도)
    public TextMeshProUGUI explanationText; // ExpPanel의 해설 텍스트
    public TextMeshProUGUI buffTxt; // ExpPanel의 버프 텍스트

    private System.Action<bool> onQuizEnd;
    private bool isCorrectCache; // 마지막 선택 결과 캐싱
    public string lastExplanation; // 마지막 해설 캐싱

    public void Show(QuizData quiz, System.Action<bool> onEnd)
    {
        gameObject.SetActive(true);
        onQuizEnd = onEnd;
        isCorrectCache = false;
        lastExplanation = quiz.explanation;

        // 질문 텍스트

        questionText.text = quiz.questionText;



        // 질문 이미지(여러 개 지원)
        if (quiz.questionImages != null)
            questionImages = quiz.questionImages;
        if (quiz.questionImages != null)
        {
            questionImageUI.sprite = quiz.questionImages;
            questionImageUI.gameObject.SetActive(true);
        }
        else
        {
            questionImageUI.gameObject.SetActive(false);
        }



        // 선택지 표시
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var btn = choiceButtons[i];
            bool hasText = quiz.choices != null && i < quiz.choices.Count && !string.IsNullOrEmpty(quiz.choices[i]);
            bool hasImage = quiz.choiceImages != null && i < quiz.choiceImages.Count && quiz.choiceImages[i] != null;

            btn.gameObject.SetActive(hasText || hasImage);

            var btnImage = btn.GetComponentsInChildren<Image>(true)
            .FirstOrDefault(img => img.gameObject != btn.gameObject);
            var btnText = btn.GetComponentInChildren<TextMeshProUGUI>(true);

            if (hasImage)
            {
                btnImage.sprite = quiz.choiceImages[i];
                btnImage.gameObject.SetActive(true);
                btnText.gameObject.SetActive(false);
            }
            else if (hasText)
            {
                btnText.text = quiz.choices[i];
                btnText.gameObject.SetActive(true);
                btnImage.gameObject.SetActive(false);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }

            btn.interactable = true;
            int idx = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                isCorrectCache = (idx == quiz.correctIndex);
                foreach (var b in choiceButtons) b.interactable = false;
                ShowExpPanel(isCorrectCache, lastExplanation, "", () => {
                    quizPanel.SetActive(false);
                    onQuizEnd?.Invoke(isCorrectCache);
                });
            });
        }
    }


    /// 해설/버프 UI(ExpPanel) 표시, 닫기 버튼에서만 onClose 호출
    public void ShowExpPanel(bool isCorrect, string explanation, string buffDescription, System.Action onClose)
    {
        expPanel.SetActive(true);
        explanationText.text = (isCorrect ? "정답!\n" : "오답!\n") + explanation;
        buffTxt.text = buffDescription;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            expPanel.SetActive(false);
            quizPanel.SetActive(false);
            onClose?.Invoke();
        });
    }
}
