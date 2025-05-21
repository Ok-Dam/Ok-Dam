using UnityEngine;
using UnityEngine.AI;  // NavMesh를 사용하려면 반드시 추가해야 합니다.

public class AIRandomMovement : MonoBehaviour
{
    public float changeInterval = 4f;  // 4초마다 애니메이션 상태 및 회전 변경
    private float timer = 0f;  // 타이머

    // 애니메이션과 회전, 위치 값들
    private Vector3 rotation1 = new Vector3(0, 715.33f, 0);
    private Vector3 rotation2 = new Vector3(0, 713.67f, 0);
    private Vector3 rotation3 = new Vector3(-244.8f, 729.25f, -715.8f);
    private float positionY = 34.92f;

    private int currentState = 0;  // 애니메이션 상태 추적 (0: 상태 1, 1: 상태 2, 2: 상태 3)
    private Animator animator;
    private NavMeshAgent agent;  // NavMeshAgent 추가

    public Transform[] targetPoints;  // 이동할 목표 지점들
    private int currentTargetIndex = 0;  // 현재 목표 지점 인덱스

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();  // NavMeshAgent 컴포넌트 할당

        // 첫 번째 목표 지점으로 이동 시작
        if (targetPoints.Length > 0)
        {
            agent.SetDestination(targetPoints[currentTargetIndex].position);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;  // 경과 시간 계산

        // 4초마다 회전 및 애니메이션 변경
        if (timer >= changeInterval)
        {
            timer = 0f;  // 타이머 초기화
            ChangeRotationAndAnimation();  // 회전 값과 애니메이션 변경
        }

        // NavMeshAgent가 목표 지점에 가까워지면 다음 목표로 이동
        if (agent.remainingDistance < 1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Length;  // 목표 순환
            agent.SetDestination(targetPoints[currentTargetIndex].position);  // 다음 목표 지점으로 이동
        }

        // 이동 상태에 맞는 애니메이션 처리
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }

    // 회전 및 애니메이션 상태를 변경하는 함수
    private void ChangeRotationAndAnimation()
    {
        currentState = (currentState + 1) % 4;  // 상태 순환 (0 -> 1 -> 2 -> 3)

        switch (currentState)
        {
            case 0:
                // 상태 1: Y_ROTATION = 715.33
                transform.rotation = Quaternion.Euler(rotation1);
                animator.SetTrigger("Idle");  // 애니메이션 변경 (Idle)
                break;

            case 1:
                // 상태 2: Y_ROTATION = 713.67
                transform.rotation = Quaternion.Euler(rotation2);
                animator.SetTrigger("Walk");  // 애니메이션 변경 (Walk)
                break;

            case 2:
                // 상태 3: X_ROTATION = -244.8, Y_ROTATION = 729.25, Z_ROTATION = -715.8
                transform.rotation = Quaternion.Euler(rotation3);
                animator.SetTrigger("Run");  // 애니메이션 변경 (Run)
                break;

            case 3:
                // 상태 4: Y_POSITION = 34.92
                transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
                animator.SetTrigger("Attack");  // 애니메이션 변경 (Attack)
                break;
        }
    }
}
