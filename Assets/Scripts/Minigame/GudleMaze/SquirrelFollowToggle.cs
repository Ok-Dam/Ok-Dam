using UnityEngine;
using UnityEngine.AI;

public class SquirrelFollowToggle : MonoBehaviour
{
    public Transform player;              // 따라갈 대상 (카메라 또는 플레이어 오브젝트)
    public Transform[] wanderPoints;      // 자유롭게 돌아다닐 위치들
    private NavMeshAgent agent;
    private int pressCount = 0;
    private int currentWanderIndex = 0;
    private bool isFollowing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextWanderPoint();
    }

    void Update()
    {
        // Q키 누르면 토글
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pressCount++;
            isFollowing = (pressCount % 2 == 1);

            if (isFollowing)
            {
                // 플레이어 따라감
                agent.SetDestination(player.position);
            }
            else
            {
                // 자유 움직임 복귀
                GoToNextWanderPoint();
            }
        }

        // 자유롭게 돌아다니기
        if (!isFollowing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextWanderPoint();
        }

        // 따라가는 중이면 계속 플레이어 위치로 업데이트
        if (isFollowing)
        {
            agent.SetDestination(player.position);
        }
    }

    void GoToNextWanderPoint()
    {
        if (wanderPoints.Length == 0) return;

        agent.destination = wanderPoints[currentWanderIndex].position;
        currentWanderIndex = (currentWanderIndex + 1) % wanderPoints.Length;
    }
}
