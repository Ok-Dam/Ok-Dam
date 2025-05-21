using UnityEngine;
using UnityEngine.AI;

public class Squirrel : MonoBehaviour
{
    private NavMeshAgent agent;
    public float roamRadius = 5f;  // 다람쥐가 돌아다닐 반경
    public float waitTime = 2f;     // 목표에 도달 후 기다릴 시간
    private float waitTimer;

    void Start()
    {
        // NavMeshAgent 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = 0f;  // Y축에 문제 없도록 설정
        agent.autoBraking = false;  // 속도 감소 방지
        agent.stoppingDistance = 0.1f;  // 목표에 조금 더 가까워졌을 때 멈추기
        SetNewDestination();  // 첫 번째 목표 설정
    }

    void Update()
    {
        // 목적지에 도달하면 기다린 후 새로운 목표 설정
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                SetNewDestination();
                waitTimer = 0f;
            }
        }
    }

    void SetNewDestination()
    {
        // 랜덤 위치 계산
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        // y값을 현재 다람쥐의 y값으로 고정 (하늘로 날아가지 않도록)
        randomDirection.y = transform.position.y;

        // 유효한 NavMesh 위치인지 확인
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            // 새로운 목적지 설정
            agent.SetDestination(hit.position);
        }
        else
        {
            // NavMesh 위에 위치하지 않으면 다시 시도
            SetNewDestination();
        }
    }
}
