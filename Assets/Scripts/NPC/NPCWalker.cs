using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCWalker : MonoBehaviour
{
    /*
    - 일정 목적지를 정하고 이동.
    - 대화중이면 잠시 정지. 대화가 끝나고 재개.
    - 이동 과정에서 랜덤 확률로 멈췄다가 다시 이동.
    - 목적지 도착시 잠시 멈췄다가 새 목적지 설정 후 이동. 
     */


    NavMeshAgent agent;
    Animator animator;
    NPCController controller;

    private bool prevIsWalking = false; // 이전 프레임의 NPC 걷기 상태를 저장. 정지/이동 판별용
    private bool isWaiting = false; // 목적지 도착 후 대기 중인지.
    private bool isPaused = false; // 이동 도중 멈춘 상태인지 (자연스러움을 위해)
    private Vector3 currentDestination;

    // 검사 주기와 확률. 몇 초마다 몇 퍼 확률로 잠깐 멈출 건지.
    private float pauseCheckInterval = 1.5f; // 1.5초마다 검사
    private float pauseChance = 0.12f;       // 12% 확률

    // 정지 시간 범위. 3~6초 사이 멈춤.
    public float minPauseTime = 3f;
    public float maxPauseTime = 6f;
    // 목적지 도착 후 대기 시간도 동일하게 적용
    public float waitAtDestinationMin = 3f;
    public float waitAtDestinationMax = 6f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        controller = GetComponent<NPCController>();

        SetNewDestination();
        StartCoroutine(RandomPauseCheckRoutine());
    }

    void Update()
    {
        bool isTalking = controller != null && controller.getIsNPCTalking();

        // 1. 대화 중이면 즉시 멈춤(Idle)
        if (isTalking)
        {
            agent.isStopped = true;
            SetWalkingAnimation(false);
            return;
        }
        else
        {
            // 대화가 끝나면 이동 재개
            if (!isPaused && agent.isStopped)
                agent.isStopped = false;
        }

        // 2. 목적지 도착 시 대기
        if (!isWaiting && !isPaused && !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            float waitTime = Random.Range(waitAtDestinationMin, waitAtDestinationMax);
            StartCoroutine(WaitAndMove(waitTime));
        }

        // 애니메이션 상태 변화 시에만 갱신
        bool isWalking = !agent.isStopped && agent.velocity.magnitude > 0.1f;
        SetWalkingAnimation(isWalking);
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            currentDestination = hit.position;
            agent.SetDestination(currentDestination);
        }
    }

    // 목적지 도착 후 대기, 그 후 새 목적지로 이동
    IEnumerator WaitAndMove(float waitTime)
    {
        isWaiting = true;
        agent.isStopped = true;
        SetWalkingAnimation(false);
        yield return new WaitForSeconds(waitTime);
        SetNewDestination();
        agent.isStopped = false;
        isWaiting = false;
    }

    // 이동 중 랜덤 멈춤 (같은 목적지로 재개)
    IEnumerator PauseCoroutine(float waitTime)
    {
        isPaused = true;
        agent.isStopped = true;
        SetWalkingAnimation(false);
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        isPaused = false;
        // 목적지는 그대로, 이동 재개
    }

    // 1.5초마다 이동 중 랜덤 멈춤 검사
    IEnumerator RandomPauseCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(pauseCheckInterval);

            // 이동 중, 대기/일시정지/대화 중이 아닐 때만 검사
            if (!isPaused && !isWaiting && agent.hasPath && agent.velocity.magnitude > 0.1f)
            {
                if (Random.value < pauseChance)
                {
                    float pauseTime = Random.Range(minPauseTime, maxPauseTime);
                    StartCoroutine(PauseCoroutine(pauseTime));
                }
            }
        }
    }

    // 애니메이터 파라미터 최적화
    void SetWalkingAnimation(bool isWalking)
    {
        if (isWalking != prevIsWalking)
        {
            animator.SetBool("isWalking", isWalking);
            prevIsWalking = isWalking;
        }
    }
}
