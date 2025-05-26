using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizPanelUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Image> questionImages; // 여러 질문 이미지 지원
    public List<Button> choiceButtons; // 버튼 수만큼 Inspector에서 할당
    public TextMeshProUGUI explanationText;
    public Button closeButton;

    // 선택지 버튼 내부에 Image, TextMeshProUGUI 모두 넣어둘 것!
    // (둘 다 활성화/비활성화로 상황에 따라 표시)

    private System.Action<bool> onQuizEnd;
    private bool isCorrectCache; // 마지막 선택 결과 캐싱

    public void Show(QuizData quiz, System.Action<bool> onEnd)
    {
        gameObject.SetActive(true);
        onQuizEnd = onEnd;
        isCorrectCache = false; // 초기화

        // 질문 텍스트
        questionText.text = quiz.questionText;

        // 질문 이미지(여러 개 지원)
        for (int i = 0; i < questionImages.Count; i++)
        {
            if (quiz.questionImages != null && i < quiz.questionImages.Count && quiz.questionImages[i] != null)
            {
                questionImages[i].sprite = quiz.questionImages[i];
                questionImages[i].gameObject.SetActive(true);
            }
            else
            {
                questionImages[i].gameObject.SetActive(false);
            }
        }

        // 해설 비활성화
        explanationText.gameObject.SetActive(false);

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
                bool isCorrect = (idx == quiz.correctIndex);
                isCorrectCache = isCorrect; // 결과 캐싱
                explanationText.text = (isCorrect ? "정답!\n" : "오답!\n") + quiz.explanation;
                explanationText.gameObject.SetActive(true);
                foreach (var b in choiceButtons) b.interactable = false;
                closeButton.gameObject.SetActive(true);
            });
        }

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
            // 퀴즈를 닫을 때 콜백 실행
            onQuizEnd?.Invoke(isCorrectCache);
        });
        closeButton.gameObject.SetActive(false);
    }
}
