using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizPanelUI : MonoBehaviour
{
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Sprite questionImages;
    public List<Button> choiceButtons;
    public Button closeButton;
    public Image questionImageUI;

    public GameObject expPanel;
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI buffTxt;

    private System.Action<bool> onQuizEnd;
    private bool isCorrectCache;
    public string lastExplanation;

    public void Show(QuizData quiz, System.Action<bool> onEnd)
    {
        gameObject.SetActive(true);
        onQuizEnd = onEnd;
        isCorrectCache = false;
        lastExplanation = quiz.explanation;

        questionText.text = quiz.questionText;

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
                onQuizEnd?.Invoke(isCorrectCache);
            });
        }
    }

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

    // 정보 안내 전용 함수 추가
    public void ShowInformation(string infoText, Sprite image, System.Action onEnd)
    {
        gameObject.SetActive(true);
        questionText.text = infoText;

        if (image != null)
        {
            questionImageUI.sprite = image;
            questionImageUI.gameObject.SetActive(true);
        }
        else
        {
            questionImageUI.gameObject.SetActive(false);
        }

        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        expPanel.SetActive(false);

        closeButton.gameObject.SetActive(true);
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onEnd?.Invoke();
        });
    }
}
