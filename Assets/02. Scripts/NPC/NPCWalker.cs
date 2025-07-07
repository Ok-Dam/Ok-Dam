using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCWalker : MonoBehaviour
{
    /*
    - ���� �������� ���ϰ� �̵�.
    - ��ȭ���̸� ��� ����. ��ȭ�� ������ �簳.
    - �̵� �������� ���� Ȯ���� ����ٰ� �ٽ� �̵�.
    - ������ ������ ��� ����ٰ� �� ������ ���� �� �̵�. 
    - ���� �ð� ������ �������� ���ư��� �����
     */

    NavMeshAgent agent;
    Animator animator;
    NPCController controller;

    // �߰�: �����ʿ� ��������Ʈ ����
    private NPCSpawner spawner;
    private Vector3 spawnPoint;

    private bool prevIsWalking = false; // ���� �������� NPC �ȱ� ���¸� ����. ����/�̵� �Ǻ���
    private bool isWaiting = false; // ������ ���� �� ��� ������.
    private bool isPaused = false; // �̵� ���� ���� �������� (�ڿ��������� ����)
    private Vector3 currentDestination;

    // NPC�� ���� �� ���� �ð��� ������ �Ա��� ���ƿͼ� ������
    private float wanderTime; // min�� max ���� random
    [SerializeField] private float minWanderTime = 30;
    [SerializeField] private float maxWanderTime = 60;

    private float wanderTimer = 0; // ��� �ð�
    private bool returningToSpawn = false; // ���� �� ����

    private bool despawning = false;

    // NPC�� �̵� �� ��� ����ٰ� �̵��ϰ� ��
    private float pauseCheckInterval = 1.5f; // 1.5�ʸ��� �˻�
    private float pauseChance = 0.12f;       // 12% Ȯ��

    // ���� �ð� ����.
    public float minPauseTime = 1f;
    public float maxPauseTime = 2f;

    // �������� �����ص� ���� �ð� ������ �ִٰ� �� �������� ����
    public float waitAtDestinationMin = 2f;
    public float waitAtDestinationMax = 5f;

    // Init �Լ�: �����ʿ� ��������Ʈ ����
    public void Init(NPCSpawner spawner, Vector3 spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
        this.wanderTime = Random.Range(minWanderTime, maxWanderTime); // 1~2�� ����
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

        // wanderTimer üũ. �ð� �� �Ǹ� ���� ����Ʈ�� ���� ����
        if (!returningToSpawn)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderTime)
            {
                returningToSpawn = true;
                agent.SetDestination(spawnPoint); // ���� ����
            }
        }
        else
        {
            // ���� ��: ��������Ʈ�� �����ϸ� ����
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Despawn();
                return;
            }
        }

        bool isTalking = controller != null && controller.getIsNPCTalking();

        // 1. ��ȭ ���̸� ��� ����(Idle)
        if (isTalking)
        {
            agent.isStopped = true;
            SetWalkingAnimation(false);
            return;
        }
        else
        {
            // ��ȭ�� ������ �̵� �簳
            if (!isPaused && agent.isStopped)
                agent.isStopped = false;
        }

        // 2. ������ ���� �� ��� (���� ���� �ƴ� ����)
        if (!returningToSpawn && !isWaiting && !isPaused && !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            float waitTime = Random.Range(waitAtDestinationMin, waitAtDestinationMax);
            StartCoroutine(WaitAndMove(waitTime));
        }

        // �ִϸ��̼� ���� ��ȭ �ÿ��� ����
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

    // ������ ���� �� ���, �� �� �� �������� �̵�
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

    // �̵� �� ���� ���� (���� �������� �簳)
    IEnumerator PauseCoroutine(float waitTime)
    {
        isPaused = true;
        agent.isStopped = true;
        SetWalkingAnimation(false);
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        isPaused = false;
        // �������� �״��, �̵� �簳
    }

    // ���� �ð����� �̵� �� ���� ���� �˻�
    IEnumerator RandomPauseCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(pauseCheckInterval);

            // �̵� ��, ���/�Ͻ�����/��ȭ ���� �ƴ� ���� �˻�
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

    // �ִϸ����� �Ķ���� ����ȭ
    void SetWalkingAnimation(bool isWalking)
    {
        if (isWalking != prevIsWalking)
        {
            animator.SetBool("isWalking", isWalking);
            prevIsWalking = isWalking;
        }
    }

    // ���� ó�� (��� ����)
    void Despawn()
    {
        despawning = true;
        if (spawner != null)
            spawner.OnNPCDespawn(gameObject);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            // ���������� 30�� ȸ��
            transform.Rotate(0f, 30f, 0f);

            // ���ο� �������� �̵��ϰ� �ϰ� ������ �Ʒ��� �߰�
            // SetNewDestination();
        }
    }
}
