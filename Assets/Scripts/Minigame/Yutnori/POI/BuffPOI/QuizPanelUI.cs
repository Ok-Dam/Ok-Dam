
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizPanelUI : MonoBehaviour
{
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Sprite questionImages; // ���� ���� �̹��� ����
    public List<Button> choiceButtons; // ��ư ����ŭ Inspector���� �Ҵ�
    public Button closeButton; // ���� �г��� Ȯ�� ��ư
    public Image questionImageUI;

    // ExpPanel ����
    public GameObject expPanel; // ExpPanel ������Ʈ (���� �гΰ� ����)
    public TextMeshProUGUI explanationText; // ExpPanel�� �ؼ� �ؽ�Ʈ
    public TextMeshProUGUI buffTxt; // ExpPanel�� ���� �ؽ�Ʈ

    private System.Action<bool> onQuizEnd;
    private bool isCorrectCache; // ������ ���� ��� ĳ��
    public string lastExplanation; // ������ �ؼ� ĳ��

    public void Show(QuizData quiz, System.Action<bool> onEnd)
    {
        gameObject.SetActive(true);
        onQuizEnd = onEnd;
        isCorrectCache = false;
        lastExplanation = quiz.explanation;

        // ���� �ؽ�Ʈ

        questionText.text = quiz.questionText;



        // ���� �̹���(���� �� ����)
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



        // ������ ǥ��
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


    /// �ؼ�/���� UI(ExpPanel) ǥ��, �ݱ� ��ư������ onClose ȣ��
    public void ShowExpPanel(bool isCorrect, string explanation, string buffDescription, System.Action onClose)
    {
        expPanel.SetActive(true);
        explanationText.text = (isCorrect ? "����!\n" : "����!\n") + explanation;
        buffTxt.text = buffDescription;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            expPanel.SetActive(false);
            quizPanel.SetActive(false);
            onClose?.Invoke();
        });
    }
}
