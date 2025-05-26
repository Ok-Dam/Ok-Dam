using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizPanelUI : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Image> questionImages; // ���� ���� �̹��� ����
    public List<Button> choiceButtons; // ��ư ����ŭ Inspector���� �Ҵ�
    public TextMeshProUGUI explanationText;
    public Button closeButton;

    // ������ ��ư ���ο� Image, TextMeshProUGUI ��� �־�� ��!
    // (�� �� Ȱ��ȭ/��Ȱ��ȭ�� ��Ȳ�� ���� ǥ��)

    private System.Action<bool> onQuizEnd;
    private bool isCorrectCache; // ������ ���� ��� ĳ��

    public void Show(QuizData quiz, System.Action<bool> onEnd)
    {
        gameObject.SetActive(true);
        onQuizEnd = onEnd;
        isCorrectCache = false; // �ʱ�ȭ

        // ���� �ؽ�Ʈ
        questionText.text = quiz.questionText;

        // ���� �̹���(���� �� ����)
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

        // �ؼ� ��Ȱ��ȭ
        explanationText.gameObject.SetActive(false);

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
                bool isCorrect = (idx == quiz.correctIndex);
                isCorrectCache = isCorrect; // ��� ĳ��
                explanationText.text = (isCorrect ? "����!\n" : "����!\n") + quiz.explanation;
                explanationText.gameObject.SetActive(true);
                foreach (var b in choiceButtons) b.interactable = false;
                closeButton.gameObject.SetActive(true);
            });
        }

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
            // ��� ���� �� �ݹ� ����
            onQuizEnd?.Invoke(isCorrectCache);
        });
        closeButton.gameObject.SetActive(false);
    }
}
