using UnityEngine;
using System;

public class NPCController : MonoBehaviour
{
    private Animator animator;
    private Transform playerTransform;
    private PlayerController playerController;

    // UI 매니저와 연결할 이벤트
    public event Action<NPCController> OnInteractionStarted;
    public event Action OnInteractionCanceled;
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    private bool isNPCTalking = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 플레이어가 생성된 후 자동으로 찾아서 연결
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {        
            playerController= player.GetComponent<PlayerController>();
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 혹시 플레이어가 아직 생성 안되었을 때를 대비해서 다시 시도
        if (playerTransform == null || playerController == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
                playerTransform = player.transform;
            }
            else
            {
                return; // 플레이어 없으면 실행 안 함
            }
        }
    }

        // F키로 NPC 상호작용 시작. Playerinteraction.cs에서 호출
        public void InteractWithPlayer()
    {
        // 애니메이션 Any State > Idle
        ForceIdle();

        isNPCTalking = true;

        // NPC가 플레이어 바라보기
        Vector3 lookPos = playerTransform.position;
        lookPos.y = transform.position.y; // Y축 고정(회전만)
        transform.LookAt(lookPos);

        // UI 표시 이벤트 발송. UI 매니저가 NPCController의 이벤트를 구독중임
        playerController.setIsTalking(true);
        OnInteractionStarted?.Invoke(this);
    }

    // 대화 종료 버튼시. 아예 대화 ui 끌 때
    public void CancelInteraction()
    {
        playerController.setIsTalking(false); // 이동 못 하게 하는 bool
        isNPCTalking = false;
        OnInteractionCanceled?.Invoke();
    }

    // 대화 버튼시
    public void StartDialogue()
    {
        animator.SetBool("isTalking", true); // 애니메이션 관련 bool
        OnDialogueStarted?.Invoke();
    }

    // F키(대화 중 종료) 입력 시. 대화 자막 끄고 다시 선택지로 돌아갈때
    public void EndDialogue()
    {
        animator.SetBool("isTalking", false);
        OnDialogueEnded?.Invoke();
        OnInteractionStarted?.Invoke(this); // 다시 대화 선택지 뜸
    }

    // 애니메이션 Idle로 강제 전환
    public void ForceIdle()
    {
        animator.SetTrigger("forceIdle");
    }

    public bool getIsNPCTalking() { return isNPCTalking; }
}
