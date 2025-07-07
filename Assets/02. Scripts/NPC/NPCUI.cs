using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class NPCUI : MonoBehaviour
{
    public List<Button> choiceButtons;
    public GameObject dialoguePanel;
    public Text dialogueText;
    public TMP_InputField inputField;
    public GptRequester gpt;
    public static bool IsTalkingToNPC { get; private set; } = false;

    private NPCController currentNPC;
    private int currentChoiceIndex = 0;
    private bool isChoiceActive = false;
    private bool inputBuffer = false;

    void Start()
    {
        inputField.gameObject.SetActive(false);

        // ✅ Enter 눌렀을 때 GPT로 전송되도록 설정
        inputField.onSubmit.AddListener(OnSubmitMessage);
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            currentNPC?.EndDialogue();
        }

        if (!isChoiceActive) return;

        if (inputBuffer)
        {
            if (!Input.GetKey(KeyCode.F)) inputBuffer = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentChoiceIndex = (currentChoiceIndex - 1 + choiceButtons.Count) % choiceButtons.Count;
            HighlightChoice(currentChoiceIndex);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentChoiceIndex = (currentChoiceIndex + 1) % choiceButtons.Count;
            HighlightChoice(currentChoiceIndex);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            inputBuffer = true;
            choiceButtons[currentChoiceIndex].onClick.Invoke();
        }
    }

    public void ConnectToNPC(NPCController npc)
    {
        if (currentNPC != null)
        {
            currentNPC.OnInteractionStarted -= ShowChoices;
            currentNPC.OnInteractionCanceled -= HideChoices;
            currentNPC.OnDialogueStarted -= ShowDialogueUI;
            currentNPC.OnDialogueEnded -= HideDialogueUI;
        }

        currentNPC = npc;

        currentNPC.OnInteractionStarted += ShowChoices;
        currentNPC.OnInteractionCanceled += HideChoices;
        currentNPC.OnDialogueStarted += ShowDialogueUI;
        currentNPC.OnDialogueEnded += HideDialogueUI;
    }

    private void ShowChoices(NPCController npc)
    {
        HideDialogueUI();

        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(true);

        isChoiceActive = true;
        inputBuffer = true;
        currentChoiceIndex = 0;
        HighlightChoice(currentChoiceIndex);
    }

    private void HideChoices()
    {
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        isChoiceActive = false;
    }

    private void ShowDialogueUI()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "🤖 NPC: 안녕하세요! 무엇이 궁금하신가요?";
        IsTalkingToNPC = true;

        inputField.gameObject.SetActive(true);
        inputField.text = "";
        inputField.interactable = true;
        inputField.Select();
        inputField.ActivateInputField();

        StartCoroutine(FocusInputDelayed());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HideChoices();
    }

    private void HideDialogueUI()
    {
        dialoguePanel.SetActive(false);
        inputField.gameObject.SetActive(false);
        inputField.text = "";
        IsTalkingToNPC = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HighlightChoice(int idx)
    {
        EventSystem.current.SetSelectedGameObject(choiceButtons[idx].gameObject);
    }

    public void OnTalkBtnClicked()
    {
        currentNPC?.StartDialogue();
        ShowDialogueUI();
    }

    public void OnCancelBtnClicked()
    {
        currentNPC?.CancelInteraction();
    }

    public bool getIsChoiceActive()
    {
        return isChoiceActive;
    }

    private void OnSubmitMessage(string message)
    {
        string userMessage = message.Trim();
        if (string.IsNullOrEmpty(userMessage)) return;

        inputField.text = "";
        dialogueText.text = "🤖 NPC가 생각 중...";

        gpt.OnGptResponse = (response) =>
        {
            dialogueText.text = "🤖 NPC: " + response;
            inputField.Select();
            inputField.ActivateInputField();

            var scrollRect = dialoguePanel.GetComponentInChildren<ScrollRect>();
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 1f;
        };

        gpt.RequestGpt(userMessage);
    }

    private IEnumerator FocusInputDelayed()
    {
        yield return null;

        inputField.Select();
        inputField.ActivateInputField();
    }
}
