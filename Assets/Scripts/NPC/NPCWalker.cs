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
    - 일정 시간 지나면 스폰으로 돌아가서 사라짐
     */

    NavMeshAgent agent;
    Animator animator;
    NPCController controller;

    // 추가: 스포너와 스폰포인트 저장
    private NPCSpawner spawner;
    private Vector3 spawnPoint;

    private bool prevIsWalking = false; // 이전 프레임의 NPC 걷기 상태를 저장. 정지/이동 판별용
    private bool isWaiting = false; // 목적지 도착 후 대기 중인지.
    private bool isPaused = false; // 이동 도중 멈춘 상태인지 (자연스러움을 위해)
    private Vector3 currentDestination;

    // NPC는 스폰 후 일정 시간이 지나면 입구로 돌아와서 디스폰됨
    private float wanderTime; // min과 max 사이 random
    [SerializeField] private float minWanderTime = 30;
    [SerializeField] private float maxWanderTime = 60;

    private float wanderTimer = 0; // 경과 시간
    private bool returningToSpawn = false; // 복귀 중 상태

    private bool despawning = false;

    // NPC는 이동 중 잠깐 멈췄다가 이동하곤 함
    private float pauseCheckInterval = 1.5f; // 1.5초마다 검사
    private float pauseChance = 0.12f;       // 12% 확률

    // 정지 시간 범위.
    public float minPauseTime = 1f;
    public float maxPauseTime = 2f;

    // 목적지에 도착해도 일정 시간 가만히 있다가 새 목적지를 정함
    public float waitAtDestinationMin = 2f;
    public float waitAtDestinationMax = 5f;

    // Init 함수: 스포너와 스폰포인트 저장
    public void Init(NPCSpawner spawner, Vector3 spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
        this.wanderTime = Random.Range(minWanderTime, maxWanderTime); // 1~2분 랜덤
        this.wanderTimer = 0f;
        this.returningToSpawn = false;
    }

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
        if (despawning) return;

        // wanderTimer 체크. 시간 다 되면 스폰 포인트로 가서 디스폰
        if (!returningToSpawn)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderTime)
            {
                returningToSpawn = true;
                agent.SetDestination(spawnPoint); // 복귀 시작
            }
        }
        else
        {
            // 복귀 중: 스폰포인트에 도달하면 디스폰
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Despawn();
                return;
            }
        }

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

        // 2. 목적지 도달 시 대기 (복귀 중이 아닐 때만)
        if (!returningToSpawn && !isWaiting && !isPaused && !agent.pathPending &&
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
        if (!returningToSpawn)
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

    // 일정 시간마다 이동 중 랜덤 멈춤 검사
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

    // 디스폰 처리 (즉시 삭제)
    void Despawn()
    {
        despawning = true;
        if (spawner != null)
            spawner.OnNPCDespawn(gameObject);
        Destroy(gameObject);
    }
}
