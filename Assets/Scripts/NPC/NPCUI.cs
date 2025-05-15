using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class NPCUI : MonoBehaviour
{
    public List<Button> choiceButtons; // Inspector에서 위→아래 순서로 할당
    public GameObject dialoguePanel;
    public Text dialogueText;

    private NPCController currentNPC;
    private int currentChoiceIndex = 0;
    private bool isChoiceActive = false;

    private bool inputBuffer = false; // 상호작용 + 대화 선택지 확정 + 대화 확인 등 모든 걸 f키로 하기 때문에 f키 한 번만 눌러도 모든 게 실행 돼버리는 거 방지용

    void Start()
    {

    }
    void Update()
    {
        // 대화 패널이 떠 있을 때 F키로 대화 종료
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            currentNPC?.EndDialogue();
        }

        // 대화 선택지 관련 -----------------------------------
        if (!isChoiceActive) return;

        // 선택지 UI가 활성화된 직후 첫 프레임은 F키 입력 무시 > F키 한 번 눌렀는데 여러 번 처리되는거 방지
        if (inputBuffer)
        {
            if (!Input.GetKey(KeyCode.F)) // F키에서 손을 뗄 때까지 대기
                inputBuffer = false;
            return;
        }

        // 대화 선택지 선택
        // 위로 이동 (W)
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentChoiceIndex = (currentChoiceIndex - 1 + choiceButtons.Count) % choiceButtons.Count;
            HighlightChoice(currentChoiceIndex);
        }
        // 아래로 이동 (S)
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentChoiceIndex = (currentChoiceIndex + 1) % choiceButtons.Count;
            HighlightChoice(currentChoiceIndex);
        }
        // 선택 (F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            inputBuffer = true;
            choiceButtons[currentChoiceIndex].onClick.Invoke();
        }
    }

    // NPCController의 이벤트 구독
    public void ConnectToNPC(NPCController npc)
    {
        // 이전 NPC 이벤트 해제
        if (currentNPC != null)
        {
            currentNPC.OnInteractionStarted -= ShowChoices;
            currentNPC.OnInteractionCanceled -= HideChoices;
            currentNPC.OnDialogueStarted -= ShowDialogueUI;
            currentNPC.OnDialogueEnded -= HideDialogueUI;
        }

        currentNPC = npc;

        // 새로운 NPC 이벤트 구독
        currentNPC.OnInteractionStarted += ShowChoices;
        currentNPC.OnInteractionCanceled += HideChoices;
        currentNPC.OnDialogueStarted += ShowDialogueUI;
        currentNPC.OnDialogueEnded += HideDialogueUI;
    }

    // 대화 선택지 UI 표시
    private void ShowChoices(NPCController npc)
    {
        HideDialogueUI();
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(true);

        isChoiceActive = true;
        inputBuffer = true; // 선택지 UI가 뜨는 순간 F키 입력 무시
        currentChoiceIndex = 0;
        HighlightChoice(currentChoiceIndex);
    }

    // 선택지 UI 숨김
    private void HideChoices()
    {
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);

        isChoiceActive = false;
    }

    // 대화창 표시
    private void ShowDialogueUI()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "안녕하세요";
        HideChoices();
    }

    // 대화창 숨김
    private void HideDialogueUI()
    {
        dialoguePanel.SetActive(false);
    }

    // 하이라이트(선택) 표시
    private void HighlightChoice(int idx)
    {
        // EventSystem을 이용해 버튼 하이라이트
        EventSystem.current.SetSelectedGameObject(choiceButtons[idx].gameObject);
    }

    public void OnTalkBtnClicked()
    {
        currentNPC?.StartDialogue();
    }

    public void OnCancelBtnClicked()
    {
        currentNPC?.CancelInteraction();
    }

    public bool getIsChoiceActive()
    {
        return isChoiceActive;
    }
}
